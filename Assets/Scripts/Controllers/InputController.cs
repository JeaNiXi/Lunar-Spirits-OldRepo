using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

using Inventory;
using Character;
using System;

namespace Managers
{
    public class InputController : MonoBehaviour
    {
        public static InputController Instance;
        [field: SerializeField] public CharacterManager MainCharacter { get; private set; }
        [field: SerializeField] public InventoryController MainInventoryController { get; private set; }


        [SerializeField]
        InputActionReference
            Movement,
            Attack,
            MousePointer,
            OpenInventory,
            Use;

        private Vector2 oldMousePosition;

        private void Awake()
        {
            if (Instance != null && Instance != this)
                Destroy(this);
            else
            {
                Instance = this;
                DontDestroyOnLoad(this);
            }
        }
        private void Update()
        {
            if (GameManager.Instance.GameState == GameManager.GameStates.PLAYING)
            {
                MoveHandler();
                InventoryHandler();
                AttackHandler();
                UseHandler();
            }
        }




        #region UpdateHandlers
        private void MoveHandler()
        {
            if(!Attack.action.WasPressedThisFrame())
                MainCharacter.MoveInput = Movement.action.ReadValue<Vector2>();
        }
        private void InventoryHandler()
        {
            if (OpenInventory.action.WasPressedThisFrame())
                MainInventoryController.ToggleInventory();
        }
        private void AttackHandler()
        {
            if (Attack.action.WasPerformedThisFrame())
                MainCharacter.StartAttack();
        }
        private void UseHandler()
        {
            if (Use.action.WasPressedThisFrame() && MainCharacter.IsAtSavePlace)
                GameManager.Instance.ToggleSavePlacePanel(true);
            if (Use.action.WasPressedThisFrame() && MainCharacter.IsAtChest)
                CharacterManager.Instance.UseTreasureChest(CharacterManager.Instance.currentChestCollider);
        }
        #endregion
    }
}