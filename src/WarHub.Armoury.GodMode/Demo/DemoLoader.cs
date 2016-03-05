// WarHub licenses this file to you under the MIT license.
// See LICENSE file in the project root for more information.

namespace WarHub.Armoury.GodMode.Demo
{
    using System.IO;
    using System.Reflection;
    using Model;
    using Model.BattleScribe.Services;

    public static class DemoLoader
    {
        static DemoLoader()
        {
            var serializationService = new GuidControllingSerializationService();
            using (var gstStream = GetGameSystemStream())
            {
                GameSystem = serializationService.LoadGameSystem(gstStream);
            }
            using (var catStream = GetCatalogueStream())
            {
                Catalogue = serializationService.LoadCatalogue(catStream);
            }
            Catalogue.SystemContext = GameSystem.Context;
        }

        public static ICatalogue Catalogue { get; }

        public static IGameSystem GameSystem { get; }

        private static Stream GetGameSystemStream()
        {
            var assembly = typeof(DemoLoader).GetTypeInfo().Assembly;
            return assembly.GetManifestResourceStream("WarHub.Armoury.GodMode.Demo.Warhammer40K.gst");
        }

        private static Stream GetCatalogueStream()
        {
            var assembly = typeof(DemoLoader).GetTypeInfo().Assembly;
            return assembly.GetManifestResourceStream("WarHub.Armoury.GodMode.Demo.Tau-Codex.cat");
        }
    }
}
