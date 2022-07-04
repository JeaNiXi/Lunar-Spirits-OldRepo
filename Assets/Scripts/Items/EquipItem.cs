using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Inventory.SO;

namespace Inventory
{
    public class EquipItem : MonoBehaviour
    {
        [SerializeField] public EquipmentItem EquipmentItem { get; set; }
        [SerializeField] private SpriteRenderer spriteRenderer;
        public void SetSprite(Sprite sprite)
        {
            spriteRenderer.sprite = sprite;
        }
    }
}