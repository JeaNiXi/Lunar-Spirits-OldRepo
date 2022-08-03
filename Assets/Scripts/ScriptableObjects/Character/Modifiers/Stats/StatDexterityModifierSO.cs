using Character;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Inventory.SO
{
    [CreateAssetMenu(fileName = "Stat Dexterity Modifier", menuName = "Character/Modifiers/Stat/Dexterity")]
    public class StatDexterityModifierSO : ModifiersSO
    {
        public override void ApplyModifier(CharacterManager character, int value)
        {
            throw new System.NotImplementedException();
        }
    }
}