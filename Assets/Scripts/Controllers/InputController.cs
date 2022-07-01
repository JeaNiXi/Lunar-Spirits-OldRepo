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
        }




        #region UpdateHandlers
        private void MoveHandler()
        {
            mainCharacter.MoveInput = Movement.action.ReadValue<Vector2>();
        }
        private void InventoryHandler()
        {
            if (OpenInventory.action.WasPressedThisFrame())
                mainInventoryController.ToggleInventory();
        }
        #endregion
    }
}