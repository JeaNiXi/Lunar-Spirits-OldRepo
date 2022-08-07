using Character;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

namespace Inventory.SO
{
    public abstract class EquipmentModifierSO : ScriptableObject
    {
        public enum ModifierType
        {
            HEALTH,
            ARMOR,
            MR,
        }
        public LocalizedString ModifierLocalizedName;
        public ModifierType modifierType;
        public abstract void ApplyModifier(CharacterManager character, int value);
    }
}