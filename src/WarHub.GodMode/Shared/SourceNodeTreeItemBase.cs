using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace WarHub.GodMode.Shared
{
    public abstract class SourceNodeTreeItemBase : ComponentBase
    {
        [Parameter]
        public SourceNodeTreeItemBase ParentItem { get; set; }

        [Parameter]
        public int IndexInParent { get; set; } = -1;

        [CascadingParameter]
        public SourceNodeTree Tree { get; set; }

        public ElementReference FocusableElementRef { get; protected set; }

        protected bool Expanded { get; set; }

        protected bool HasChildren { get; set; }

        protected SourceNodeTreeItemBase[] ChildItems { get; set; }

        public bool Contains(SourceNodeTreeItemBase item)
        {
            while (item is { })
            {
                if (item.ParentItem == this)
                {
                    return true;
                }
                item = item.ParentItem;
            }
            return false;
        }

        protected override void OnParametersSet()
        {
            if (this == Tree.RootItem)
            {
                Expanded = true;
            }
            if (ParentItem is { })
            {
                ParentItem.ChildItems[IndexInParent] = this;
            }
        }

        protected async Task ToggleExpand()
        {
            if (!HasChildren)
            {
                return;
            }
            Expanded = !Expanded;
            if (!Expanded && this.Contains(Tree.ActiveItem))
            {
                await SelectNode();
            }
        }

        protected async Task SelectNode()
        {
            await Tree.SelectNode(this);
        }

        protected async Task OnKeyDown(KeyboardEventArgs e)
        {
            const int PageSteps = 10;
            await (e.Key switch
            {
                "ArrowLeft" => GoLeft(),
                "ArrowUp" => GoUp(1),
                "ArrowRight" => GoRight(),
                "ArrowDown" => GoDown(1),
                "Home" => GoHome(),
                "End" => GoEnd(),
                "PageUp" => GoUp(PageSteps),
                "PageDown" => GoDown(PageSteps),
                "*" => ExpandSiblings(),
                _ => Task.CompletedTask
            });
        }

        private async Task GoLeft()
        {
            if (Expanded)
            {
                await ToggleExpand();
            }
            else
            {
                await ParentItem?.SelectNode();
            }
        }

        private async Task GoRight()
        {
            if (!Expanded)
            {
                await ToggleExpand();
            }
            else
            {
                await ChildItems[0].SelectNode();
            }
        }

        private async Task GoUp(int steps)
        {
            var x = this;
            for (var i = 0; i < steps; i++)
            {
                x = GetPrevious(x);
            }
            await x.SelectNode();

            static SourceNodeTreeItemBase GetPrevious(SourceNodeTreeItemBase item)
            {
                if (item.IndexInParent == 0)
                {
                    return item.ParentItem;
                }
                else if (item.IndexInParent > 0)
                {
                    var prev = item.ParentItem.ChildItems[item.IndexInParent - 1];
                    while (prev.Expanded)
                    {
                        prev = prev.ChildItems[^1];
                    }
                    return prev;
                }
                return item;
            }
        }

        private async Task GoDown(int steps)
        {
            var x = this;
            for (var i = 0; i < steps; i++)
            {
                x = GetNext(x);
            }
            await x.SelectNode();

            static SourceNodeTreeItemBase GetNext(SourceNodeTreeItemBase item)
            {
                if (item.Expanded)
                {
                    return item.ChildItems[0];
                }
                if (item.IndexInParent + 1 < item.ParentItem?.ChildItems.Length)
                {
                    return item.ParentItem.ChildItems[item.IndexInParent + 1];
                }
                else
                {
                    var next = item;
                    while (next.IndexInParent + 1 == next.ParentItem?.ChildItems.Length)
                    {
                        next = next.ParentItem;
                    }
                    if (next.ParentItem is { })
                    {
                        return next.ParentItem.ChildItems[next.IndexInParent + 1];
                    }
                    return item;
                }
            }
        }

        private async Task GoHome()
        {
            var first = this;
            while (first.IndexInParent >= 0)
            {
                first = first.ParentItem;
            }
            await first.SelectNode();
        }

        private async Task GoEnd()
        {
            var first = this;
            while (first.IndexInParent >= 0)
            {
                first = first.ParentItem;
            }
            var last = first;
            while (last.Expanded)
            {
                last = last.ChildItems[^1];
            }
            await last.SelectNode();
        }

        private async Task ExpandSiblings()
        {
            if (ParentItem is null)
            {
                return;
            }
            foreach (var item in ParentItem.ChildItems)
            {
                if (!item.Expanded && item.HasChildren)
                {
                    await item.ToggleExpand();
                    item.StateHasChanged();
                }
            }
        }
    }
}
