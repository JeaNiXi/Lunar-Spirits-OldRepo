using Character;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Inventory.SO
{
    [CreateAssetMenu(fileName = "Armor Value", menuName = "Character/Modifiers/Equipment/Armor")]
    public class ArmorValueSO : EquipmentModifierSO
    {
        public override void ApplyModifier(CharacterManager character, int value)
        {
            throw new System.NotImplementedException();
        }
    }
}