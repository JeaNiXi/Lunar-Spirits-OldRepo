using Character;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Inventory.SO
{
    [CreateAssetMenu(fileName = "Enemy Base Health Modifier", menuName = "Character/Modifiers/Enemy/BaseHealthDamage")]
    public class EnemyHealthModifierSO : EnemyModifiersSO
    {
        public override void ApplyModifier(CharacterManager character, int value)
        {
            character.ActorParams.GetBaseHit(value);
        }
    }
}