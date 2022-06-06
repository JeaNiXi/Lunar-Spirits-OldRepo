using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField] InventoryManager mainInventoryManager;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
            if (mainInventoryManager.GetInventoryState())
                mainInventoryManager.SetInventoryActive(false);
            else
                mainInventoryManager.SetInventoryActive(true);
    }
}
