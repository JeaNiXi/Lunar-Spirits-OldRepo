using Character;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Inventory.SO
{
    [CreateAssetMenu(fileName = "Stat Constitution Modifier", menuName = "Character/Modifiers/Stat/Constitution")]
    public class StatConstitutionModifierSO : ModifiersSO
    {
        public override void ApplyModifier(CharacterManager character, int value)
        {
            throw new System.NotImplementedException();
        }
    }
}