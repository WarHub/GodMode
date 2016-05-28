// WarHub licenses this file to you under the MIT license.
// See LICENSE file in the project root for more information.

namespace WarHub.Armoury.GodMode.AppServices.Implementations
{
    using Autofac;
    using Modules.Editor.Commands;

    public class CommandsAggregateService : ICommandsAggregateService
    {
        public CreateCatalogueItemCommand CreateCatalogueItemCommand => Resolve<CreateCatalogueItemCommand>();

        public CreateConditionItemCommand CreateConditionItemCommand => Resolve<CreateConditionItemCommand>();

        public CreateModifierCommand CreateModifierCommand => Resolve<CreateModifierCommand>();

        public OpenCatalogueCommand OpenCatalogueCommand => Resolve<OpenCatalogueCommand>();

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
