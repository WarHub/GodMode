// WarHub licenses this file to you under the MIT license.
// See LICENSE file in the project root for more information.

namespace WarHub.Armoury.GodMode.ModelFacades
{
    using Mvvm.Commands;

    public interface IModelFacade
    {
        string Detail { get; }

        object Model { get; }

        string Name { get; }

        ICommand RemoveCommand { get; set; }
    }
}
