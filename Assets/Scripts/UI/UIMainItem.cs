using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIMainItem : MonoBehaviour
{
    [SerializeField] private Component imageComponent;
    [SerializeField] private Image itemImage;
    [SerializeField] private TMP_Text quantityText;
    //[SerializeField] private Image DefaultImage;

    private bool IsEmpty { get; set; }

    public void SetData()
    {
        this.itemImage.sprite = null;
        this.quantityText.text = "";
        this.IsEmpty = true;
        imageComponent.gameObject.SetActive(false);
    }
    public void SetData(Sprite sprite, int quantity)
    {
        imageComponent.gameObject.SetActive(true);
        this.itemImage.sprite = sprite;
        this.quantityText.text = quantity.ToString();
        this.IsEmpty = false;
    }
    public UIMainItem(Sprite sprite, int quantity)
    {
        imageComponent.gameObject.SetActive(true);
        this.itemImage.sprite = sprite;
        this.quantityText.text = quantity.ToString();
        this.IsEmpty = false;
    }
    public void DeleteObject()
    {
        Destroy(gameObject);
    }
}