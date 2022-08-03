using Character;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Inventory.SO
{
    [CreateAssetMenu(fileName = "Stat Charisma Modifier", menuName = "Character/Modifiers/Stat/Charisma")]
    public class StatCharismaModifierSO : ModifiersSO
    {
        public override void ApplyModifier(CharacterManager character, int value)
        {
            throw new System.NotImplementedException();
        }
    }
}