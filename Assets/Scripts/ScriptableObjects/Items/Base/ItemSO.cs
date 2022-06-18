using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ItemSO : ScriptableObject
{
    public enum ItemTypes
    {
        POTION,
        INGREDIENT,
        WEAPON,
        SHIELD,
        RANGED_WEAPON,
        RANGED_AMMO,
        ARMOR,
        MEDALION,
        RING,
        HEAD_GEAR,
        BRACERS,
        BOOTS,
        QUEST_ITEM,
        MISC,
    }

    public enum EquipSlots
    {
        _0_DEFAULT,
        _1_HEAD,
        _2_MEDALION,
        _3_RING1,
        _4_RING2,
        _5_ARMOR,
        _6_BRACERS,
        _7_BOOTS,
        _8_WEAPON_MAIN,
        _9_WEAPON_SECONDARY,
        _10_RANGED,
        _11_AMMO,
    }

    public int ID => GetInstanceID();
    [field: SerializeField] public string Name { get; set; }
    [field: SerializeField] public int MaxStackSize { get; set; }
    [field: SerializeField] [field: TextArea] public string Description { get; set; }
    [field: SerializeField] public ItemTypes ItemType { get; set; }
    [field: SerializeField] public Sprite ItemImage { get; set; }

    public List<EquipSlots> SlotsToEquipIn;

}
[Serializable]
public class ModifierType
{
    public ModifiersSO Modifier;
    public int Value;
}