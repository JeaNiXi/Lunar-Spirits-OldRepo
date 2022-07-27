using Inventory.SO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Inventory.SO.InventoryItem;
using static Inventory.SO.ItemSO;

namespace Helpers.SO
{
    [CreateAssetMenu(fileName = "ModifiersList", menuName = "Helpers/MainModifiersList")]

    public class ModifiersListSO : ScriptableObject
    {
        public ModifiersListStatChildSO modifiersStatListSO;
        public ModifiersListWeaponChildSO modifiersWeaponListSO;
        public ModifiersListEquipmentChildSO modifiersEquipmentListSO;
        public ModifiersResistChildSO modifiersResistListSO;
        public ModifiersVulnerabilityChildSO modifiersVulnerabilityListSO;

        public List<ModifierType> GenerateStatModifiersList(int playerLevel, ItemRarities itemRarity, bool isShield)
        {
            return new List<ModifierType>();
        }
        public List<WeaponModifierType> GenerateWeaponModifiersList(int playerLevel, ItemRarities itemRarity, bool isAmmo)
        {
            List<WeaponModifierType> newList = new List<WeaponModifierType>();
            int posModifiersCount;
            switch (itemRarity)
            {
                case ItemRarities.BAD:
                case ItemRarities.COMMON:
                    posModifiersCount = 1;
                    break;
                case ItemRarities.UNCOMMON:
                case ItemRarities.RARE:
                    posModifiersCount = 2;
                    break;
                case ItemRarities.EPIC:
                case ItemRarities.LEGENDARY:
                    posModifiersCount = 3;
                    break;
                case ItemRarities.MITHYCAL:
                case ItemRarities.ETERNAL:
                    posModifiersCount = 4;
                    break;
                default:
                    posModifiersCount = 0;
                    break;
            }
            int modifierValue;
            switch (playerLevel)
            {
                case 1:
                case 2:
                case 3:
                    modifierValue = Random.Range(1, 30);
                    break;
                case 4:
                case 5:
                case 6:
                    modifierValue = Random.Range(30, 60);
                    break;
                default:
                    modifierValue = 0;
                    break;
            }
            for (int i = 0; i < posModifiersCount; i++)
            {
                int index = Random.Range(0, modifiersWeaponListSO.modifiersWeaponList.Count);
                newList.Add(new WeaponModifierType(modifiersWeaponListSO.modifiersWeaponList[index], modifierValue));
            }
            return newList;
        }
    }
}