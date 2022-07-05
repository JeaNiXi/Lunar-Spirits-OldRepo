using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Inventory.SO
{
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

        public enum ItemSlots
        {
            MAIN_SLOT,
            HEAD,
            MEDALION,
            RING1,
            RING2,
            ARMOR,
            BRACERS,
            BOOTS,
            WEAPON_MAIN,
            WEAPON_SECONDARY,
            RANGED,
            AMMO,
            QUICK_SLOT,
        }

        public int ID => GetInstanceID();
        [field: SerializeField] public string Name { get; set; }
        [field: SerializeField] public int MaxStackSize { get; set; }
        [field: SerializeField] [field: TextArea] public string Description { get; set; }
        [field: SerializeField] public bool IsQuestItem { get; set; }
        [field: SerializeField] public ItemTypes ItemType { get; set; }
        [field: SerializeField] public Sprite ItemImage { get; set; }
        [field: SerializeField] public float KnockbackStrength { get; set; }
        [field: SerializeField] public float BaseDamage { get; set; }
        public List<ItemSlots> CanBeInSlots;

    }
    [Serializable]
    public class ModifierType
    {
        public ModifiersSO Modifier;
        public int Value;
    }
}