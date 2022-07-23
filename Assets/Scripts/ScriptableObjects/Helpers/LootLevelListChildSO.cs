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
            lootType = (LootTypes)2;
            //
            switch(lootType)
            {
                case (LootTypes)2:
                    {
                        WeaponSO weapon = GetRandomWeapon();
                        return new InventoryItem((ItemSO)weapon, 1, InventoryItem.SlotType.MAIN_SLOT);
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
    }
}