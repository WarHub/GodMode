// WarHub licenses this file to you under the MIT license.
// See LICENSE file in the project root for more information.

namespace WarHub.Armoury.GodMode.Behaviors
{
    using System.Threading.Tasks;
    using System.Windows.Input;
    using Xamarin.Forms;

    public class ItemSelectedBehavior
    {
        public static readonly BindableProperty CommandProperty = BindableProperty.CreateAttached("Command",
            typeof(ICommand), typeof(ItemSelectedBehavior), default(ICommand), propertyChanged: CommandPropertyChanged);


        public static ICommand GetCommand(BindableObject bindable)
        {
            return (ICommand) bindable.GetValue(CommandProperty);
        }

        public static void SetCommand(BindableObject bindable, ICommand value)
        {
            bindable.SetValue(CommandProperty, value);
        }

        private static void CommandPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var listView = bindable as ListView;
            if (listView == null)
            {
                return;
            }
            if (oldValue != null)
            {
                listView.ItemSelected -= ListViewOnItemSelected;
            }
            if (newValue != null)
            {
                listView.ItemSelected += ListViewOnItemSelected;
            }
        }

        private static void ListViewOnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var listView = sender as ListView;
            if (listView == null)
            {
                return;
            }
            listView.SelectedItem = null;
            var command = GetCommand(listView);
            if (command?.CanExecute(e.SelectedItem) ?? false)
            {
                command.Execute(e.SelectedItem);
            }
        }
    }
}
