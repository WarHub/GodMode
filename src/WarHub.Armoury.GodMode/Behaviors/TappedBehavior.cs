// WarHub licenses this file to you under the MIT license.
// See LICENSE file in the project root for more information.

namespace WarHub.Armoury.GodMode.Behaviors
{
    using System;
    using System.Windows.Input;
    using Xamarin.Forms;

    public class TappedBehavior
    {
        public static readonly BindableProperty CommandProperty = BindableProperty.CreateAttached("Command",
            typeof(ICommand), typeof(TappedBehavior), default(ICommand), propertyChanged: CommandPropertyChanged);

        public static readonly BindableProperty CommandParameterProperty =
            BindableProperty.CreateAttached("CommandParameter", typeof(object), typeof(TappedBehavior), default(object));

        private static readonly Lazy<IGestureRecognizer> GestureLazy = new Lazy<IGestureRecognizer>(
            () =>
            {
                var gesture = new TapGestureRecognizer {NumberOfTapsRequired = 1};
                gesture.Tapped += OnTapped;
                return gesture;
            });

        private static IGestureRecognizer Gesture => GestureLazy.Value;

        public static ICommand GetCommand(BindableObject bindable)
        {
            return (ICommand) bindable.GetValue(CommandProperty);
        }

        public static void SetCommand(BindableObject bindable, ICommand value)
        {
            bindable.SetValue(CommandProperty, value);
        }

        public static object GetCommandParameter(BindableObject bindable)
        {
            return bindable.GetValue(CommandParameterProperty);
        }

        public static void SetCommandParameter(BindableObject bindable, object value)
        {
            bindable.SetValue(CommandParameterProperty, value);
        }

        private static void CommandPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var view = bindable as View;
            if (view != null && !view.GestureRecognizers.Contains(Gesture))
            {
                view.GestureRecognizers.Add(Gesture);
            }
        }

        private static void OnTapped(object sender, EventArgs e)
        {
            var bindable = sender as BindableObject;
            if (bindable == null)
            {
                return;
            }
            var command = GetCommand(bindable);
            var parameter = GetCommandParameter(bindable);
            if (command?.CanExecute(parameter) ?? false)
            {
                command.Execute(parameter);
            }
        }
    }
}
