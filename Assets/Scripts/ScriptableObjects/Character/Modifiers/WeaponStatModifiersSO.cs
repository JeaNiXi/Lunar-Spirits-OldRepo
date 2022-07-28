using Character;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponStatModifiersSO : ScriptableObject
{
    public abstract void ApplyModifier(ActorManager character, int value);
}
