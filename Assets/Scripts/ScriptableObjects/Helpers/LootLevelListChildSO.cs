using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Inventory.SO
{
    [CreateAssetMenu(fileName = "LootLevelListChild", menuName = "Helpers/LootLevelListChild")]
    public class LootLevelListChildSO : ScriptableObject
    {
        [SerializeField] [NonReorderable] private List<PotionsSO> potions = new List<PotionsSO>();
        [SerializeField] [NonReorderable] private List<WeaponSO> weapons = new List<WeaponSO>();
        [SerializeField] [NonReorderable] private List<EquipmentSO> armor = new List<EquipmentSO>();
        private enum LootTypes
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
            MISC,
        }
        private const int LOOT_TYPE_COUNT = 12; 

        public InventoryItem GetInventoryItem()
        {
            LootTypes lootType = (LootTypes)Random.Range(0, LOOT_TYPE_COUNT+1);
            //
            int test = Random.Range(0, 2);
            if (test == 0)
                lootType = (LootTypes)2;
            else
                lootType = (LootTypes)6;
            //
            switch(lootType)
            {
                case (LootTypes)2:
                    {
                        WeaponSO weapon = GetRandomWeapon();
                        return new InventoryItem((ItemSO)weapon, 1, InventoryItem.SlotType.MAIN_SLOT);
                    }
                case (LootTypes)6:
                    {
                        EquipmentSO equipment = GetRandomArmor();
                        return new InventoryItem((ItemSO)equipment, 1, InventoryItem.SlotType.MAIN_SLOT);
                    }
                default:
                    return new InventoryItem();
                
            }
        }
        public WeaponSO GetRandomWeapon()
        {
            int count = weapons.Count;
            int index = Random.Range(0, count - 1);
            return weapons[index];
        }
        public EquipmentSO GetRandomArmor()
        {
            int count = armor.Count;
            int index = Random.Range(0, count - 1);
            return armor[index];
        }
    }
}