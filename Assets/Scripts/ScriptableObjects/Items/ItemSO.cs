using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Item", menuName = "Items/Basic Item")]
public class ItemSO : ScriptableObject
{
    public enum Rarity
    {
        COMMON,
        RARE
    }
    [field: SerializeField] public string Name { get; set; }
    [field: SerializeField] public Rarity RarityState { get; set; }
    [field: SerializeField][field: TextArea] public string Description { get; set; }
    [field: SerializeField] public bool IsStackable { get; set; }
    [field: SerializeField] public int MaxStackSize { get; set; }
    [field: SerializeField] public Sprite ItemSprite { get; set; }
}
