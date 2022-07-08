using Actor.SO;
using Character;
using Inventory;
using Inventory.SO;
using Inventory.UI;
using Managers.SO;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;

        [field: SerializeField] public GameManagerSO MainGMSO { get; private set; }
        [field: SerializeField] public InventoryController MainInveContPrefab { get; private set; }
        [field: SerializeField] public UIMainInventory MainUIInveContPrefab { get; private set; }
        [field: SerializeField] public InventorySO MainInventorySO { get; private set; }
        [field: SerializeField] public CharacterManager MainCharacter { get; private set; }
        [field: SerializeField] public InputController MainInputControllerPrefab { get; private set; }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
            }
            Initialize();
            MainCharacter.OnBattlerTriggerEnter += HandleBattleStart;
        }
        private void Initialize()
        {
            MainCharacter.SetMainInventorySO(MainInventorySO);
            InventoryController inventory = Instantiate(MainInveContPrefab, Vector3.zero, Quaternion.identity);
            inventory.SetUIMainInventory(MainUIInveContPrefab);
            inventory.SetMainInventorySO(MainInventorySO);
            inventory.SetMainCharacter(MainCharacter);
            InputController input = Instantiate(MainInputControllerPrefab, Vector3.zero, Quaternion.identity);
            input.SetMainCharacter(MainCharacter);
            input.SetMainInventoryController(inventory);
        }

        private void HandleBattleStart(BattlerSO battler)
        {
            MainGMSO.SetBattlerSO(battler);
            SceneManager.LoadScene("BattleScene");
        }
    }
}