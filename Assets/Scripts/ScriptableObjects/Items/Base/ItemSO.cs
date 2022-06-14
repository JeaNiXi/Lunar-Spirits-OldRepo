using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ItemSO : ScriptableObject
{
    public enum ItemTypes
    {
        DEFAULT,
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
        BRACES,
        BOOTS,
        QUEST_ITEM,
        MISC,
    }
    public int ID => GetInstanceID();
    [field: SerializeField] public string Name { get; set; }
    [field: SerializeField] public int MaxStackSize { get; set; }
    [field: SerializeField] [field: TextArea] public string Description { get; set; }
    [field: SerializeField] public ItemTypes ItemType { get; set; }
    [field: SerializeField] public Sprite ItemImage { get; set; }
}
[Serializable]
public class ModifierType
{
    public ModifiersSO Modifier;
    public int Value;
}