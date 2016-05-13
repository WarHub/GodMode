// WarHub licenses this file to you under the MIT license.
// See LICENSE file in the project root for more information.

namespace WarHub.Armoury.GodMode.Bindables
{
    using System;
    using System.Linq;
    using Model;

    public static class ModelStringifyExtensions
    {
        public static string Stringify(this ICatalogueCondition cond)
        {
            if (cond == null)
                throw new ArgumentNullException(nameof(cond));
            var parentIsRef = cond.ParentKind == ConditionParentKind.Reference;
            var parent = cond.GetConditionParentRef();
            var parentName = !parentIsRef ? string.Empty : (parent == null ? "(ref not found)" : parent.Name);
            var childIsRef = cond.ChildKind == ConditionChildKind.Reference;
            var child = cond.GetConditionChildRef();
            var childName = !childIsRef ? string.Empty : (child == null ? "(ref not found)" : child.Name);
            return cond.ConditionKind == ConditionKind.InstanceOf
                ? $"{cond.ParentKind} is {cond.ConditionKind} {childName}"
                : $"{(parentIsRef ? parentName : cond.ParentKind.ToString())}" +
                  $" has {cond.ConditionKind} {cond.ChildValue} {cond.ChildValueUnit}" +
                  $"{(childIsRef ? " of " + childName : string.Empty)}";
        }

        public static IProfile GetParentProfile(this IProfileModifier modifier)
        {
            return modifier.Context.Profiles.FirstOrDefault(profile => profile.Modifiers.Contains(modifier)) ??
                   modifier.Context.ProfileLinks.First(link => link.Modifiers.Contains(modifier)).Target;
        }

        public static string ToDetailString(this ICatalogueConditionNodeContainer container)
        {
            var conditionCount = container.Conditions.Count;
            var groupsCount = container.ConditionGroups.Count;
            return $"{conditionCount} condition{(conditionCount != 1 ? "s" : "")}," +
                   $" {groupsCount} cond. group{(groupsCount != 1 ? "s" : "")}";
        }

        public static string Stringify(this IRuleModifier m)
        {
            switch (m.Action)
            {
                case RuleModifierAction.Hide:
                case RuleModifierAction.Show:
                    return m.Action.ToString();
                case RuleModifierAction.Set:
                    return $"{m.Action} '{m.Field}' to '{m.Value}'";
                case RuleModifierAction.Append:
                    return $"{m.Action} '{m.Field}' with '{m.Value}'";
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public static string Stringify(this IEntryModifier m)
        {
            switch (m.Action)
            {
                case EntryBaseModifierAction.Hide:
                case EntryBaseModifierAction.Show:
                    return m.Action.ToString();
                case EntryBaseModifierAction.Increment:
                case EntryBaseModifierAction.Decrement:
                    return $"{m.Action} '{m.Field}' by '{m.Value}'";
                case EntryBaseModifierAction.Set:
                    return $"{m.Action} '{m.Field}' to '{m.Value}'";
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public static string Stringify(this IGroupModifier m)
        {
            switch (m.Action)
            {
                case EntryBaseModifierAction.Hide:
                case EntryBaseModifierAction.Show:
                    return m.Action.ToString();
                case EntryBaseModifierAction.Increment:
                case EntryBaseModifierAction.Decrement:
                    return $"{m.Action} '{m.Field}' by '{m.Value}'";
                case EntryBaseModifierAction.Set:
                    return $"{m.Action} '{m.Field}' to '{m.Value}'";
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public static string Stringify(this IProfileModifier m)
        {
            switch (m.Action)
            {
                case ProfileModifierAction.Hide:
                case ProfileModifierAction.Show:
                    return m.Action.ToString();
                case ProfileModifierAction.Increment:
                case ProfileModifierAction.Decrement:
                    return $"{m.Action} '{m.GetTargetCharacteristic()?.Name ?? m.Field.RawValue}' by '{m.Value}'";
                case ProfileModifierAction.Append:
                    return $"{m.Action} '{m.GetTargetCharacteristic()?.Name ?? m.Field.RawValue}' with '{m.Value}'";
                case ProfileModifierAction.Set:
                    return $"{m.Action} '{m.GetTargetCharacteristic()?.Name ?? m.Field.RawValue}' to '{m.Value}'";
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private static ICharacteristic GetTargetCharacteristic(this IProfileModifier m)
        {
            return
                m.GetParentProfile()
                    .Characteristics.FirstOrDefault(characteristic => characteristic.TypeId.Value == m.Field.Value);
        }
    }
}
