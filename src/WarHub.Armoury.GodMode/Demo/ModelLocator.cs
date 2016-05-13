// WarHub licenses this file to you under the MIT license.
// See LICENSE file in the project root for more information.

namespace WarHub.Armoury.GodMode.Demo
{
    using System.Linq;
    using Model;

    public static class ModelLocator
    {
        public static IBookIndex Book => Profile.Book;

        public static ICatalogue Catalogue => DemoLoader.Catalogue;

        public static ICatalogueCondition CatalogueCondition => RuleModifier.Conditions.First();

        public static ICatalogueConditionGroup CatalogueConditionGroup => RuleModifier.ConditionGroups.First();

        public static ICharacteristic Characteristic => Profile.Characteristics.First();

        public static IEntry Entry => Catalogue.SharedEntries.First();

        public static IEntryLimits EntryLimits => Entry.Limits;

        public static IEntryLink EntryLink => Catalogue.EntryLinks.First();

        public static IEntryModifier EntryModifier
            => Catalogue.Context.Entries.First(entry => entry.Modifiers.Count > 0).Modifiers.First();

        public static IGroup Group => Catalogue.Context.Groups.First();

        public static IGroupLink GroupLink => Catalogue.Context.GroupLinks.First();

        public static IGroupModifier GroupModifier
            => Catalogue.Context.Groups.First(group => group.Modifiers.Count > 0).Modifiers.First();

        public static IIdentifier Id => Catalogue.Id;

        public static IProfile Profile => Catalogue.SharedProfiles.First();

        public static IProfileLink ProfileLink => Catalogue.Context.ProfileLinks.First();

        public static IProfileModifier ProfileModifier
            => Catalogue.Context.Profiles.First(profile => profile.Modifiers.Count > 0).Modifiers.First();

        public static IRootEntry RootEntry => Catalogue.Entries.First();

        public static IRootLink RootEntryLink => Catalogue.EntryLinks.First();

        public static IRule Rule => Catalogue.Rules.First();

        public static IRuleLink RuleLink => Catalogue.RuleLinks.First();

        public static IRuleModifier RuleModifier => Rule.Modifiers.First();
    }
}
