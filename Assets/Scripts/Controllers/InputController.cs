using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

using Inventory;
using Character;

namespace Managers
{
    public class InputController : MonoBehaviour
    {
        public CharacterManager MainCharacter { get; private set; }
        public void SetMainCharacter(CharacterManager character)
        {
            MainCharacter = character;
        }
        public InventoryController MainInventoryController { get; private set; }
        public void SetMainInventoryController(InventoryController inventoryController)
        {
            MainInventoryController = inventoryController;
        }

        [SerializeField]
        InputActionReference
            Movement,
            Attack,
            MousePointer,
            OpenInventory;

        private void Update()
        {
            MoveHandler();
            InventoryHandler();
            AttackHandler();
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
        #endregion
    }
}