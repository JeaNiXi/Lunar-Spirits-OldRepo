using Character;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

public abstract class WeaponStatModifiersSO : ScriptableObject
{
    public LocalizedString ModifierLocalizedName;
    public abstract void ApplyModifier(ActorManager character, int value);
}
