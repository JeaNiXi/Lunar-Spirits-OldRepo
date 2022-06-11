using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] ItemSO itemSO;
    [field: SerializeField] int Quanitity { get; set; }
    private SpriteRenderer spriteRenderer;
    [ExecuteInEditMode]
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = itemSO.ItemImage;
    }
 
    private void SetSprite()
    {

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //collision.gameObject.GetComponent<BoxCollider2D>().enabled = false;
        collision.GetComponent<MainCharacter>().GetInventorySO().AddItem(this.itemSO, this.Quanitity);
        Destroy(this.gameObject);
    }
}
