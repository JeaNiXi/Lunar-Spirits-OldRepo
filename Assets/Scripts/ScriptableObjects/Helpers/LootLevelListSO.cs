using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Inventory.SO
{
    [CreateAssetMenu(fileName = "LootLevelListMain", menuName = "Helpers/LootLevelListMain")]

    public class LootLevelListSO : ScriptableObject
    {
        [SerializeField] private LootLevelListChildSO DefaultLootList;
        [SerializeField] private LootLevelListChildSO Level_1_3_Item_List;
        [SerializeField] private LootLevelListChildSO Level_4_6_Item_List;
        public LootLevelListChildSO GetLootLevel(int playerLevel)
        {
            switch(playerLevel)
            {
                case 1:
                case 2:
                case 3:
                    return Level_1_3_Item_List;
                default:
                    return DefaultLootList;
            }
        }
    }
}