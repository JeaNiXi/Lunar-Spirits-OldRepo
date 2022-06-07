using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIMouseFollower : MonoBehaviour
{
    [SerializeField] private Image itemSprite;
    [SerializeField] private TMP_Text quantity;

    private Canvas canvas;

    private void Awake()
    {
        //canvas = transform.root.GetComponent<Canvas>();
    }
    public void CreateMouseFollower(Sprite sprite, int quantity)
    {
        this.itemSprite.sprite = sprite;
        this.quantity.text = quantity.ToString();
    }
    public void RestoreMouseFollower()
    {
        this.itemSprite.sprite = null;
        this.quantity.text = "";
    }
    public void Update()
    {
        transform.position = Input.mousePosition;
        //RectTransformUtility.ScreenPointToLocalPointInRectangle((RectTransform)canvas.transform, Input.mousePosition, Camera.main, out Vector2 position);
        //transform.position = canvas.transform.TransformPoint(position);
    }
}
