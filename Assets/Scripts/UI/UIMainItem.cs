using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIMainItem : MonoBehaviour
{
    [SerializeField] private Image itemImage;
    [SerializeField] private TMP_Text quantityText;

    public void SetData()
    {
        this.itemImage = null;
        this.quantityText.text = "";
    }
    public void SetData(Sprite sprite, int quantity)
    {
        this.itemImage.sprite = sprite;
        this.quantityText.text = quantity.ToString();
    }
}