using Character;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Actor.SO
{
    [CreateAssetMenu(fileName = "New Battler", menuName = "Base/New Battler")]

    public class BattlerSO : ScriptableObject
    {
        [field: SerializeField] public string BossName { get; set; }
        [field: SerializeField] public Battler BattlerPrefab { get; set; }
        [field: SerializeField] public bool IsDefeated { get; private set; }
        public void SetDefeatState(bool value)
        {
            IsDefeated = value;
        }
    }
}