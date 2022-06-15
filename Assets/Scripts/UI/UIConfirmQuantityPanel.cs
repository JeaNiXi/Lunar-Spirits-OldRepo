using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class UIConfirmQuantityPanel : MonoBehaviour
{
    [SerializeField] private Button YButton;
    [SerializeField] private Button NButton;
    [SerializeField] private Button UPButton;
    [SerializeField] private Button DOWNButton;
    [SerializeField] private TMP_InputField inputField;

    [SerializeField] private UIMainInventory uiMainInventory;

    private int inputInteger;
    private int maxRSize;

    public void ToggleConfirmQuantityPanel(bool value, int index, int maxRemoveSize)
    {
        inputInteger = 0;
        maxRSize = maxRemoveSize;
        if(value == true)
        {
            gameObject.SetActive(true);
            inputField.text = inputInteger.ToString();
            UPButton.onClick.AddListener(() => AddInt());
            DOWNButton.onClick.AddListener(() => RemoveInt());
            YButton.onClick.AddListener(() => uiMainInventory.ConfirmRemovingQuantity(index, Convert.ToInt32(inputField.text)));
            NButton.onClick.AddListener(() => uiMainInventory.ToggleConfirmQuantityPanel(false, -1, -1));
        }
        if (value == false)
        {
            UPButton.onClick.RemoveAllListeners();
            DOWNButton.onClick.RemoveAllListeners();
            YButton.onClick.RemoveAllListeners();
            NButton.onClick.RemoveAllListeners();
            gameObject.SetActive(false);
        }
    }
    private void AddInt()
    {
        if (inputInteger >= maxRSize)
        {
            inputInteger = 0;
            UpdateText();
        }
        else
        {
            inputInteger++;
            UpdateText();
        }
    }
    private void RemoveInt()
    {
        if (inputInteger <= 0)
        {
            inputInteger = maxRSize;
            UpdateText();
        }
        else
        {
            inputInteger--;
            UpdateText();
        }
    }
    private void UpdateText()
    {
        inputField.text = inputInteger.ToString();
    }
    public void CorrectEdit()
    {
        if (inputField.text == "-" || Convert.ToInt32(inputField.text) <= 0)
        {
            inputField.text = "0";
            inputInteger = 0;
        }
        if (Convert.ToInt32(inputField.text) > maxRSize)
            inputField.text = maxRSize.ToString();
    }
    public bool IsConfirmationQuantityPanelActive()
    {
        if (gameObject.activeSelf)
            return true;
        else
            return false;
    }
}
