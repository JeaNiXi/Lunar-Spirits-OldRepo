using Character;
using Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Inventory.SO
{
    [CreateAssetMenu(fileName = "New Weapon", menuName = "Inventory/Items/Weapon Item")]
    public class WeaponSO : ItemSO, IRemovable, IEquipable
    {
        public enum ScaleType
        {
            Strength,
            Dexterity,
            Intelligence,
        }
        [SerializeField] public ScaleType scaleType;
        [SerializeField] public float KnockbackStrength;

        public void EquipItem(CharacterManager character, InventorySO mainInventory, int index, string containerType)
        {
            if (mainInventory.EquipItemHandler(index, containerType)) 
            {
                ApplyModifier(character);
            }
        }

        public void EquipItem(CharacterManager character)
        {
            ApplyModifier(character);
        }
        private void ApplyModifier(CharacterManager character)
        {
            foreach (ModifierType modifier in itemParameters.statModifiers)
            {
                modifier.Modifier.ApplyModifier(character, modifier.Value);
            }
        }
    }
}