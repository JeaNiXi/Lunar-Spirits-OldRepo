using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Inventory.SO
{
    [CreateAssetMenu(fileName = "Stat Health Modifier", menuName = "Modifiers/Stat/Health")]
    public class StatHealthModifierSO : ModifiersSO
    {
        public override void ApplyModifier(GameObject character, int value)
        {
            Debug.Log("APPLIED STAT MODIFIER!");
        }

    }
}