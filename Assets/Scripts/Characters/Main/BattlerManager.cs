using Actor.SO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Character
{
    public class BattlerManager : MonoBehaviour
    {
        [SerializeField] public BattlerSO MainBattlerSO;

        public BattlerSO GetBattlerSO() => MainBattlerSO;

        private void Awake()
        {
            if(MainBattlerSO.IsDefeated)
            {
                gameObject.SetActive(false);
            }
        }
    }
}