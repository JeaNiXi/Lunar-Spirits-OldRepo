using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Inventory;
using Character;

namespace Managers
{
    public class InputController : MonoBehaviour
    {
        [SerializeField] InventoryController MainInventoryController;
        [SerializeField] CharacterManager mainCharacter;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.I))
            {
                MainInventoryController.ToggleInventory();
            }
            if (Input.GetAxis("Horizontal") != 0)
            {
                float direction = Input.GetAxis("Horizontal");
                mainCharacter.Move(direction);
            }
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                MainInventoryController.ToggleMouseClick();
            }
        }
    }
}