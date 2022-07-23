using Inventory.SO;
using Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mechanics
{
    public class TreasureChest : MonoBehaviour
    {
        [SerializeField] private LootLevelListSO levelListsSO;

        private const int MAX_ITEMS_COUNT = 6;
        private List<InventoryItem> exportList = new List<InventoryItem>();
        [field: SerializeField] public bool HasRandomLoot { get; set; }
        [field: SerializeField] public int ItemsCount { get; set; }
        [field: SerializeField] public bool IsOpened { get; set; }
        [field: SerializeField] public bool WasLooted { get; set; }

        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private Sprite closedSprite;
        [SerializeField] private Sprite openedSprite;

        public List<InventoryItem> GenerateRandomLoot(int playerLevel)
        {
            LootLevelListChildSO lootList = levelListsSO.GetLootLevel(playerLevel);
            int count = ItemsCount != 0 ? ItemsCount : Random.Range(1, MAX_ITEMS_COUNT + 1);
            List<InventoryItem> fullLootList = new List<InventoryItem>();

            while (count > 0)
            {
                fullLootList.Add(lootList.GetInventoryItem());
                count--;
            }
            return fullLootList;
        }


        [SerializeField] [NonReorderable] private List<InventoryItem> itemsList = new List<InventoryItem>();

        public List<InventoryItem> GetLootItems(int playerLevel)
        {
            if (exportList.Count == 0)
            {
                CreateLootItemsList(playerLevel);
                return exportList;
            }
            return exportList;
        }
        private void CreateLootItemsList(int playerLevel)
        {
            if (itemsList.Count != 0)
                exportList = itemsList;
            else
                exportList = GenerateRandomLoot(playerLevel);
        }
        public void SetOpenedState(bool value)
        {
            IsOpened = value;
            if (value)
                spriteRenderer.sprite = openedSprite;
            else
                spriteRenderer.sprite = closedSprite;
        }
    }
}