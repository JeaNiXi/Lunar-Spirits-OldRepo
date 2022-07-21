using Character;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Inventory.SO
{
    [CreateAssetMenu(fileName = "Weapon BaseDamage Modifier", menuName = "Character/Modifiers/Weapon/BaseDamage")]

    public class WeaponDamageBaseSO : WeaponModifiersSO
    {
        public override void ApplyModifier(ActorManager character, int value)
        {
            character.ActorParams.GetBaseHit(value);
        }
    }
}