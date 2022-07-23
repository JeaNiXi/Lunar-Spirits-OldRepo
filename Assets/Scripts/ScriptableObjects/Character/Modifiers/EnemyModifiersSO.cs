using Character;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Inventory.SO
{
    public abstract class EnemyModifiersSO : ScriptableObject
    {
        public abstract void ApplyModifier(CharacterManager character, int value);
    }

    [Serializable]
    public class EnemyModifierType
    {
        public EnemyModifiersSO Modifier;
        public int Value;
    }
}