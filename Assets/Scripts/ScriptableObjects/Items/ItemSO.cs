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
    public enum EQUIP_SLOT_TYPE
    {
        NONE,
        HEAD,
        MEDALION,
        RING_1,
        RING_2,
        ARMOR,
        BRACERS,
        BOOTS,
        WEAPON_1,
        WEAPON_2,
        RANGE_WEAPON,
        RANGE_WEAPON_AMMO,
        QUICK_SLOT,
    }
    [field: SerializeField] public string Name { get; set; }
    //public int ID => GetInstanceID();
    [field: SerializeField] public Rarity RarityState { get; set; }
    [field: SerializeField] public EQUIP_SLOT_TYPE EquipSlot { get; set; }
    [field: SerializeField][field: TextArea] public string Description { get; set; }
    [field: SerializeField] public bool IsStackable { get; set; }
    [field: SerializeField] public int MaxStackSize { get; set; }
    [field: SerializeField] public Sprite ItemSprite { get; set; }

}
