using Character;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Inventory.SO
{
    [CreateAssetMenu(fileName = "Base Health Modifier", menuName = "Character/Modifiers/Base/Health")]
    public class BaseHealthModifierSO : ModifiersSO
    {
        public override void ApplyModifier(CharacterManager character, int value)
        {
            character.GetActorSO().SetBaseHealth(50);
        }
    }
}