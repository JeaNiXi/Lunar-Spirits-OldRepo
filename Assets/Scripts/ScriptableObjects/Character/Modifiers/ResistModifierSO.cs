using Character;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Inventory.SO
{
    public abstract class ResistModifierSO : ScriptableObject
    {
        public enum ModifierType
        {
            FIRE_RES,
            WATER_RES,
            EARTH_RES,
            AIR_RES,
            POISON_RES,
            LIGHTNING_RES,
            PHYSICAL_RES,
            LIGHT_RES,
            DARK_RES,
        }
        public string ModifierName;
        public ModifierType modifierType;
        public abstract void ApplyModifier(CharacterManager character, int value);
    }
}