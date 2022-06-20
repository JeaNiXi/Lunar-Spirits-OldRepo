using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Inventory.SO
{
    [CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory/Base/Inventory")]
    public class InventorySO : ScriptableObject
    {
        public event Action
            OnInventoryUpdated,
            OnQuickSlotUpdated,
            OnEquipmentUpdated;

        [SerializeField] [NonReorderable] private List<InventoryItem> Container = new List<InventoryItem>(24);
        [SerializeField] [NonReorderable] private List<QuickSlotItem> QSContainer = new List<QuickSlotItem>(2);
        [SerializeField] [NonReorderable] private List<EquipmentItem> EquipContainer = new List<EquipmentItem>(11);


        private int Rows { get => Container.Count / 6; }
        private const int MAX_ITEM_SLOTS = 42;
        private const int MAX_QUICK_SLOTS = 2;
        private const int MAX_EQUIPMENT_SLOTS = 11;



        public int GetInventorySize() => Container.Count;
        public List<InventoryItem> GetItemList() => Container;
        public List<QuickSlotItem> GetQuickSlotList() => QSContainer;
        public List<EquipmentItem> GetEquipmentItemsList() => EquipContainer;

        public void debug()
        {


        }
        public int AddItem(ItemSO item, int quantity)
        {
            if (item.MaxStackSize > 1)
            {
                int index;
                int sizeToAdd = quantity;
                while (sizeToAdd > 0)
                {
                    if (SearchForItemStackable(item, out index))
                    {
                        if (Container[index].quantity + quantity > Container[index].item.MaxStackSize)
                        {
                            int maxStackSize = Container[index].item.MaxStackSize;
                            sizeToAdd = quantity - (Container[index].item.MaxStackSize - Container[index].quantity);
                            Container[index] = new InventoryItem(item, maxStackSize);
                        }
                        else
                        {
                            int addSize = Container[index].quantity + sizeToAdd;
                            Container[index] = new InventoryItem(item, addSize);
                            InformUI();
                            return 0;
                            //sizeToAdd = 0;
                        }
                    }
                    else
                    {
                        if (SearchForEmptySlot(out index))
                        {
                            if (sizeToAdd > item.MaxStackSize)
                            {
                                Container[index] = new InventoryItem(item, item.MaxStackSize);
                                sizeToAdd -= item.MaxStackSize;
                            }
                            else
                            {
                                Container[index] = new InventoryItem(item, sizeToAdd);
                                InformUI();
                                return 0;
                                //sizeToAdd = 0;
                            }
                        }
                        else
                        {
                            //Should be remade. No Empty Slots, nowhere to stack. Debug Inventory Full!
                            Debug.Log("STACKABLE Inventory Full!");
                            InformUI();
                            return sizeToAdd;
                            //sizeToAdd = 0;
                            //if (sizeToAdd > item.MaxStackSize)
                            //{
                            //    Container.Add(new InventoryItem(item, item.MaxStackSize));
                            //    sizeToAdd -= item.MaxStackSize;
                            //}
                            //else
                            //{
                            //    Container.Add(new InventoryItem(item, sizeToAdd));
                            //    sizeToAdd = 0;
                            //}
                        }
                    }
                }
            }
            else
            {
                int index;
                int sizeToAdd = quantity;
                while (sizeToAdd > 0)
                {
                    if (SearchForEmptySlot(out index))
                    {
                        Container[index] = new InventoryItem(item, item.MaxStackSize);
                        sizeToAdd -= item.MaxStackSize;
                    }
                    else
                    {
                        //Should be remade. No Empty Slots, nowhere add. Debug Inventory Full!
                        Debug.Log("NO STACKABLE FULL");
                        InformUI();
                        return sizeToAdd;
                        //sizeToAdd = 0;
                        //Container.Add(new InventoryItem(item, item.MaxStackSize));
                        //sizeToAdd -= item.MaxStackSize;
                    }
                }
            }
            InformUI();
            return 0;
        }

        public void RemoveItem(ItemSO item, int quantity, int index)
        {

        }
        public void RemoveItem(ItemSO item, int quantity)
        {

        }
        public void RemoveItem(ItemSO item)
        {

        }
        public void RemoveItem(int index, int quantity)
        {
            if (Container[index].IsEmpty)
                return;
            if (Container[index].quantity > 1)
            {
                Container[index] = new InventoryItem(Container[index].item, Container[index].quantity - quantity);
            }
            else
                Container[index] = InventoryItem.GetEmptyItem();
            InformUI();
        }
        public void RemoveItem(int index)
        {
            if (Container[index].IsEmpty)
                return;
            Container[index] = InventoryItem.GetEmptyItem();
            InformUI();
        }
        public void RemoveItem(string itemType, string itemName)
        {

        }
        public void RemoveItem(string itemType)
        {
            if (SearchForItem(itemType))
            {
                for (int i = 0; i < Container.Count; i++)
                {
                    if (Container[i].IsEmpty)
                        continue;
                    if (Container[i].item.ItemType.ToString() == itemType)
                    {
                        Container[i] = InventoryItem.GetEmptyItem();
                    }
                }
                InformUI();
            }
            else
            {
                Debug.LogError("NO ITEM FOUND");
            }
        }
        public void RemoveQuickSlotItem(int index, int quantity)
        {
            if (QSContainer[index].IsEmpty)
                return;
            if (QSContainer[index].quantity > 1)
            {
                QSContainer[index] = new QuickSlotItem(QSContainer[index].item, QSContainer[index].quantity - quantity);
            }
            else
                QSContainer[index] = QuickSlotItem.GetEmptyQuickSlotItem();
            InformQuickSlotUI();
        }

        public void SwapItems(int startIndex, int endIndex)
        {
            //if (GetQuickSlotItemAt(endIndex).item.MaxStackSize > 1) 

        }
        public void MoveInventoryItemToEmptyInventorySlot(int followerIndex, int emptyIndex)
        {
            Container[emptyIndex] = new InventoryItem(Container[followerIndex].item, Container[followerIndex].quantity);
            Container[followerIndex] = InventoryItem.GetEmptyItem();
            InformUI();
        }
        public void MoveInventoryItemToEmptyEquipmentSlot(int followerIndex, int equipIndex)
        {
            EquipContainer[equipIndex] = new EquipmentItem(Container[followerIndex].item, Container[followerIndex].quantity, EquipContainer[equipIndex].slotType);
            Container[followerIndex] = InventoryItem.GetEmptyItem();
            InformUI();
            InformEquipmentUI();
        }


        public void EquipToQuickSlot(int index, int quickSlotIndex)
        {
            if (Container[index].IsEmpty)
                return;
            if (QSContainer[quickSlotIndex].IsEmpty)
            {
                QSContainer[quickSlotIndex] = new QuickSlotItem(Container[index].item, Container[index].quantity);
                Container[index] = InventoryItem.GetEmptyItem();
            }
            else
            {
                if (QSContainer[quickSlotIndex].item.ID == Container[index].item.ID)
                {
                    if (QSContainer[quickSlotIndex].quantity < QSContainer[quickSlotIndex].item.MaxStackSize)
                    {
                        int possibleSizeToAdd = QSContainer[quickSlotIndex].item.MaxStackSize - QSContainer[quickSlotIndex].quantity;
                        if (possibleSizeToAdd >= Container[index].quantity)
                        {
                            int newSize = QSContainer[quickSlotIndex].quantity + Container[index].quantity;
                            QSContainer[quickSlotIndex] = new QuickSlotItem(Container[index].item, newSize);
                            Container[index] = InventoryItem.GetEmptyItem();
                        }
                        else
                        {
                            int reminder = Container[index].quantity - possibleSizeToAdd;
                            QSContainer[quickSlotIndex] = new QuickSlotItem(Container[index].item, Container[index].item.MaxStackSize);
                            Container[index] = new InventoryItem(QSContainer[quickSlotIndex].item, reminder);
                        }
                    }
                    else
                    {
                        QuickSlotItem tmpQSItem = new QuickSlotItem(QSContainer[quickSlotIndex].item, QSContainer[quickSlotIndex].quantity);
                        QSContainer[quickSlotIndex] = new QuickSlotItem(Container[index].item, Container[index].quantity);
                        Container[index] = new InventoryItem(tmpQSItem.item, tmpQSItem.quantity);
                    }
                }
                else
                {
                    QuickSlotItem tmpQSItem = new QuickSlotItem(QSContainer[quickSlotIndex].item, QSContainer[quickSlotIndex].quantity);
                    QSContainer[quickSlotIndex] = new QuickSlotItem(Container[index].item, Container[index].quantity);
                    Container[index] = new InventoryItem(tmpQSItem.item, tmpQSItem.quantity);
                }
            }
            InformUI();
            InformQuickSlotUI();
        }
        public void UnequipQuitSlot(int index)
        {
            if (QSContainer[index].IsEmpty)
                return;
            if (QSContainer[index].item.MaxStackSize > 1)
            {
                int newIndex;
                int sizeToAdd;
                int reminder = QSContainer[index].quantity;
                while (reminder > 0)
                {
                    if (SearchForItemStackable(QSContainer[index].item, out newIndex))
                    {
                        if (Container[newIndex].quantity + reminder > Container[newIndex].item.MaxStackSize)
                        {
                            sizeToAdd = Container[newIndex].item.MaxStackSize - Container[newIndex].quantity;
                            Container[newIndex] = new InventoryItem(QSContainer[index].item, QSContainer[index].item.MaxStackSize);
                            reminder -= sizeToAdd;
                        }
                        else
                        {
                            sizeToAdd = Container[newIndex].quantity + reminder;
                            Container[newIndex] = new InventoryItem(QSContainer[index].item, sizeToAdd);
                            QSContainer[index] = QuickSlotItem.GetEmptyQuickSlotItem();
                            InformUI();
                            InformQuickSlotUI();
                            return;
                        }
                    }
                    else
                    {
                        if (SearchForEmptySlot(out newIndex))
                        {
                            if (reminder > QSContainer[index].item.MaxStackSize)
                            {
                                sizeToAdd = reminder - QSContainer[index].item.MaxStackSize;
                                Container[newIndex] = new InventoryItem(QSContainer[index].item, QSContainer[index].item.MaxStackSize);
                                reminder -= sizeToAdd;
                            }
                            else
                            {
                                Container[newIndex] = new InventoryItem(QSContainer[index].item, reminder);
                                QSContainer[index] = QuickSlotItem.GetEmptyQuickSlotItem();
                                InformUI();
                                InformQuickSlotUI();
                                return;
                            }
                        }
                        else
                        {
                            QSContainer[index] = new QuickSlotItem(QSContainer[index].item, reminder);
                            Debug.Log("NO EMPTY SLOTS TO UNEQUIP QUICK SLOT STACKABLE ITEM, ITEMS LEFT: " + reminder);
                            InformUI();
                            InformQuickSlotUI();
                            return;
                        }
                    }
                }
            }
            else
            {
                int newIndex;
                int sizeToAdd = QSContainer[index].quantity;
                while (sizeToAdd > 0)
                {
                    if (SearchForEmptySlot(out newIndex))
                    {
                        Container[newIndex] = new InventoryItem(QSContainer[index].item, QSContainer[index].item.MaxStackSize);
                        sizeToAdd -= QSContainer[index].item.MaxStackSize;
                    }
                    else
                    {
                        QSContainer[index] = new QuickSlotItem(QSContainer[index].item, sizeToAdd);
                        Debug.Log("NO EMPTY SLOTS TO UNEQUIP QUICK SLOT UNSTACKABLE ITEM, ITEMS LEFT: " + sizeToAdd);
                        InformUI();
                        InformQuickSlotUI();
                        return;
                    }
                }
            }
            InformUI();
            InformQuickSlotUI();
        }


        private bool SearchForItem(string itemType)
        {
            for (int i = 0; i < Container.Count; i++)
            {
                if (!Container[i].IsEmpty && Container[i].item.ItemType.ToString() == itemType)
                {
                    return true;
                }
            }
            return false;
        }
        private bool SearchForItemStackable(ItemSO item, out int index)
        {
            for (int i = 0; i < Container.Count; i++)
            {
                if (!Container[i].IsEmpty && Container[i].item == item && Container[i].quantity < Container[i].item.MaxStackSize)
                {
                    index = i;
                    return true;
                }
            }
            index = -1;
            return false;
        }
        private bool SearchForEmptySlot(out int index)
        {
            for (int i = 0; i < Container.Count; i++)
            {
                if (Container[i].IsEmpty)
                {
                    index = i;
                    return true;
                }
            }
            index = -1;
            return false;
        }


        private void InformUI()
        {
            CheckForInventoryGridEnd();
            OnInventoryUpdated?.Invoke();
        }
        private void InformQuickSlotUI()
        {
            OnQuickSlotUpdated?.Invoke();
        }
        private void InformEquipmentUI()
        {
            OnEquipmentUpdated?.Invoke();
        }

        public void CorrectQuantity()
        {
            if (Container.Count > MAX_ITEM_SLOTS)
            {
                int startIndex = Container.Count - 1;
                int reminder = Container.Count - (Container.Count - MAX_ITEM_SLOTS);
                for (int i = startIndex; i >= reminder; i--)
                {
                    Container.RemoveAt(i);
                }
            }
            for (int i = 0; i < Container.Count; i++)
            {
                if (!Container[i].IsEmpty && Container[i].quantity > Container[i].item.MaxStackSize)
                {
                    ItemSO tempItem = Container[i].item;
                    Container[i] = new InventoryItem(tempItem, tempItem.MaxStackSize);
                }
            }
        }
        public void CheckForInventoryGridEnd()
        {
            // Making grid end perfectly.
            if (Container.Count % 6 != 0)
            {
                int iterations = (((Container.Count / 6) + 1) * 6) - Container.Count;
                for (int i = 0; i < iterations; i++)
                {
                    Container.Add(InventoryItem.GetEmptyItem());
                }
            }
            // Cheking if rows number should be increased.
            for (int i = Container.Count - 1; i > Container.Count - 7; i--)
            {
                if (Rows < 7)
                {
                    if (!Container[i].IsEmpty)
                    {
                        for (int j = 0; j < 6; j++)
                        {
                            Container.Add(InventoryItem.GetEmptyItem());
                        }
                    }
                }
                else
                {
                    break;
                }
            }
            // Cheking if rows number should be decreased. (Maybe should disable if increase was true?)
            bool stopSearch = false;
            do
            {
                //Rows = Container.Count / 6;
                if (Rows > 4)
                {
                    for (int i = Container.Count - 1; i > Container.Count - 13; i--)
                    {
                        if (!Container[i].IsEmpty)
                        {
                            return;
                        }
                    }
                    int startPoint = Container.Count - 1;
                    for (int j = startPoint; j > startPoint - 6; j--)
                    {
                        Container.RemoveAt(Container.Count - 1);
                    }
                }
                else
                {
                    stopSearch = true;
                }
            }
            while (!stopSearch);
        }
        public void CorrectQuickSlotQuantity()
        {
            if (QSContainer.Count > MAX_QUICK_SLOTS)
            {
                int startIndex = QSContainer.Count - 1;
                int reminder = QSContainer.Count - (QSContainer.Count - MAX_QUICK_SLOTS);
                for (int i = startIndex; i >= reminder; i--)
                {
                    QSContainer.RemoveAt(i);
                }
            }
            for (int i = 0; i < QSContainer.Count; i++)
            {
                if (!QSContainer[i].IsEmpty && QSContainer[i].quantity > QSContainer[i].item.MaxStackSize)
                {
                    ItemSO tempItem = QSContainer[i].item;
                    QSContainer[i] = new QuickSlotItem(tempItem, tempItem.MaxStackSize);
                }
            }
            for (int i = 0; i < QSContainer.Count; i++)
            {
                if (QSContainer[i].IsEmpty)
                    continue;

            }
        }
        public void CorrectEquipSlotsQuantity()
        {
            // Correcting Number of Equip Slots
            if (EquipContainer.Count > MAX_EQUIPMENT_SLOTS)
            {
                int startIndex = EquipContainer.Count - 1;
                int reminder = EquipContainer.Count - (EquipContainer.Count - MAX_EQUIPMENT_SLOTS);
                for (int i = startIndex; i >= reminder; i--)
                {
                    EquipContainer.RemoveAt(i);
                }
            }
            else
                if (EquipContainer.Count < MAX_EQUIPMENT_SLOTS)
            {
                int reminder = MAX_EQUIPMENT_SLOTS - EquipContainer.Count;
                for (int i = 0; i < reminder; i++)
                {
                    EquipContainer.Add(EquipmentItem.GetEmptyEquipmentItem());
                }
            }
            // Correcting Type of Slot
            for (int i = 0; i < MAX_EQUIPMENT_SLOTS; i++)
            {
                EquipContainer[i] = new EquipmentItem(EquipContainer[i].item, EquipContainer[i].quantity, (EquipmentItem.SlotType)i + 1);
            }
            // Checking for Allowed Slot Item and Correcting quantity.
            //for (int i = 0; i < MAX_EQUIPMENT_SLOTS; i++)
            //{
            //    if (EquipContainer[i].IsEmpty)
            //    {
            //        EquipContainer[i] = new EquipmentItem(EquipContainer[i].slotType);
            //        continue;
            //    }
            //    else
            //    {
            //        if(EquipContainer[i].item.SlotsToEquipIn.Contains((ItemSO.EquipSlots)EquipContainer[i].slotType))
            //        {
            //            if (EquipContainer[i].slotType != EquipmentItem.SlotType._11_AMMO)
            //                EquipContainer[i] = new EquipmentItem(EquipContainer[i].item, 1, EquipContainer[i].slotType);
            //            else
            //                if (EquipContainer[i].quantity > EquipContainer[i].item.MaxStackSize)
            //                EquipContainer[i] = new EquipmentItem(EquipContainer[i].item, EquipContainer[i].item.MaxStackSize, EquipContainer[i].slotType);
            //        }
            //        else
            //        {
            //            EquipContainer[i] = new EquipmentItem(EquipContainer[i].slotType);
            //        }
            //    }
            //}
        }

        public InventoryItem GetItemAt(int index)
        {
            return Container[index];
        }
        public QuickSlotItem GetQuickSlotItemAt(int index)
        {
            return QSContainer[index];
        }
        public Dictionary<int, InventoryItem> GetCurrentInventoryState()
        {
            Dictionary<int, InventoryItem> dictionary = new Dictionary<int, InventoryItem>();
            for (int i = 0; i < Container.Count; i++)
            {
                if (Container[i].IsEmpty)
                    dictionary[i] = InventoryItem.GetEmptyItem();
                else
                    dictionary[i] = Container[i];
            }
            return dictionary;
        }
    }


    [Serializable]
    public struct InventoryItem
    {
        // Is a property. Returning True if item==null, or False if not.
        public bool IsEmpty => item == null || quantity <= 0;

        public ItemSO item;
        public int quantity;

        public InventoryItem(ItemSO item, int quantity)
        {
            this.item = item;
            this.quantity = quantity;
        }
        public static InventoryItem GetEmptyItem()
        {
            return new InventoryItem
            {
                item = null,
                quantity = 0,
            };
        }
    }
    [Serializable]
    public struct QuickSlotItem
    {
        public bool IsEmpty => item == null || quantity <= 0;

        public ItemSO item;
        public int quantity;

        public static List<string> acceptedSlots = new List<string>
    {
        "_12_QUICK_SLOT",
    };

        public QuickSlotItem(ItemSO item, int quantity)
        {
            this.item = item;
            this.quantity = quantity;
        }
        public static QuickSlotItem GetEmptyQuickSlotItem()
        {
            return new QuickSlotItem
            {
                item = null,
                quantity = 0,
            };
        }
    }
    [Serializable]
    public struct EquipmentItem
    {
        public bool IsEmpty => item == null || quantity <= 0;

        public enum SlotType
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

        public ItemSO item;
        public int quantity;
        public SlotType slotType;


        public EquipmentItem(ItemSO item, int quantity, SlotType type)
        {
            this.item = item;
            this.quantity = quantity;
            this.slotType = type;
        }
        public EquipmentItem(SlotType type)
        {
            item = null;
            quantity = 0;
            this.slotType = type;
        }
        public static EquipmentItem GetEmptyEquipmentItem()
        {
            return new EquipmentItem
            {
                item = null,
                quantity = 0,
                slotType = SlotType._0_DEFAULT,
            };
        }
    }
}