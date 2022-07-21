using Character;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Inventory.SO
{
    public abstract class WeaponModifiersSO : ScriptableObject
    {
        public abstract void ApplyModifier(ActorManager character, int value);
    }
}