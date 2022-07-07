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
        public void SetBattlerSO(BattlerSO battler)
        {
            this.MainBattlerSO = battler;
        }
    }
}