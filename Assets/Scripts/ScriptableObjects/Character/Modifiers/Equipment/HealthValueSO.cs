using Character;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Inventory.SO
{
    [CreateAssetMenu(fileName = "Health Value", menuName = "Character/Modifiers/Equipment/Health")]
    public class HealthValueSO : EquipmentModifierSO
    {
        public override void ApplyModifier(CharacterManager character, int value)
        {
            character.ActorParams.HealthBonus += value;
        }
    }
}