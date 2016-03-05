// WarHub licenses this file to you under the MIT license.
// See LICENSE file in the project root for more information.

namespace WarHub.Armoury.GodMode.ViewModels
{
    using Demo;
    using Model;

    public class BookIndexViewModel : GenericViewModel<BookIndexViewModel, IBookIndex>
    {
        public BookIndexViewModel(IBookIndex model = null) : base(model ?? ModelLocator.Book)
        {
        }

        public string Page
        {
            get { return Book.Page; }
            set { Set(() => Book.Page == value, () => Book.Page = value); }
        }

        public string Title
        {
            get { return Book.Title; }
            set { Set(() => Book.Title == value, () => Book.Title = value); }
        }

        private IBookIndex Book => Model;

        protected override BookIndexViewModel WithModelCore(IBookIndex model)
        {
            return new BookIndexViewModel(model);
        }
    }
}
