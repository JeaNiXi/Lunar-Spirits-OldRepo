using Character;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Inventory.SO
{
    public abstract class ResistModifierSO : ScriptableObject
    {
        public abstract void ApplyModifier(CharacterManager character, int value);
    }
}