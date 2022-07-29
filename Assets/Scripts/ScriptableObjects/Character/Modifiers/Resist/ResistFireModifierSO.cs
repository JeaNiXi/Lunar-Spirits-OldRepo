using Character;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Inventory.SO
{
    [CreateAssetMenu(fileName = "Resist Fire Modifier", menuName = "Character/Modifiers/Resistance/Fire Damage")]
    public class ResistFireModifierSO : ResistModifierSO
    {
        public override void ApplyModifier(CharacterManager character, int value)
        {
            throw new System.NotImplementedException();
        }
    }
}