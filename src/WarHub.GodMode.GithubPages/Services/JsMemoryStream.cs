using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Microsoft.JSInterop.WebAssembly;

namespace WarHub.GodMode.GithubPages.Services
{
    public class JsMemoryStream : Stream
    {
        private bool isDisposed = false;
        private long position = 0;
        private readonly IJsMemoryInstanceInterop jsMemory;

        public JsMemoryStream(IJsMemoryInstanceInterop jsMemory)
        {
            this.jsMemory = jsMemory ?? throw new ArgumentNullException(nameof(jsMemory));
        }

        public override bool CanRead => !isDisposed;

        public override bool CanSeek => !isDisposed;

        public override bool CanWrite => false;

        public override long Length => ThrowIfDisposedOrReturn(jsMemory.Length);

        public override long Position
        {
            get => ThrowIfDisposedOrReturn(position);
            set => Seek(ThrowIfDisposedOrReturn(value), SeekOrigin.Begin);
        }

        public override void Flush() => ThrowIfDisposed();

        public override int Read(byte[] buffer, int offset, int count)
        {
            if (buffer is null)
                throw new ArgumentNullException(nameof(buffer));
            if (offset < 0)
                throw new ArgumentOutOfRangeException(nameof(offset), "Must be >= 0.");
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count), "Must be >= 0.");
            if (offset + count > buffer.Length)
                throw new ArgumentException("Sum of offset + count must be <= buffer.Length.");
            ThrowIfDisposed();
            if (position == jsMemory.Length)
            {
                return 0;
            }
            var bytesRead = jsMemory.Read(position, buffer, offset, count);
            position += bytesRead;
            return bytesRead;
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            ThrowIfDisposed();
            var originPos = origin switch
            {
                SeekOrigin.Begin => 0,
                SeekOrigin.Current => position,
                SeekOrigin.End => jsMemory.Length,
                _ => throw new ArgumentOutOfRangeException(nameof(origin), "Unknown SeekOrigin value.")
            };
            var newPos = originPos + offset;
            if (newPos < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(offset), "Position has to be >= 0.");
            }
            if (newPos >= Length)
            {
                throw new ArgumentOutOfRangeException(nameof(offset), "Position has to be < Length.");
            }
            return position = newPos;
        }

        public override void SetLength(long value)
        {
            throw new NotSupportedException();
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            throw new NotSupportedException();
        }

        protected override void Dispose(bool disposing)
        {
            if (!isDisposed)
            {
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                // Fire and forget, this only impacts JS GC
                DisposeAsync();
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                isDisposed = true;
            }
            base.Dispose(disposing);
        }

        public override async ValueTask DisposeAsync()
        {
            if (!isDisposed)
            {
                await jsMemory.DisposeAsync();
                isDisposed = true;
            }
        }

        private void ThrowIfDisposed()
        {
            if (isDisposed)
            {
                throw new ObjectDisposedException(nameof(JsMemoryStream));
            }
        }

        private T ThrowIfDisposedOrReturn<T>(T value)
        {
            ThrowIfDisposed();
            return value;
        }
    }

    public interface IJsMemoryInstanceInterop : IAsyncDisposable
    {
        long Length { get; }

        int Read(long position, byte[] buffer, int offset, int count);

        ValueTask<int> ReadAsync(long position, byte[] buffer, int offset, int count, CancellationToken cancellationToken = default);
    }

    public class JsMemoryInteropService
    {
        public const string Prefix = "JsMemoryInterop";

        private readonly IJSRuntime js;

        public JsMemoryInteropService(IJSRuntime js)
        {
            this.js = js;
        }

        /// <summary>
        /// Pass a delegate which should call a JavaScript function that: retrieves the Uint8Array,
        /// "eval()"s the input string which will return a function, calls that function and passes
        /// your data array to it, returns the result of that function (an integer).
        /// Return that integer from your delegate.
        /// </summary>
        /// <param name="memoryReferenceProvider"></param>
        /// <returns>Stream that can read the Uint8Array passed to eval'ed function.</returns>
        public async Task<Stream> OpenReadAsync(Func<string, Task<int>> memoryReferenceProvider)
        {
            if (memoryReferenceProvider == null)
                throw new ArgumentNullException(nameof(memoryReferenceProvider));

            const string EvalString = "(window." + Prefix + ".PinMemory.bind(window." + Prefix + "))";
            var memRef = await memoryReferenceProvider(EvalString);
            var length = await GetLengthAsync(memRef);
            var memoryInterop = new JsMemoryInstanceInterop(memRef, this, length);
            return new JsMemoryStream(memoryInterop);
        }

        private async ValueTask<long> GetLengthAsync(int memRef)
        {
            return await js.InvokeAsync<long>(Prefix + ".GetLength", memRef);
        }

        private async ValueTask FreeMemory(int memRef)
        {
            await js.InvokeVoidAsync(Prefix + ".UnpinMemory", memRef);
        }

        private int ReadUnmarshalled(int memRef, long position, byte[] buffer, int offset, int count)
        {
            var wasm = (WebAssemblyJSRuntime)js;
            return wasm.InvokeUnmarshalled<JsMemoryParams, int>(Prefix + ".ReadUnmarshalled", new JsMemoryParams
            {
                MemRef = memRef,
                Position = (ulong)position,
                Buffer = buffer,
                Offset = offset,
                Count = count
            });
        }

        [StructLayout(LayoutKind.Explicit)]
        private struct JsMemoryParams
        {
            /// <summary>
            /// Position in JS memory from which to start operation.
            /// </summary>
            [FieldOffset(0)]
            public ulong Position;

            /// <summary>
            /// Position in <see cref="Buffer"/> from which to start operation.
            /// </summary>
            [FieldOffset(8)]
            public int Offset;

            /// <summary>
            /// Number of bytes to copy between JS memory and <see cref="Buffer"/>.
            /// </summary>
            [FieldOffset(12)]
            public int Count;

            /// <summary>
            /// Index of JS memory used.
            /// </summary>
            [FieldOffset(16)]
            public int MemRef;

            /// <summary>
            /// Managed buffer.
            /// </summary>
            [FieldOffset(20)]
            public byte[] Buffer;
        }

        private class JsMemoryInstanceInterop : IJsMemoryInstanceInterop
        {
            private readonly int memRef;
            private readonly JsMemoryInteropService interop;
            private bool isDisposed;

            public JsMemoryInstanceInterop(int memRef, JsMemoryInteropService interop, long length)
            {
                this.memRef = memRef;
                this.interop = interop ?? throw new ArgumentNullException(nameof(interop));
                Length = length;
            }

            public long Length { get; }

            public async ValueTask DisposeAsync()
            {
                if (!isDisposed)
                {
                    await interop.FreeMemory(memRef);
                    isDisposed = true;
                }
            }

            public int Read(long position, byte[] buffer, int offset, int count)
            {
                return interop.ReadUnmarshalled(memRef, position, buffer, offset, count);
            }

            public ValueTask<int> ReadAsync(long position, byte[] buffer, int offset, int count, CancellationToken cancellationToken = default)
            {
                throw new NotImplementedException();
            }
        }
    }
}
