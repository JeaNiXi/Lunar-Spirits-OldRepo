using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Inventory.UI
{
    public class UILootPanel : MonoBehaviour
    {
        public void SetLootPanelActive(bool value)
        {
            gameObject.SetActive(value);
        }
    }
}