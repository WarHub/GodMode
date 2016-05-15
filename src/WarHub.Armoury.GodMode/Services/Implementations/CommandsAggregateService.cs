// WarHub licenses this file to you under the MIT license.
// See LICENSE file in the project root for more information.

namespace WarHub.Armoury.GodMode.Services.Implementations
{
    using Autofac;
    using Commands;
    using Modules.Editor.Commands;

    public class CommandsAggregateService : ICommandsAggregateService
    {
        private NavigateRelayCommand _navigateRelayCommand;

        public CreateCatalogueItemCommand CreateCatalogueItemCommand => Resolve<CreateCatalogueItemCommand>();

        public CreateConditionItemCommand CreateConditionItemCommand => Resolve<CreateConditionItemCommand>();

        public CreateModifierCommand CreateModifierCommand => Resolve<CreateModifierCommand>();

        public NavigateRelayCommand NavigateRelayCommand
            => _navigateRelayCommand ?? (_navigateRelayCommand = Resolve<NavigateRelayCommand>());

        public OpenCatalogueItemCommand OpenCatalogueItemCommand => Resolve<OpenCatalogueItemCommand>();

        public OpenConditionItemCommand OpenConditionItemCommand => Resolve<OpenConditionItemCommand>();

        public OpenLinkTargetAsChildCommand OpenLinkTargetAsChildCommand => Resolve<OpenLinkTargetAsChildCommand>();

        public OpenLinkTargetAsSharedCommand OpenLinkTargetAsSharedCommand => Resolve<OpenLinkTargetAsSharedCommand>();

        public OpenModifierCommand OpenModifierCommand => Resolve<OpenModifierCommand>();

        public RemoveCatalogueItemCommand RemoveCatalogueItemCommand => Resolve<RemoveCatalogueItemCommand>();

        public RemoveConditionItemCommand RemoveConditionItemCommand => Resolve<RemoveConditionItemCommand>();

        public RemoveModifierCommand RemoveModifierCommand => Resolve<RemoveModifierCommand>();

        private static T Resolve<T>() => App.ServiceProvider.Resolve<T>();
    }
}
