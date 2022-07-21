using Character;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Inventory.SO
{
    [CreateAssetMenu(fileName = "Stat Health Modifier", menuName = "Character/Modifiers/Stat/Health")]
    public class StatHealthModifierSO : ModifiersSO
    {
        public override void ApplyModifier(CharacterManager character, int value)
        {
            character.ActorParams.AddHealthBonus(value);
        }
    }
}