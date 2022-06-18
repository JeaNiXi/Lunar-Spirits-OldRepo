using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIMouseFollower : MonoBehaviour
{
    [SerializeField] private Image itemImage;
    [SerializeField] private TMP_Text quantity;

    public string FollowerType { get; set; }
    public string FollowerSlotType { get; set; }
    public int ItemIndex { get; set; }
    public bool IsActive() => isActiveAndEnabled == true;
    public void ToggleMouseFollower(bool value)
    {
        gameObject.SetActive(value);
    }

    public void SetUpData(Sprite sprite, int quantity)
    {
        itemImage.sprite = sprite;
        this.quantity.text = quantity.ToString();
    }
    private void Update()
    {
        gameObject.transform.position = Input.mousePosition;
    }
}
