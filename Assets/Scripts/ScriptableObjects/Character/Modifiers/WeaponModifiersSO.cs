using Character;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

namespace Inventory.SO
{
    public abstract class WeaponModifiersSO : ScriptableObject
    {
        public LocalizedString ModifierLocalizedName;
        public abstract void ApplyModifier(ActorManager character, int value);
    }
}