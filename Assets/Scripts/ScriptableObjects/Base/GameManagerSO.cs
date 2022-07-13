using Actor.SO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Managers.SO
{
    [CreateAssetMenu(fileName = "New Game Manager", menuName = "Managers/Game Manager")]

    public class GameManagerSO : ScriptableObject
    {
        [field: SerializeField] public BattlerSO MainBattlerSO { get; private set; }
        [field: SerializeField] public int LastSceneIndex { get; private set; }
        [field: SerializeField] public bool IsInitialized { get; private set; }
        public void SetBattlerSO(BattlerSO battler)
        {
            this.MainBattlerSO = battler;
        }
        public void SetLastSceneIndex(int index)
        {
            this.LastSceneIndex = index;
;       }
        public void SetDefeatedBattler(bool value)
        {
            this.MainBattlerSO.SetDefeatState(value);
        }
        public void SetSceneInitializedState(bool value)
        {
            IsInitialized = value;
        }
    }
}