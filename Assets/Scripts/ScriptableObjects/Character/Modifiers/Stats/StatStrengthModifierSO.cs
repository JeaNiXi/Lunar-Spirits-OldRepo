using Character;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Inventory.SO
{
    [CreateAssetMenu(fileName = "Stat Strength Modifier", menuName = "Character/Modifiers/Stat/Strength")]
    public class StatStrengthModifierSO : ModifiersSO
    {
        public override void ApplyModifier(CharacterManager character, int value)
        {
            throw new System.NotImplementedException();
        }
    }
}