using Character;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Inventory.SO
{
    [CreateAssetMenu(fileName = "Weapon StatDamage Modifier", menuName = "Character/Modifiers/Weapon/StatBaseDamage")]
    public class WeaponBaseDamageStatSO : WeaponStatModifiersSO
    {
        public override void ApplyModifier(ActorManager character, int value)
        {
            throw new System.NotImplementedException();
        }
    }
}