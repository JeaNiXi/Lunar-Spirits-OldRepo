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
        [SerializeField] private CharacterManager mainCharacter;
        [SerializeField] private InventoryController mainInventoryController;

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
                mainCharacter.MoveInput = Movement.action.ReadValue<Vector2>();
        }
        private void InventoryHandler()
        {
            if (OpenInventory.action.WasPressedThisFrame())
                mainInventoryController.ToggleInventory();
        }
        private void AttackHandler()
        {
            if (Attack.action.WasPerformedThisFrame())
                mainCharacter.StartAttack();
        }
        #endregion
    }
}