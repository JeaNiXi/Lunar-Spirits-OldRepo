using Character;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Inventory.SO
{
    public abstract class ModifiersSO : ScriptableObject
    {
        public enum ModifierType
        {
            STRENGTH,
            DEXTERITY,
            CONSTITUTION,
            INTELLIGENCE,
            WISDOM,
            CHARISMA,
        }
        public string ModifierName;
        public ModifierType modifierType;
        public abstract void ApplyModifier(CharacterManager character, int value);
    }
}