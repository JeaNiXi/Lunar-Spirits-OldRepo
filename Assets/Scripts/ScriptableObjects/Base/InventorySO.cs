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
        private const int MIN_ITEM_SLOTS = 24;
        private const int MAX_EQUIPMENT_SLOTS = 11;
        private const int QUICK_SLOT_COUNT = 2;

        public int GetInventorySize() => Container.Count;
        public List<InventoryItem> GetItemList() => Container;
        public List<QuickSlotItem> GetQuickSlotList() => QSContainer;
        public List<EquipmentItem> GetEquipmentItemsList() => EquipContainer;



        //public int AddItem(ItemSO item, int quantity)
        //{
        //if (item.MaxStackSize > 1)
        //{
        //    int index;
        //    int sizeToAdd = quantity;
        //    while (sizeToAdd > 0)
        //    {
        //        if (SearchForItemStackable(item, out index))
        //        {
        //            if (Container[index].quantity + quantity > Container[index].item.MaxStackSize)
        //            {
        //                int maxStackSize = Container[index].item.MaxStackSize;
        //                sizeToAdd = quantity - (Container[index].item.MaxStackSize - Container[index].quantity);
        //                Container[index] = new InventoryItem(item, maxStackSize);
        //            }
        //            else
        //            {
        //                int addSize = Container[index].quantity + sizeToAdd;
        //                Container[index] = new InventoryItem(item, addSize);
        //                InformUI();
        //                return 0;
        //                //sizeToAdd = 0;
        //            }
        //        }
        //        else
        //        {
        //            if (SearchForEmptySlot(out index))
        //            {
        //                if (sizeToAdd > item.MaxStackSize)
        //                {
        //                    Container[index] = new InventoryItem(item, item.MaxStackSize);
        //                    sizeToAdd -= item.MaxStackSize;
        //                }
        //                else
        //                {
        //                    Container[index] = new InventoryItem(item, sizeToAdd);
        //                    InformUI();
        //                    return 0;
        //                    //sizeToAdd = 0;
        //                }
        //            }
        //            else
        //            {
        //                //Should be remade. No Empty Slots, nowhere to stack. Debug Inventory Full!
        //                Debug.Log("STACKABLE Inventory Full!");
        //                InformUI();
        //                return sizeToAdd;
        //                //sizeToAdd = 0;
        //                //if (sizeToAdd > item.MaxStackSize)
        //                //{
        //                //    Container.Add(new InventoryItem(item, item.MaxStackSize));
        //                //    sizeToAdd -= item.MaxStackSize;
        //                //}
        //                //else
        //                //{
        //                //    Container.Add(new InventoryItem(item, sizeToAdd));
        //                //    sizeToAdd = 0;
        //                //}
        //            }
        //        }
        //    }
        //}
        //else
        //{
        //    int index;
        //    int sizeToAdd = quantity;
        //    while (sizeToAdd > 0)
        //    {
        //        if (SearchForEmptySlot(out index))
        //        {
        //            Container[index] = new InventoryItem(item, item.MaxStackSize);
        //            sizeToAdd -= item.MaxStackSize;
        //        }
        //        else
        //        {
        //            //Should be remade. No Empty Slots, nowhere add. Debug Inventory Full!
        //            Debug.Log("NO STACKABLE FULL");
        //            InformUI();
        //            return sizeToAdd;
        //            //sizeToAdd = 0;
        //            //Container.Add(new InventoryItem(item, item.MaxStackSize));
        //            //sizeToAdd -= item.MaxStackSize;
        //        }
        //    }
        //}
        //InformUI();
        //return 0;
        //}

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
            //if (Container[index].IsEmpty)
            //    return;
            //if (Container[index].quantity > 1)
            //{
            //    Container[index] = new InventoryItem(Container[index].item, Container[index].quantity - quantity);
            //}
            //else
            //    Container[index] = InventoryItem.GetEmptyItem();
            //InformUI();
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
            //if (QSContainer[index].IsEmpty)
            //    return;
            //if (QSContainer[index].quantity > 1)
            //{
            //    QSContainer[index] = new QuickSlotItem(QSContainer[index].item, QSContainer[index].quantity - quantity);
            //}
            //else
            //    QSContainer[index] = QuickSlotItem.GetEmptyQuickSlotItem();
            //InformQuickSlotUI();
        }



        //public void MoveInventoryItemToEmptyInventorySlot(int followerIndex, int emptyIndex)
        //{
        //    Container[emptyIndex] = new InventoryItem(Container[followerIndex].item, Container[followerIndex].quantity);
        //    Container[followerIndex] = InventoryItem.GetEmptyItem();
        //    InformUI();
        //}
        //public void MoveInventoryItemToEmptyEquipmentSlot(int followerIndex, int equipIndex)
        //{
        //    EquipContainer[equipIndex] = new EquipmentItem(Container[followerIndex].item, Container[followerIndex].quantity, EquipContainer[equipIndex].slotType);
        //    Container[followerIndex] = InventoryItem.GetEmptyItem();
        //    InformUI();
        //    InformEquipmentUI();
        //}
        #region ItemHandling
        public void SwapItemsHandler(string originContainer, int originIndex, string destContainer, int destIndex)
        {
            switch (originContainer)
            {
                case "Container":
                    switch (destContainer)
                    {
                        case "Container":
                            SwapItems(Container[originIndex], originIndex, Container[destIndex], destIndex);
                            break;
                        case "QSContainer":
                            SwapItems(Container[originIndex], originIndex, QSContainer[destIndex], destIndex);
                            break;
                        case "EquipmentContainer":
                            SwapItems(Container[originIndex], originIndex, EquipContainer[destIndex], destIndex);
                            break;
                        default:
                            break;
                    }
                    break;
                case "QSContainer":
                    switch (destContainer)
                    {
                        case "Container":
                            SwapItems(QSContainer[originIndex], originIndex, Container[destIndex], destIndex);
                            break;
                        case "QSContainer":
                            SwapItems(QSContainer[originIndex], originIndex, QSContainer[destIndex], destIndex);
                            break;
                        case "EquipmentContainer":
                            SwapItems(QSContainer[originIndex], originIndex, EquipContainer[destIndex], destIndex);
                            break;
                        default:
                            break;
                    }
                    break;
                case "EquipmentContainer":
                    switch (destContainer)
                    {
                        case "Container":
                            SwapItems(EquipContainer[originIndex], originIndex, Container[destIndex], destIndex);
                            break;
                        case "QSContainer":
                            SwapItems(EquipContainer[originIndex], originIndex, QSContainer[destIndex], destIndex);
                            break;
                        case "EquipmentContainer":
                            SwapItems(EquipContainer[originIndex], originIndex, EquipContainer[destIndex], destIndex);
                            break;
                        default:
                            break;
                    }
                    break;
                default:
                    break;
            }
        }
        public void SwapItems(InventoryItem originItem, int originIndex, InventoryItem destItem, int destIndex)
        {
            bool IsFound = false;
            foreach (var slot in Container[originIndex].item.CanBeInSlots)
            {
                if (CanBeEquipped(slot.ToString(), Container[destIndex].slotType))
                {
                    IsFound = true;
                    if (Container[destIndex].IsEmpty)
                    {
                        Container[destIndex] = new InventoryItem(Container[originIndex].item, Container[originIndex].quantity, Container[originIndex].slotType);
                        Container[originIndex] = InventoryItem.GetEmptyItem();
                        InformUI();
                        break;
                    }
                    else
                    {
                        //Do shit.
                        break;
                    }
                }
            }
            if(!IsFound)
                Debug.Log("Can not Be Equipped Here!");
        }
        public void SwapItems(InventoryItem originItem, int originIndex, QuickSlotItem destItem, int destIndex)
        {
            // In Production.
            bool IsFound = false;
            foreach (var slot in Container[originIndex].item.CanBeInSlots)
            {
                if (CanBeEquipped(slot.ToString(), QSContainer[destIndex].slotType))
                {
                    IsFound = true;
                    if (QSContainer[destIndex].IsEmpty)
                    {
                        QSContainer[destIndex] = new QuickSlotItem(Container[originIndex].item, Container[originIndex].quantity, QSContainer[destIndex].slotType);
                        Container[originIndex] = InventoryItem.GetEmptyItem();
                        InformUI();
                        InformQuickSlotUI();
                        break;
                    }
                    else
                    {
                        if (QSContainer[destIndex].item.ID == Container[originIndex].item.ID)
                        {
                            if (QSContainer[destIndex].quantity < QSContainer[destIndex].item.MaxStackSize)
                            {
                                int possibleSizeToAdd = QSContainer[destIndex].item.MaxStackSize - QSContainer[destIndex].quantity;
                                if (possibleSizeToAdd >= Container[originIndex].quantity)
                                {
                                    int newSize = QSContainer[destIndex].quantity + Container[originIndex].quantity;
                                    QSContainer[destIndex] = new QuickSlotItem(Container[originIndex].item, newSize, QSContainer[destIndex].slotType);
                                    Container[originIndex] = InventoryItem.GetEmptyItem();
                                }
                                else
                                {
                                    int reminder = Container[originIndex].quantity - possibleSizeToAdd;
                                    QSContainer[destIndex] = new QuickSlotItem(Container[originIndex].item, Container[originIndex].item.MaxStackSize, QSContainer[destIndex].slotType);
                                    Container[originIndex] = new InventoryItem(QSContainer[destIndex].item, reminder, Container[originIndex].slotType);
                                }
                            }
                            else
                            {
                                QuickSlotItem tmpQSItem = new QuickSlotItem(QSContainer[destIndex].item, QSContainer[destIndex].quantity, QSContainer[destIndex].slotType);
                                QSContainer[destIndex] = new QuickSlotItem(Container[originIndex].item, Container[originIndex].quantity, QSContainer[destIndex].slotType);
                                Container[originIndex] = new InventoryItem(tmpQSItem.item, tmpQSItem.quantity, Container[originIndex].slotType);
                            }
                        }
                        else
                        {
                            QuickSlotItem tmpQSItem = new QuickSlotItem(QSContainer[destIndex].item, QSContainer[destIndex].quantity, QSContainer[destIndex].slotType);
                            QSContainer[destIndex] = new QuickSlotItem(Container[originIndex].item, Container[originIndex].quantity, QSContainer[destIndex].slotType);
                            Container[originIndex] = new InventoryItem(tmpQSItem.item, tmpQSItem.quantity,Container[originIndex].slotType);
                        }
                        InformUI();
                        InformQuickSlotUI();
                        break;
                    }
                }
            }
            if (!IsFound)
                Debug.Log("Can not Be Equipped Here!");
        }
        public void SwapItems(InventoryItem originItem, int originIndex, EquipmentItem destItem, int destIndex)
        {
            bool IsFound = false;
            foreach(var slot in Container[originIndex].item.CanBeInSlots)
            {
                if(CanBeEquipped(slot.ToString(), EquipContainer[destIndex].slotType))
                {
                    IsFound = true;
                    if(EquipContainer[destIndex].IsEmpty)
                    {
                        EquipContainer[destIndex] = new EquipmentItem(Container[originIndex].item, Container[originIndex].quantity, EquipContainer[destIndex].slotType);
                        Container[originIndex] = InventoryItem.GetEmptyItem();
                        InformUI();
                        InformEquipmentUI();
                        break;
                    }
                    else
                    {
                        // Do shit.
                        break;
                    }
                }
            }
            if(!IsFound)
                Debug.Log("Can not Be Equipped Here!");
        }
        public void SwapItems(QuickSlotItem originItem, int originIndex, InventoryItem destItem, int destIndex)
        {
            bool IsFound = false;
            foreach (var slot in QSContainer[originIndex].item.CanBeInSlots)
            {
                if (CanBeEquipped(slot.ToString(), Container[destIndex].slotType))
                {
                    IsFound = true;
                    if (Container[destIndex].IsEmpty)
                    {
                        Container[destIndex] = new InventoryItem(QSContainer[originIndex].item, QSContainer[originIndex].quantity, Container[destIndex].slotType);
                        QSContainer[originIndex] = QuickSlotItem.GetEmptyQuickSlotItem();
                        InformUI();
                        InformQuickSlotUI();
                        break;
                    }
                    else
                    {
                        if (QSContainer[originIndex].item.ID == Container[destIndex].item.ID)
                        {
                            // same items, we can add from qs to inventory and leave reminder of add all
                            Debug.Log("SAME ITEMS LETS SWAP!");
                            break;
                        }
                        else
                        {
                            bool IsOtherFound = false;
                            foreach (var otherSlot in Container[destIndex].item.CanBeInSlots)
                            {
                                if (CanBeEquipped(otherSlot.ToString(), QSContainer[originIndex].slotType))
                                {
                                    IsOtherFound = true;
                                    //item from container can be quick slot we can check for swap
                                    //we swap items here
                                    Debug.Log("NOT SAME ITEMS BUT WE CAN SWAP LETS SWAP!");

                                    break;
                                }
                            }
                            if(!IsOtherFound)
                                Debug.Log("CONT ITEM Can not Be Equipped IN QS!");
                        }
                    }
                }
            }
            if(!IsFound)
                Debug.Log("QS ITEM Can not Be Equipped Here!");
        }





        //                if (QSContainer[originIndex].item.ID == Container[destIndex].item.ID) 
        //                {
        //                    if (QSContainer[originIndex].item.MaxStackSize > 1)
        //                    {
        //                        int newIndex;
        //                        int sizeToAdd;
        //                        int reminder = QSContainer[index].quantity;
        //                        while (reminder > 0)
        //                        {
        //                            if (SearchForItemStackable(QSContainer[index].item, out newIndex))
        //                            {
        //                                if (Container[newIndex].quantity + reminder > Container[newIndex].item.MaxStackSize)
        //                                {
        //                                    sizeToAdd = Container[newIndex].item.MaxStackSize - Container[newIndex].quantity;
        //                                    Container[newIndex] = new InventoryItem(QSContainer[index].item, QSContainer[index].item.MaxStackSize);
        //                                    reminder -= sizeToAdd;
        //                                }
        //                                else
        //                                {
        //                                    sizeToAdd = Container[newIndex].quantity + reminder;
        //                                    Container[newIndex] = new InventoryItem(QSContainer[index].item, sizeToAdd);
        //                                    QSContainer[index] = QuickSlotItem.GetEmptyQuickSlotItem();
        //                                    InformUI();
        //                                    InformQuickSlotUI();
        //                                    return;
        //                                }
        //                            }
        //                            else
        //                            {
        //                                if (SearchForEmptySlot(out newIndex))
        //                                {
        //                                    if (reminder > QSContainer[index].item.MaxStackSize)
        //                                    {
        //                                        sizeToAdd = reminder - QSContainer[index].item.MaxStackSize;
        //                                        Container[newIndex] = new InventoryItem(QSContainer[index].item, QSContainer[index].item.MaxStackSize);
        //                                        reminder -= sizeToAdd;
        //                                    }
        //                                    else
        //                                    {
        //                                        Container[newIndex] = new InventoryItem(QSContainer[index].item, reminder);
        //                                        QSContainer[index] = QuickSlotItem.GetEmptyQuickSlotItem();
        //                                        InformUI();
        //                                        InformQuickSlotUI();
        //                                        return;
        //                                    }
        //                                }
        //                                else
        //                                {
        //                                    QSContainer[index] = new QuickSlotItem(QSContainer[index].item, reminder);
        //                                    Debug.Log("NO EMPTY SLOTS TO UNEQUIP QUICK SLOT STACKABLE ITEM, ITEMS LEFT: " + reminder);
        //                                    InformUI();
        //                                    InformQuickSlotUI();
        //                                    return;
        //                                }
        //                            }
        //                        }
        //                    }
        //                    else
        //                    {
        //                        int newIndex;
        //                        int sizeToAdd = QSContainer[index].quantity;
        //                        while (sizeToAdd > 0)
        //                        {
        //                            if (SearchForEmptySlot(out newIndex))
        //                            {
        //                                Container[newIndex] = new InventoryItem(QSContainer[index].item, QSContainer[index].item.MaxStackSize);
        //                                sizeToAdd -= QSContainer[index].item.MaxStackSize;
        //                            }
        //                            else
        //                            {
        //                                QSContainer[index] = new QuickSlotItem(QSContainer[index].item, sizeToAdd);
        //                                Debug.Log("NO EMPTY SLOTS TO UNEQUIP QUICK SLOT UNSTACKABLE ITEM, ITEMS LEFT: " + sizeToAdd);
        //                                InformUI();
        //                                InformQuickSlotUI();
        //                                return;
        //                            }
        //                        }
        //                    }
        //                }



        //                InformUI();
        //                InformQuickSlotUI();
        //                //do shit
        //                break;
        //            }
        //        }
        //    }
        //    if(!IsFound)
        //        Debug.Log("Can not Be Equipped Here!");
        //}
        public void SwapItems(QuickSlotItem originItem, int originIndex, QuickSlotItem destItem, int destIndex)
        {

        }
        public void SwapItems(QuickSlotItem originItem, int originIndex, EquipmentItem destItem, int destIndex)
        {

        }
        public void SwapItems(EquipmentItem originItem, int originIndex, InventoryItem destItem, int destIndex)
        {

        }
        public void SwapItems(EquipmentItem originItem, int originIndex, QuickSlotItem destItem, int destIndex)
        {

        }
        public void SwapItems(EquipmentItem originItem, int originIndex, EquipmentItem destItem, int destIndex)
        {

        }
        #endregion

        #region HelperMethods
        private bool CanBeEquipped(string itemPossibleEquipSlot, InventoryItem.SlotType slotType) => itemPossibleEquipSlot == slotType.ToString();
        private bool CanBeEquipped(string itemPossibleEquipSlot, QuickSlotItem.SlotType slotType) => itemPossibleEquipSlot == slotType.ToString();
        private bool CanBeEquipped(string itemPossibleEquipSlot, EquipmentItem.SlotType slotType) => itemPossibleEquipSlot == slotType.ToString();
        #endregion




        public void EquipToQuickSlot(int index, int quickSlotIndex)
        {
            //if (Container[index].IsEmpty)
            //    return;
            //if (QSContainer[quickSlotIndex].IsEmpty)
            //{
            //    QSContainer[quickSlotIndex] = new QuickSlotItem(Container[index].item, Container[index].quantity);
            //    Container[index] = InventoryItem.GetEmptyItem();
            //}
            //else
            //{
            //    if (QSContainer[quickSlotIndex].item.ID == Container[index].item.ID)
            //    {
            //        if (QSContainer[quickSlotIndex].quantity < QSContainer[quickSlotIndex].item.MaxStackSize)
            //        {
            //            int possibleSizeToAdd = QSContainer[quickSlotIndex].item.MaxStackSize - QSContainer[quickSlotIndex].quantity;
            //            if (possibleSizeToAdd >= Container[index].quantity)
            //            {
            //                int newSize = QSContainer[quickSlotIndex].quantity + Container[index].quantity;
            //                QSContainer[quickSlotIndex] = new QuickSlotItem(Container[index].item, newSize);
            //                Container[index] = InventoryItem.GetEmptyItem();
            //            }
            //            else
            //            {
            //                int reminder = Container[index].quantity - possibleSizeToAdd;
            //                QSContainer[quickSlotIndex] = new QuickSlotItem(Container[index].item, Container[index].item.MaxStackSize);
            //                Container[index] = new InventoryItem(QSContainer[quickSlotIndex].item, reminder);
            //            }
            //        }
            //        else
            //        {
            //            QuickSlotItem tmpQSItem = new QuickSlotItem(QSContainer[quickSlotIndex].item, QSContainer[quickSlotIndex].quantity);
            //            QSContainer[quickSlotIndex] = new QuickSlotItem(Container[index].item, Container[index].quantity);
            //            Container[index] = new InventoryItem(tmpQSItem.item, tmpQSItem.quantity);
            //        }
            //    }
            //    else
            //    {
            //        QuickSlotItem tmpQSItem = new QuickSlotItem(QSContainer[quickSlotIndex].item, QSContainer[quickSlotIndex].quantity);
            //        QSContainer[quickSlotIndex] = new QuickSlotItem(Container[index].item, Container[index].quantity);
            //        Container[index] = new InventoryItem(tmpQSItem.item, tmpQSItem.quantity);
            //    }
            //}
            InformUI();
            InformQuickSlotUI();
        }
        public void UnequipQuitSlot(int index)
        {
            //if (QSContainer[index].IsEmpty)
            //    return;
            //if (QSContainer[index].item.MaxStackSize > 1)
            //{
            //    int newIndex;
            //    int sizeToAdd;
            //    int reminder = QSContainer[index].quantity;
            //    while (reminder > 0)
            //    {
            //        if (SearchForItemStackable(QSContainer[index].item, out newIndex))
            //        {
            //            if (Container[newIndex].quantity + reminder > Container[newIndex].item.MaxStackSize)
            //            {
            //                sizeToAdd = Container[newIndex].item.MaxStackSize - Container[newIndex].quantity;
            //                Container[newIndex] = new InventoryItem(QSContainer[index].item, QSContainer[index].item.MaxStackSize);
            //                reminder -= sizeToAdd;
            //            }
            //            else
            //            {
            //                sizeToAdd = Container[newIndex].quantity + reminder;
            //                Container[newIndex] = new InventoryItem(QSContainer[index].item, sizeToAdd);
            //                QSContainer[index] = QuickSlotItem.GetEmptyQuickSlotItem();
            //                InformUI();
            //                InformQuickSlotUI();
            //                return;
            //            }
            //        }
            //        else
            //        {
            //            if (SearchForEmptySlot(out newIndex))
            //            {
            //                if (reminder > QSContainer[index].item.MaxStackSize)
            //                {
            //                    sizeToAdd = reminder - QSContainer[index].item.MaxStackSize;
            //                    Container[newIndex] = new InventoryItem(QSContainer[index].item, QSContainer[index].item.MaxStackSize);
            //                    reminder -= sizeToAdd;
            //                }
            //                else
            //                {
            //                    Container[newIndex] = new InventoryItem(QSContainer[index].item, reminder);
            //                    QSContainer[index] = QuickSlotItem.GetEmptyQuickSlotItem();
            //                    InformUI();
            //                    InformQuickSlotUI();
            //                    return;
            //                }
            //            }
            //            else
            //            {
            //                QSContainer[index] = new QuickSlotItem(QSContainer[index].item, reminder);
            //                Debug.Log("NO EMPTY SLOTS TO UNEQUIP QUICK SLOT STACKABLE ITEM, ITEMS LEFT: " + reminder);
            //                InformUI();
            //                InformQuickSlotUI();
            //                return;
            //            }
            //        }
            //    }
            //}
            //else
            //{
            //    int newIndex;
            //    int sizeToAdd = QSContainer[index].quantity;
            //    while (sizeToAdd > 0)
            //    {
            //        if (SearchForEmptySlot(out newIndex))
            //        {
            //            Container[newIndex] = new InventoryItem(QSContainer[index].item, QSContainer[index].item.MaxStackSize);
            //            sizeToAdd -= QSContainer[index].item.MaxStackSize;
            //        }
            //        else
            //        {
            //            QSContainer[index] = new QuickSlotItem(QSContainer[index].item, sizeToAdd);
            //            Debug.Log("NO EMPTY SLOTS TO UNEQUIP QUICK SLOT UNSTACKABLE ITEM, ITEMS LEFT: " + sizeToAdd);
            //            InformUI();
            //            InformQuickSlotUI();
            //            return;
            //        }
            //    }
            //}
            //InformUI();
            //InformQuickSlotUI();
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

        #region InformingUI
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
        #endregion

        #region Correcting SO
        public void CorrectQuantity()
        {
            if (Container.Count > MAX_ITEM_SLOTS)
            {
                int startIndex = Container.Count - 1;
                for (int i = startIndex; i >= MAX_ITEM_SLOTS; i--)
                {
                    Container.RemoveAt(i);
                }
            }
            if (Container.Count < MIN_ITEM_SLOTS)
            {
                int reminder = MIN_ITEM_SLOTS - Container.Count;
                for (int i = 0; i < reminder; i++)
                {
                    Container.Add(InventoryItem.GetEmptyItem());
                }
            }
            for (int i = 0; i < Container.Count; i++)
            {
                if (!Container[i].IsEmpty && Container[i].quantity > Container[i].item.MaxStackSize)
                {
                    ItemSO tempItem = Container[i].item;
                    Container[i] = new InventoryItem(tempItem, tempItem.MaxStackSize, InventoryItem.SlotType.MAIN_SLOT);
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
            if (QSContainer.Count > QUICK_SLOT_COUNT)
            {
                int startIndex = QSContainer.Count - 1;
                for (int i = startIndex; i >= QUICK_SLOT_COUNT; i--)
                {
                    QSContainer.RemoveAt(i);
                }
            }
            if (QSContainer.Count < QUICK_SLOT_COUNT)
            {
                int reminder = QUICK_SLOT_COUNT - QSContainer.Count;
                for (int i = 0; i < reminder; i++)
                {
                    QSContainer.Add(QuickSlotItem.GetEmptyQuickSlotItem());
                }
            }
            for (int i = 0; i < QSContainer.Count; i++)
            {
                if (QSContainer[i].IsEmpty)
                {
                    QSContainer[i] = QuickSlotItem.GetEmptyQuickSlotItem();
                    continue;
                }
                else
                {
                    bool ItemFound = false;

                    foreach (ItemSO.ItemSlots slotType in QSContainer[i].item.CanBeInSlots)
                    {
                        if (slotType.ToString() == QSContainer[i].slotType.ToString())
                        {
                            ItemFound = true;
                            if (QSContainer[i].quantity > QSContainer[i].item.MaxStackSize)
                            {
                                QSContainer[i] = new QuickSlotItem(QSContainer[i].item, QSContainer[i].item.MaxStackSize, QuickSlotItem.SlotType.QUICK_SLOT);
                                break;
                            }
                            break;
                        }
                    }
                    if (!ItemFound)
                        QSContainer[i] = QuickSlotItem.GetEmptyQuickSlotItem();
                }
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
            else if (EquipContainer.Count < MAX_EQUIPMENT_SLOTS)
            {
                for (int i = EquipContainer.Count; i < MAX_EQUIPMENT_SLOTS; i++)
                {
                    EquipContainer.Add(EquipmentItem.GetEmptyEquipmentItem());
                }
            }

            //Correcting Type of Slot
            for (int i = 0; i < MAX_EQUIPMENT_SLOTS; i++)
            {
                EquipContainer[i] = new EquipmentItem(EquipContainer[i].item, EquipContainer[i].quantity, (EquipmentItem.SlotType)i);
            }

            // Checking for Allowed Slot Item and Correcting quantity.
            for (int i = 0; i < MAX_EQUIPMENT_SLOTS; i++)
            {
                if (EquipContainer[i].IsEmpty)
                {
                    EquipContainer[i] = new EquipmentItem(EquipContainer[i].slotType);
                    continue;
                }
                else
                {
                    bool ItemFound = false;

                    foreach (var slotType in EquipContainer[i].item.CanBeInSlots)
                    {
                        if (slotType.ToString() == EquipContainer[i].slotType.ToString())
                        {
                            ItemFound = true;
                            if (EquipContainer[i].slotType != EquipmentItem.SlotType.AMMO)
                            {
                                EquipContainer[i] = new EquipmentItem(EquipContainer[i].item, 1, EquipContainer[i].slotType);
                                break;
                            }
                            else
                            if (EquipContainer[i].quantity > EquipContainer[i].item.MaxStackSize)
                            {
                                EquipContainer[i] = new EquipmentItem(EquipContainer[i].item, EquipContainer[i].item.MaxStackSize, EquipContainer[i].slotType);
                                break;
                            }
                            break;
                        }
                    }
                    if (!ItemFound)
                        EquipContainer[i] = new EquipmentItem(EquipContainer[i].slotType);
                }
            }
        }
        #endregion


        public InventoryItem GetItemAt(int index)
        {
            return Container[index];
        }
        public QuickSlotItem GetQuickSlotItemAt(int index)
        {
            return QSContainer[index];
        }
        public EquipmentItem GetEquipmentItemAt(int index)
        {
            return EquipContainer[index];
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

    #region Structures
    [Serializable]
    public struct InventoryItem
    {
        // Is a property. Returning True if item==null, or False if not.
        public bool IsEmpty => item == null || quantity <= 0;
        public enum SlotType
        {
            MAIN_SLOT,
        }
        public enum ItemContainer
        {
            Container,
        }
        public ItemSO item;
        public int quantity;
        public SlotType slotType;
        public ItemContainer itemContainer;

        public InventoryItem(ItemSO item, int quantity, SlotType slotType)
        {
            this.item = item;
            this.quantity = quantity;
            this.slotType = slotType;
            this.itemContainer = ItemContainer.Container;
        }
        public static InventoryItem GetEmptyItem()
        {
            return new InventoryItem
            {
                item = null,
                quantity = 0,
                slotType = SlotType.MAIN_SLOT,
                itemContainer = ItemContainer.Container,
            };
        }
    }
    [Serializable]
    public struct QuickSlotItem
    {
        public bool IsEmpty => item == null || quantity <= 0;
        public enum SlotType
        {
            QUICK_SLOT
        }
        public enum ItemContainer
        {
            QSContainer,
        }
        public ItemSO item;
        public int quantity;
        public SlotType slotType;
        public ItemContainer itemContainer;

        public QuickSlotItem(ItemSO item, int quantity, SlotType slotType)
        {
            this.item = item;
            this.quantity = quantity;
            this.slotType = slotType;
            this.itemContainer = ItemContainer.QSContainer;
        }
        public static QuickSlotItem GetEmptyQuickSlotItem()
        {
            return new QuickSlotItem
            {
                item = null,
                quantity = 0,
                slotType = SlotType.QUICK_SLOT,
                itemContainer = ItemContainer.QSContainer,
            };
        }
    }
    [Serializable]
    public struct EquipmentItem
    {
        public bool IsEmpty => item == null || quantity <= 0;

        public enum SlotType
        {
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
            DEFAULT,
        }
        public enum ItemContainer
        {
            EquipmentContainer,
        }

        public ItemSO item;
        public int quantity;
        public SlotType slotType;
        public ItemContainer itemContainer;

        public EquipmentItem(ItemSO item, int quantity, SlotType slotType)
        {
            this.item = item;
            this.quantity = quantity;
            this.slotType = slotType;
            this.itemContainer = ItemContainer.EquipmentContainer;
        }
        public EquipmentItem(SlotType type)
        {
            item = null;
            quantity = 0;
            slotType = type;
            itemContainer = ItemContainer.EquipmentContainer;
        }
        public static EquipmentItem GetEmptyEquipmentItem()
        {
            return new EquipmentItem
            {
                item = null,
                quantity = 0,
                slotType = SlotType.DEFAULT,
                itemContainer = ItemContainer.EquipmentContainer,
            };
        }
    }
}
#endregion