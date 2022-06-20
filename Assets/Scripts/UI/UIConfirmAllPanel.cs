using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Inventory.UI
{
    public class UIConfirmAllPanel : MonoBehaviour
    {
        [SerializeField] private Button YButton;
        [SerializeField] private Button NButton;
        [SerializeField] private UIMainInventory uiMainInventory;
        public void ToggleConfirmationPanel(bool value, int index)
        {
            if (value == true)
            {
                gameObject.SetActive(value);
                YButton.onClick.AddListener(() => uiMainInventory.ConfirmRemovingAll(index));
                NButton.onClick.AddListener(() => uiMainInventory.ToggleConfirmationPanel(false, -1));
            }
            else
            {
                YButton.onClick.RemoveAllListeners();
                NButton.onClick.RemoveAllListeners();
                gameObject.SetActive(value);
            }
        }
        public bool IsConfirmationPanelActive()
        {
            if (gameObject.activeSelf)
                return true;
            else
                return false;
        }

    }
}