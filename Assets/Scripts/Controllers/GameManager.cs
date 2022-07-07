using Actor.SO;
using Character;
using Managers.SO;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
        [field: SerializeField] public GameManagerSO MainGMSO { get; private set; }
        public CharacterManager mainCharacter;

        private void Awake()
        {
            mainCharacter.OnBattlerTriggerEnter += HandleBattleStart;
        }

        private void HandleBattleStart(BattlerSO battler)
        {
            MainGMSO.SetBattlerSO(battler);
            SceneManager.LoadScene("BattleScene");
        }
    }
}