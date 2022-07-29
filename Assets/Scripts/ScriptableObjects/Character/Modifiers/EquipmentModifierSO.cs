using Character;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        public ModifierType modifierType;
        public abstract void ApplyModifier(CharacterManager character, int value);
    }
}