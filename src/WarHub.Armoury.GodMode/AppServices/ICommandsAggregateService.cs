// WarHub licenses this file to you under the MIT license.
// See LICENSE file in the project root for more information.

namespace WarHub.Armoury.GodMode.AppServices
{
    using Commands;
    using Modules.Editor.Commands;

    public interface ICommandsAggregateService
    {
        CreateCatalogueItemCommand CreateCatalogueItemCommand { get; }

        CreateConditionItemCommand CreateConditionItemCommand { get; }

        CreateModifierCommand CreateModifierCommand { get; }

        NavigateRelayCommand NavigateRelayCommand { get; }

        OpenCatalogueCommand OpenCatalogueCommand { get; }

        OpenCatalogueItemCommand OpenCatalogueItemCommand { get; }

        OpenConditionItemCommand OpenConditionItemCommand { get; }

        OpenLinkTargetAsChildCommand OpenLinkTargetAsChildCommand { get; }

        OpenLinkTargetAsSharedCommand OpenLinkTargetAsSharedCommand { get; }

        OpenModifierCommand OpenModifierCommand { get; }

        RemoveCatalogueItemCommand RemoveCatalogueItemCommand { get; }

        RemoveConditionItemCommand RemoveConditionItemCommand { get; }

        RemoveModifierCommand RemoveModifierCommand { get; }
    }
}
