using Character;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Inventory.SO
{
    public abstract class EquipmentModifierSO : ScriptableObject
    {
        public abstract void ApplyModifier(CharacterManager character, int value);
    }
}