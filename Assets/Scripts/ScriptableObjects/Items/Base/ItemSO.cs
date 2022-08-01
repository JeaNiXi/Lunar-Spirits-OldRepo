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

        public List<ItemSlots> CanBeInSlots;
        [field: SerializeField] public ItemParameters ItemParameters { get; set; }
    }
    [Serializable]
    public struct ModifierType
    {
        public ModifiersSO Modifier;
        public int Value;
        public ModifierType(ModifiersSO modifiersSO, int value)
        {
            this.Modifier = modifiersSO;
            this.Value = value;
        }
    }
    [Serializable] 
    public struct WeaponModifierType
    {
        public WeaponModifiersSO Modifier;
        public int Value;

        public WeaponModifierType(WeaponModifiersSO weaponModifiersSO, int value)
        {
            this.Modifier = weaponModifiersSO;
            this.Value = value;
        }
    }
    [Serializable]
    public struct WeaponStatModifierType
    {
        public WeaponStatModifiersSO Modifier;
        public int Value;
        public WeaponStatModifierType(WeaponStatModifiersSO weaponStatModifiersSO, int value)
        {
            this.Modifier = weaponStatModifiersSO;
            this.Value = value;
        }
    }
    [Serializable]
    public struct EquipmentModifierType
    {
        public EquipmentModifierSO Modifier;
        public int Value;
        public EquipmentModifierType(EquipmentModifierSO equipmentModifierSO, int value)
        {
            this.Modifier = equipmentModifierSO;
            this.Value = value;
        }
    }
    [Serializable]
    public struct ResistModifierType
    {
        public ResistModifierSO Modifier;
        public int Value;
        public ResistModifierType(ResistModifierSO resistModifierSO, int value)
        {
            this.Modifier = resistModifierSO;
            this.Value = value;
        }
    }
    [Serializable]
    public struct VulnerabilityModifierType
    {
        public VulnerabilityModifierSO Modifier;
        public int Value;
        public VulnerabilityModifierType(VulnerabilityModifierSO vulnerabilityModifierSO, int value)
        {
            this.Modifier = vulnerabilityModifierSO;
            this.Value = value;
        }
    }
}