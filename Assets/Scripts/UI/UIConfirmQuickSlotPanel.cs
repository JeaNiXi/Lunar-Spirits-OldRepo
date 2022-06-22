using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Inventory.UI
{
    public class UIConfirmQuickSlotPanel : MonoBehaviour
    {
        [SerializeField] private Button firstSlotButton;
        [SerializeField] private Button secondSlotButton;
        [SerializeField] private Button cancelButton;

        [SerializeField] private UIMainInventory uiMainInventory;

        public void ToggleConfirmQuantityPanel(bool value, int index, string containerType)
        {
            if (value == true)
            {
                gameObject.SetActive(true);
                firstSlotButton.onClick.AddListener(() => uiMainInventory.ConfirmQuickSlotEquip(index, 0, containerType));
                secondSlotButton.onClick.AddListener(() => uiMainInventory.ConfirmQuickSlotEquip(index, 1, containerType));
                cancelButton.onClick.AddListener(() => uiMainInventory.ToggleConfirmQuickSlotPanel(false, -1, ""));
            }
            else
            {
                firstSlotButton.onClick.RemoveAllListeners();
                secondSlotButton.onClick.RemoveAllListeners();
                cancelButton.onClick.RemoveAllListeners();
                gameObject.SetActive(false);
            }
        }
        public bool IsConfirmQuickSlotPanelActive()
        {
            if (gameObject.activeSelf)
                return true;
            else
                return false;
        }
    }
}