using Helpers.SO;
using Inventory.UI;
using Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Tables;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.Metadata;

namespace Inventory.SO
{
    [CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory/Base/Inventory")]
    public class InventorySO : ScriptableObject
    {
        public event Action
            OnInventoryUpdated,
            OnQuickSlotUpdated,
            OnEquipmentUpdated;
        public event Action<UINotifications.Notifications>
            ThrowNotification;
        public event Action<EquipmentItem>
            OnItemEquipped,
            OnItemUnequipped;

        [SerializeField] [NonReorderable] private List<InventoryItem> Container = new List<InventoryItem>(24);
        [SerializeField] [NonReorderable] private List<QuickSlotItem> QSContainer = new List<QuickSlotItem>(2);
        [SerializeField] [NonReorderable] private List<EquipmentItem> EquipContainer = new List<EquipmentItem>(11);
        [SerializeField] [NonReorderable] private List<InventoryItem> LootContainer = new List<InventoryItem>();

        public LocalizedStringTable RaritiesTable = new LocalizedStringTable();

        private int Rows { get => Container.Count / 6; }
        private const int MAX_ITEM_SLOTS = 42;
        private const int MIN_ITEM_SLOTS = 24;
        private const int MAX_EQUIPMENT_SLOTS = 11;
        private const int QUICK_SLOT_COUNT = 2;

        public int GetInventorySize() => Container.Count;
        public List<InventoryItem> GetItemList() => Container;
        public List<QuickSlotItem> GetQuickSlotList() => QSContainer;
        public List<EquipmentItem> GetEquipmentItemsList() => EquipContainer;
        public List<InventoryItem> GetEquippedInventoryItemList()
        {
            List<InventoryItem> newList = new List<InventoryItem>();
            for (int i = 0; i < EquipContainer.Count; i++)
            {
                if (EquipContainer[i].IsEmpty)
                    continue;
                newList.Add(new InventoryItem(EquipContainer[i].item, 1, InventoryItem.SlotType.MAIN_SLOT, EquipContainer[i].itemRarity, EquipContainer[i].itemParameters));
            }
            return newList;
        }
        public void SetLootContainer(List<InventoryItem> lootList) => LootContainer = lootList;
        public List<InventoryItem> GetLootList() => LootContainer;


        #region ItemHandling
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
                            Container[index] = new InventoryItem(item, maxStackSize, Container[index].slotType);
                        }
                        else
                        {
                            int addSize = Container[index].quantity + sizeToAdd;
                            Container[index] = new InventoryItem(item, addSize, Container[index].slotType);
                            InformUI();
                            return 0;
                        }
                    }
                    else
                    {
                        if (SearchForEmptySlot(out index))
                        {
                            if (sizeToAdd > item.MaxStackSize)
                            {
                                Container[index] = new InventoryItem(item, item.MaxStackSize, Container[index].slotType);
                                sizeToAdd -= item.MaxStackSize;
                            }
                            else
                            {
                                Container[index] = new InventoryItem(item, sizeToAdd, Container[index].slotType);
                                InformUI();
                                return 0;
                            }
                        }
                        else
                        {
                            ThrowNotification?.Invoke(UINotifications.Notifications.NO_INVENTORY_SPACE_LEFT);
                            InformUI();
                            return sizeToAdd;
                        }
                    }
                }
                InformUI();
                return 0;
            }
            else
            {
                int index;
                int sizeToAdd = quantity;
                while (sizeToAdd > 0)
                {
                    if (SearchForEmptySlot(out index))
                    {
                        Container[index] = new InventoryItem(item, item.MaxStackSize, Container[index].slotType);
                        sizeToAdd -= item.MaxStackSize;
                    }
                    else
                    {
                        ThrowNotification?.Invoke(UINotifications.Notifications.NO_INVENTORY_SPACE_LEFT);
                        InformUI();
                        return sizeToAdd;
                    }
                }
                InformUI();
                return 0;
            }
        }
        public void SwapItemsHandler(string originContainer, int originIndex, string destContainer, int destIndex)
        {
            switch (originContainer)
            {
                case "Container":
                    switch (destContainer)
                    {
                        case "Container":
                            if (originIndex == destIndex)
                            {
                                ThrowNotification?.Invoke(UINotifications.Notifications.SAME_ITEM_IN_SLOT);
                                break;
                            }
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
                            if (originIndex == destIndex)
                            {
                                ThrowNotification?.Invoke(UINotifications.Notifications.SAME_ITEM_IN_SLOT);
                                break;
                            }
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
                            if (originIndex == destIndex)
                            {
                                ThrowNotification?.Invoke(UINotifications.Notifications.SAME_ITEM_IN_SLOT);
                                break;
                            }
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
                        Container[destIndex] = new InventoryItem(Container[originIndex].item, Container[originIndex].quantity, Container[originIndex].slotType, Container[originIndex].itemRarity, Container[originIndex].itemParameters);
                        Container[originIndex] = InventoryItem.GetEmptyItem();
                        InformUI();
                        break;
                    }
                    else
                    {
                        if (Container[destIndex].item.MaxStackSize == 1 || Container[originIndex].item.MaxStackSize == 1)
                        {
                            InventoryItem tmpItem = new InventoryItem(Container[originIndex].item, Container[originIndex].quantity, Container[originIndex].slotType);
                            Container[originIndex] = new InventoryItem(Container[destIndex].item, Container[destIndex].quantity, Container[destIndex].slotType);
                            Container[destIndex] = new InventoryItem(tmpItem.item, tmpItem.quantity, tmpItem.slotType);
                            InformUI();
                            break;
                        }
                        else
                        {
                            if (Container[destIndex].item.ID == Container[originIndex].item.ID)
                            {
                                if (Container[destIndex].quantity < Container[destIndex].item.MaxStackSize)
                                {
                                    int sizeWeCanAdd = Container[destIndex].item.MaxStackSize - Container[destIndex].quantity;
                                    if (Container[originIndex].quantity > sizeWeCanAdd)
                                    {
                                        int reminder = Container[originIndex].quantity - sizeWeCanAdd;
                                        Container[destIndex] = new InventoryItem(Container[originIndex].item, Container[originIndex].item.MaxStackSize, Container[originIndex].slotType);
                                        Container[originIndex] = new InventoryItem(Container[destIndex].item, reminder, Container[destIndex].slotType);
                                    }
                                    else
                                    {
                                        int newSize = Container[destIndex].quantity + Container[originIndex].quantity;
                                        Container[destIndex] = new InventoryItem(Container[originIndex].item, newSize, Container[originIndex].slotType);
                                        Container[originIndex] = InventoryItem.GetEmptyItem();
                                    }
                                    InformUI();
                                    break;
                                }
                                else
                                {
                                    InventoryItem tmpItem = new InventoryItem(Container[originIndex].item, Container[originIndex].quantity, Container[originIndex].slotType);
                                    Container[originIndex] = new InventoryItem(Container[destIndex].item, Container[destIndex].quantity, Container[destIndex].slotType);
                                    Container[destIndex] = new InventoryItem(tmpItem.item, tmpItem.quantity, tmpItem.slotType);
                                    InformUI();
                                    break;
                                }
                            }
                            else
                            {
                                InventoryItem tmpItem = new InventoryItem(Container[originIndex].item, Container[originIndex].quantity, Container[originIndex].slotType);
                                Container[originIndex] = new InventoryItem(Container[destIndex].item, Container[destIndex].quantity, Container[destIndex].slotType);
                                Container[destIndex] = new InventoryItem(tmpItem.item, tmpItem.quantity, tmpItem.slotType);
                                InformUI();
                                break;
                            }
                        }
                    }
                }
            }
            if (!IsFound)
                ThrowNotification?.Invoke(UINotifications.Notifications.WRONG_ITEM_TYPE);
        }
        public void SwapItems(InventoryItem originItem, int originIndex, QuickSlotItem destItem, int destIndex)
        {
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
                            Container[originIndex] = new InventoryItem(tmpQSItem.item, tmpQSItem.quantity, Container[originIndex].slotType);
                        }
                        InformUI();
                        InformQuickSlotUI();
                        break;
                    }
                }
            }
            if (!IsFound)
                ThrowNotification?.Invoke(UINotifications.Notifications.WRONG_ITEM_TYPE);
        }
        public void SwapItems(InventoryItem originItem, int originIndex, EquipmentItem destItem, int destIndex)
        {
            bool IsFound = false;
            foreach (var slot in Container[originIndex].item.CanBeInSlots)
            {
                if (CanBeEquipped(slot.ToString(), EquipContainer[destIndex].slotType))
                {
                    IsFound = true;
                    if (EquipContainer[destIndex].IsEmpty)
                    {
                        EquipContainer[destIndex] = new EquipmentItem(Container[originIndex].item, Container[originIndex].quantity, EquipContainer[destIndex].slotType, Container[originIndex].itemRarity, Container[originIndex].itemParameters);
                        Container[originIndex] = InventoryItem.GetEmptyItem();
                        OnItemEquipped?.Invoke(EquipContainer[destIndex]);
                        InformUI();
                        InformEquipmentUI();
                        break;
                    }
                    else
                    {
                        if (EquipContainer[destIndex].slotType == EquipmentItem.SlotType.AMMO && EquipContainer[destIndex].item.ID == Container[originIndex].item.ID)
                        {
                            if (EquipContainer[destIndex].quantity < EquipContainer[destIndex].item.MaxStackSize)
                            {
                                int sizeToAdd = EquipContainer[destIndex].item.MaxStackSize - EquipContainer[destIndex].quantity;
                                if (Container[originIndex].quantity <= sizeToAdd)
                                {
                                    int newSize = EquipContainer[destIndex].quantity + Container[originIndex].quantity;
                                    EquipContainer[destIndex] = new EquipmentItem(Container[originIndex].item, newSize, EquipContainer[destIndex].slotType);
                                    Container[originIndex] = InventoryItem.GetEmptyItem();
                                }
                                else
                                {
                                    int reminder = Container[originIndex].quantity - sizeToAdd;
                                    EquipContainer[destIndex] = new EquipmentItem(Container[originIndex].item, EquipContainer[destIndex].item.MaxStackSize, EquipContainer[destIndex].slotType);
                                    Container[originIndex] = new InventoryItem(EquipContainer[destIndex].item, reminder, Container[originIndex].slotType);
                                }
                                InformUI();
                                InformEquipmentUI();
                                break;
                            }
                        }
                        InventoryItem tmpItem = new InventoryItem(EquipContainer[destIndex].item, EquipContainer[destIndex].quantity, Container[originIndex].slotType);
                        EquipContainer[destIndex] = new EquipmentItem(Container[originIndex].item, Container[originIndex].quantity, EquipContainer[destIndex].slotType);
                        Container[originIndex] = new InventoryItem(tmpItem.item, tmpItem.quantity, tmpItem.slotType);
                        InformUI();
                        InformEquipmentUI();
                        break;
                    }
                }
            }
            if (!IsFound)
                ThrowNotification?.Invoke(UINotifications.Notifications.WRONG_ITEM_TYPE);
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
                        if (QSContainer[originIndex].item.ID == Container[destIndex].item.ID && Container[destIndex].quantity < Container[destIndex].item.MaxStackSize)
                        {
                            int sizeToAdd = Container[destIndex].item.MaxStackSize - Container[destIndex].quantity;
                            if (QSContainer[originIndex].quantity > sizeToAdd)
                            {
                                int reminder = QSContainer[originIndex].quantity - sizeToAdd;
                                Container[destIndex] = new InventoryItem(Container[destIndex].item, Container[destIndex].item.MaxStackSize, Container[destIndex].slotType);
                                QSContainer[originIndex] = new QuickSlotItem(QSContainer[originIndex].item, reminder, QSContainer[originIndex].slotType);
                                InformUI();
                                InformQuickSlotUI();
                                break;
                            }
                            else
                            {
                                int newSize = Container[destIndex].quantity + QSContainer[originIndex].quantity;
                                Container[destIndex] = new InventoryItem(Container[destIndex].item, newSize, Container[destIndex].slotType);
                                QSContainer[originIndex] = QuickSlotItem.GetEmptyQuickSlotItem();
                                InformUI();
                                InformQuickSlotUI();
                                break;
                            }
                        }
                        else
                        {
                            bool IsOtherFound = false;
                            foreach (var otherSlot in Container[destIndex].item.CanBeInSlots)
                            {
                                if (CanBeEquipped(otherSlot.ToString(), QSContainer[originIndex].slotType))
                                {
                                    IsOtherFound = true;
                                    InventoryItem newItem = new InventoryItem(QSContainer[originIndex].item, QSContainer[originIndex].quantity, Container[destIndex].slotType);
                                    QSContainer[originIndex] = new QuickSlotItem(Container[destIndex].item, Container[destIndex].quantity, QSContainer[originIndex].slotType);
                                    Container[destIndex] = new InventoryItem(newItem.item, newItem.quantity, newItem.slotType);
                                    InformUI();
                                    InformQuickSlotUI();
                                    break;
                                }
                            }
                            if (!IsOtherFound)
                                ThrowNotification?.Invoke(UINotifications.Notifications.WRONG_ITEM_TYPE);
                        }
                    }
                }
            }
            if (!IsFound)
                ThrowNotification?.Invoke(UINotifications.Notifications.WRONG_ITEM_TYPE);
        }
        public void SwapItems(QuickSlotItem originItem, int originIndex, QuickSlotItem destItem, int destIndex)
        {
            if (QSContainer[destIndex].IsEmpty)
            {
                QSContainer[destIndex] = new QuickSlotItem(QSContainer[originIndex].item, QSContainer[originIndex].quantity, QSContainer[originIndex].slotType);
                QSContainer[originIndex] = QuickSlotItem.GetEmptyQuickSlotItem();
                InformQuickSlotUI();
                return;
            }
            else
            {
                if (QSContainer[originIndex].item.ID == QSContainer[destIndex].item.ID)
                {
                    if (QSContainer[destIndex].quantity < QSContainer[destIndex].item.MaxStackSize)
                    {
                        int sizeToAdd = QSContainer[destIndex].item.MaxStackSize - QSContainer[destIndex].quantity;
                        if (QSContainer[originIndex].quantity <= sizeToAdd)
                        {
                            int tmp = QSContainer[destIndex].quantity + QSContainer[originIndex].quantity;
                            QSContainer[destIndex] = new QuickSlotItem(QSContainer[originIndex].item, tmp, QSContainer[originIndex].slotType);
                            QSContainer[originIndex] = QuickSlotItem.GetEmptyQuickSlotItem();
                            InformQuickSlotUI();
                            return;
                        }
                        else
                        {
                            int tmp = QSContainer[originIndex].quantity - sizeToAdd;
                            QSContainer[destIndex] = new QuickSlotItem(QSContainer[originIndex].item, QSContainer[originIndex].item.MaxStackSize, QSContainer[originIndex].slotType);
                            QSContainer[originIndex] = new QuickSlotItem(QSContainer[destIndex].item, tmp, QSContainer[destIndex].slotType);
                            InformQuickSlotUI();
                            return;
                        }
                    }
                    else
                    {
                        QuickSlotItem tmpItem = new QuickSlotItem(QSContainer[originIndex].item, QSContainer[originIndex].quantity, QSContainer[originIndex].slotType);
                        QSContainer[originIndex] = new QuickSlotItem(QSContainer[destIndex].item, QSContainer[destIndex].quantity, QSContainer[destIndex].slotType);
                        QSContainer[destIndex] = new QuickSlotItem(tmpItem.item, tmpItem.quantity, tmpItem.slotType);
                        InformQuickSlotUI();
                        return;
                    }
                }
                else
                {
                    QuickSlotItem tmpItem = new QuickSlotItem(QSContainer[originIndex].item, QSContainer[originIndex].quantity, QSContainer[originIndex].slotType);
                    QSContainer[originIndex] = new QuickSlotItem(QSContainer[destIndex].item, QSContainer[destIndex].quantity, QSContainer[destIndex].slotType);
                    QSContainer[destIndex] = new QuickSlotItem(tmpItem.item, tmpItem.quantity, tmpItem.slotType);
                    InformQuickSlotUI();
                    return;
                }
            }
        }
        public void SwapItems(QuickSlotItem originItem, int originIndex, EquipmentItem destItem, int destIndex)
        {
            ThrowNotification?.Invoke(UINotifications.Notifications.WRONG_ITEM_TYPE);
            return;
        }
        public void SwapItems(EquipmentItem originItem, int originIndex, InventoryItem destItem, int destIndex)
        {
            bool isFound = false;
            foreach (var slot in EquipContainer[originIndex].item.CanBeInSlots)
            {
                if (CanBeEquipped(slot.ToString(), Container[destIndex].slotType))
                {
                    isFound = true;
                    if (Container[destIndex].IsEmpty)
                    {
                        Container[destIndex] = new InventoryItem(EquipContainer[originIndex].item, EquipContainer[originIndex].quantity, Container[destIndex].slotType);
                        EquipContainer[originIndex] = new EquipmentItem(EquipContainer[originIndex].slotType);
                        InformUI();
                        InformEquipmentUI();
                        break;
                    }
                    else
                    {
                        bool newFound = false;
                        foreach (var newSlot in Container[destIndex].item.CanBeInSlots)
                        {
                            if (CanBeEquipped(newSlot.ToString(), EquipContainer[originIndex].slotType))
                            {
                                newFound = true;
                                if (Container[destIndex].item.ID == EquipContainer[originIndex].item.ID && Container[destIndex].quantity < Container[destIndex].item.MaxStackSize)
                                {
                                    int sizeToAdd = Container[destIndex].item.MaxStackSize - Container[destIndex].quantity;
                                    if (EquipContainer[originIndex].quantity > sizeToAdd)
                                    {
                                        int reminder = EquipContainer[originIndex].quantity - sizeToAdd;
                                        Container[destIndex] = new InventoryItem(Container[destIndex].item, Container[destIndex].item.MaxStackSize, Container[destIndex].slotType);
                                        EquipContainer[originIndex] = new EquipmentItem(EquipContainer[originIndex].item, reminder, EquipContainer[originIndex].slotType);
                                        InformUI();
                                        InformEquipmentUI();
                                        break;
                                    }
                                    else
                                    {
                                        int newSize = Container[destIndex].quantity + EquipContainer[originIndex].quantity;
                                        Container[destIndex] = new InventoryItem(Container[destIndex].item, newSize, Container[destIndex].slotType);
                                        EquipContainer[originIndex] = new EquipmentItem(EquipContainer[originIndex].slotType);
                                        InformUI();
                                        InformEquipmentUI();
                                        break;
                                    }
                                }
                                else
                                {
                                    EquipmentItem tmpItem = new EquipmentItem(EquipContainer[originIndex].item, EquipContainer[originIndex].quantity, EquipContainer[originIndex].slotType);
                                    EquipContainer[originIndex] = new EquipmentItem(Container[destIndex].item, Container[destIndex].quantity, EquipContainer[originIndex].slotType);
                                    Container[destIndex] = new InventoryItem(tmpItem.item, tmpItem.quantity, Container[destIndex].slotType);
                                    InformUI();
                                    InformEquipmentUI();
                                    break;
                                }
                            }
                        }
                        if (!newFound)
                            ThrowNotification?.Invoke(UINotifications.Notifications.WRONG_ITEM_TYPE);
                    }
                }
            }
            if (!isFound)
                ThrowNotification?.Invoke(UINotifications.Notifications.WRONG_ITEM_TYPE);
        }
        public void SwapItems(EquipmentItem originItem, int originIndex, QuickSlotItem destItem, int destIndex)
        {
            ThrowNotification?.Invoke(UINotifications.Notifications.WRONG_ITEM_TYPE);
            return;
        }
        public void SwapItems(EquipmentItem originItem, int originIndex, EquipmentItem destItem, int destIndex)
        {
            bool isFound = false;
            foreach (var slot in EquipContainer[originIndex].item.CanBeInSlots)
            {
                if (CanBeEquipped(slot.ToString(), EquipContainer[destIndex].slotType))
                {
                    isFound = true;
                    EquipmentItem tmpItem = new EquipmentItem(EquipContainer[originIndex].item, EquipContainer[originIndex].quantity, EquipContainer[originIndex].slotType);
                    EquipContainer[originIndex] = new EquipmentItem(EquipContainer[destIndex].item, EquipContainer[destIndex].quantity, EquipContainer[originIndex].slotType);
                    EquipContainer[destIndex] = new EquipmentItem(tmpItem.item, tmpItem.quantity, EquipContainer[destIndex].slotType);
                    InformEquipmentUI();
                    break;
                }
            }
            if (!isFound)
            {
                ThrowNotification?.Invoke(UINotifications.Notifications.WRONG_ITEM_TYPE);
                return;
            }
        }
        public void RemoveItem(int index, int quantity, string ContainerType)
        {
            switch (ContainerType)
            {
                case "Container":
                    RemoveItem(Container, index, quantity);
                    break;
                case "QSContainer":
                    RemoveItem(QSContainer, index, quantity);
                    break;
                case "EquipmentContainer":
                    RemoveItem(EquipContainer, index, quantity);
                    break;
                default:
                    break;
            }
        }
        public void RemoveItem(List<InventoryItem> tmpList, int index)
        {
            if (Container[index].IsEmpty)
                return;
            Container[index] = InventoryItem.GetEmptyItem();
            InformUI();
        }
        public void RemoveItem(List<QuickSlotItem> tmpList, int index)
        {
            if (QSContainer[index].IsEmpty)
                return;
            QSContainer[index] = QuickSlotItem.GetEmptyQuickSlotItem();
            InformQuickSlotUI();
        }
        public void RemoveItem(List<EquipmentItem> tmpList, int index)
        {
            if (EquipContainer[index].IsEmpty)
                return;
            EquipContainer[index] = new EquipmentItem(EquipContainer[index].slotType);
            InformEquipmentUI();
        }
        public void RemoveItem(List<InventoryItem> tmpList, int index, int quantity)
        {
            if (Container[index].IsEmpty)
                return;
            if (Container[index].quantity > 1)
            {
                Container[index] = new InventoryItem(Container[index].item, Container[index].quantity - quantity, Container[index].slotType);
            }
            else
                Container[index] = InventoryItem.GetEmptyItem();
            InformUI();
        }
        public void RemoveItem(List<QuickSlotItem> list, int index, int quantity)
        {
            if (QSContainer[index].IsEmpty)
                return;
            if (QSContainer[index].quantity > 1)
            {
                QSContainer[index] = new QuickSlotItem(QSContainer[index].item, QSContainer[index].quantity - quantity, QSContainer[index].slotType);
            }
            else
                QSContainer[index] = QuickSlotItem.GetEmptyQuickSlotItem();
            InformQuickSlotUI();
        }
        public void RemoveItem(List<EquipmentItem> list, int index, int quantity)
        {

        }
        public bool EquipItemHandler(int index, string containerType)
        {
            switch (containerType)
            {
                case "Container":
                    EquipItem(Container, index, EquipContainer);
                    return true;
                case "EquipmentContainer":
                    UnequipItem(EquipContainer, index, Container);
                    return false;
                default:
                    return false;
            }
        }
        public void EquipItem(List<InventoryItem> originContainer, int originIndex, List<EquipmentItem> destContainer)
        {
            int destIndex = GetEquipmentSlotIndex(ConvertListItemSlotsToEquipmentSlots(Container[originIndex].item.CanBeInSlots));
            SwapItems(Container[originIndex], originIndex, EquipContainer[destIndex], destIndex);
        }
        public void UnequipItem(List<EquipmentItem> originContainer, int originIndex, List<InventoryItem> destContainer)
        {
            if (EquipContainer[originIndex].item.MaxStackSize > 1)
            {
                bool wasChanged = false;
                int reminder = EquipContainer[originIndex].quantity;
                while (reminder > 0)
                {
                    if (SearchForItemStackable(EquipContainer[originIndex].item, out int index))
                    {
                        wasChanged = true;
                        if (Container[index].quantity + reminder > Container[index].item.MaxStackSize)
                        {
                            int sizeToAdd = Container[index].item.MaxStackSize - Container[index].quantity;
                            reminder -= sizeToAdd;
                            Container[index] = new InventoryItem(Container[index].item, Container[index].item.MaxStackSize, Container[index].slotType);
                            EquipContainer[originIndex] = new EquipmentItem(EquipContainer[originIndex].item, reminder, EquipContainer[originIndex].slotType);
                        }
                        else
                        {
                            int newSize = EquipContainer[originIndex].quantity + Container[index].quantity;
                            Container[index] = new InventoryItem(Container[index].item, newSize, Container[index].slotType);
                            EquipContainer[originIndex] = new EquipmentItem(EquipContainer[originIndex].slotType);
                            InformUI();
                            InformEquipmentUI();
                            break;
                        }
                    }
                    else
                    {
                        if (SearchForEmptySlot(out int newIndex)) // Not added reminder > maxStackSize because switching from EquipCont to empty slot won't be more than stack size.
                        {
                            Container[newIndex] = new InventoryItem(EquipContainer[originIndex].item, EquipContainer[originIndex].quantity, Container[newIndex].slotType);
                            EquipContainer[originIndex] = new EquipmentItem(EquipContainer[originIndex].slotType);
                            InformUI();
                            InformEquipmentUI();
                            break;
                        }
                        else
                        {
                            if (wasChanged)
                            {
                                Debug.Log("was changed!");
                                EquipContainer[originIndex] = new EquipmentItem(EquipContainer[originIndex].item, reminder, EquipContainer[originIndex].slotType);
                                InformUI();
                                InformEquipmentUI();
                                Debug.Log("NO MORE PLACE, Some Items are left there!");
                                break;
                            }
                            Debug.Log("NO MORE PLACE, Nothing Changed!");
                            break;
                        }
                    }
                }
            }
            else
            {
                if (SearchForEmptySlot(out int newEmptyIndex)) // Same as above.
                {
                    Container[newEmptyIndex] = new InventoryItem(EquipContainer[originIndex].item, EquipContainer[originIndex].quantity, Container[newEmptyIndex].slotType);
                    EquipContainer[originIndex] = new EquipmentItem(EquipContainer[originIndex].slotType);
                    InformUI();
                    InformEquipmentUI();
                }
                else
                {
                    Debug.Log("NO PLACE, Dont UnEquiped!");
                }
            }
        }
        #endregion

        #region HelperMethods
        private bool CanBeEquipped(string itemPossibleEquipSlot, InventoryItem.SlotType slotType) => itemPossibleEquipSlot == slotType.ToString();
        private bool CanBeEquipped(string itemPossibleEquipSlot, QuickSlotItem.SlotType slotType) => itemPossibleEquipSlot == slotType.ToString();
        private bool CanBeEquipped(string itemPossibleEquipSlot, EquipmentItem.SlotType slotType) => itemPossibleEquipSlot == slotType.ToString();
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
        private List<EquipmentItem.SlotType> ConvertListItemSlotsToEquipmentSlots(List<ItemSO.ItemSlots> itemSOList)
        {
            List<EquipmentItem.SlotType> newList = new List<EquipmentItem.SlotType>();
            foreach (ItemSO.ItemSlots slot in itemSOList)
            {
                if (Enum.TryParse(slot.ToString(), out EquipmentItem.SlotType result))
                {
                    newList.Add(result);
                }
            }
            return newList;
        }
        private int GetEquipmentSlotIndex(List<EquipmentItem.SlotType> possibleSlotTypes)
        {
            int slotCount = 0;
            List<int> indexes = new List<int>();
            int listIndex = 0;
            foreach (EquipmentItem item in EquipContainer)
                for (int i = 0; i < possibleSlotTypes.Count; i++)
                {
                    if (item.slotType == possibleSlotTypes[i])
                    {
                        slotCount++;
                        indexes.Add(EquipContainer.IndexOf(item));
                        listIndex++;
                    }
                }
            if (slotCount == 1)
                return indexes[0];
            else
            {
                if (EquipContainer[indexes[0]].IsEmpty)
                    return indexes[0];
                else if (EquipContainer[indexes[1]].IsEmpty)
                    return indexes[1];
                return indexes[0];
            }
        }

        #endregion



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
            for (int i = 0; i < Container.Count; i++)
            {
                if (Container[i].IsEmpty)
                    Container[i] = InventoryItem.GetEmptyItem();
            }
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
                if(EquipContainer[i].IsEmpty)
                    EquipContainer[i] = new EquipmentItem(EquipContainer[i].item, EquipContainer[i].quantity, (EquipmentItem.SlotType)i);
                else
                    EquipContainer[i] = new EquipmentItem(EquipContainer[i].item, EquipContainer[i].quantity, (EquipmentItem.SlotType)i, EquipContainer[i].itemRarity, EquipContainer[i].itemParameters);
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
                                EquipContainer[i] = new EquipmentItem(EquipContainer[i].item, 1, EquipContainer[i].slotType, EquipContainer[i].itemRarity,EquipContainer[i].itemParameters);
                                break;
                            }
                            else
                            if (EquipContainer[i].quantity > EquipContainer[i].item.MaxStackSize)
                            {
                                EquipContainer[i] = new EquipmentItem(EquipContainer[i].item, EquipContainer[i].item.MaxStackSize, EquipContainer[i].slotType, EquipContainer[i].itemRarity, EquipContainer[i].itemParameters);
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
        public void CorrectContainerSlotItemParameters()
        {
            for (int i = 0; i < Container.Count; i++)
            {
                if (!Container[i].IsEmpty)
                    if (AllStatParamsAreNull(Container[i]))
                    {
                        Container[i] = new InventoryItem(
                            Container[i].item,
                            Container[i].quantity,
                            Container[i].slotType,
                            Container[i].itemRarity,
                            Container[i].item.ItemParameters);
                    }
            }

            static bool AllStatParamsAreNull(InventoryItem item)
            {
                if (item.itemParameters.statModifiers.Count == 0
                    && item.itemParameters.weaponModifiers.Count == 0
                    && item.itemParameters.equipmentModifiers.Count == 0
                    && item.itemParameters.resistModifiers.Count == 0
                    && item.itemParameters.vulnerabilityModifiers.Count == 0)
                    return true;
                else return false;
            }
        }
        #endregion


        public InventoryItem GetItemAt(int index) => Container[index];
        public QuickSlotItem GetQuickSlotItemAt(int index) => QSContainer[index];
        public EquipmentItem GetEquipmentItemAt(int index) => EquipContainer[index];
        public InventoryItem GetLootItemAt(int index) => LootContainer[index];
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
    public struct ItemParameters
    {
        [NonReorderable] public List<ModifierType> statModifiers;
        [NonReorderable] public List<WeaponModifierType> weaponModifiers;
        [NonReorderable] public List<WeaponStatModifierType> weaponStatModifiers;
        [NonReorderable] public List<EquipmentModifierType> equipmentModifiers;
        [NonReorderable] public List<ResistModifierType> resistModifiers;
        [NonReorderable] public List<VulnerabilityModifierType> vulnerabilityModifiers;

        public struct StatsCount
        {
            public int posStatCount;
            public int negStatCount;
            public int equipCount;
            public int weaponCount;
            public int weaponStatCount;
            public int resistCount;
            public int vulnerabilityCount;
            public StatsCount(ItemRarities.ItemRaritiesEnum itemRarity)
            {
                switch(itemRarity)
                {
                    case ItemRarities.ItemRaritiesEnum.BAD:
                        {
                            posStatCount = 0;
                            negStatCount = 2;
                            equipCount = 1;
                            weaponCount = 1;
                            weaponStatCount = 0;
                            resistCount = 0;
                            vulnerabilityCount = 2;
                            break;
                        }
                    case ItemRarities.ItemRaritiesEnum.COMMON:
                        {
                            posStatCount = 1;
                            negStatCount = 1;
                            equipCount = 1;
                            weaponCount = 1;
                            weaponStatCount = 0;
                            resistCount = 1;
                            vulnerabilityCount = 1;
                            break;
                        }
                    case ItemRarities.ItemRaritiesEnum.UNCOMMON:
                        {
                            posStatCount = 1;
                            negStatCount = UnityEngine.Random.Range(0, 2);
                            equipCount = 1;
                            weaponCount = 1;
                            weaponStatCount = 0;
                            resistCount = 1;
                            vulnerabilityCount = UnityEngine.Random.Range(0, 2);
                            break;
                        }
                    case ItemRarities.ItemRaritiesEnum.RARE:
                        {
                            posStatCount = 1;
                            negStatCount = 0;
                            equipCount = 2;
                            weaponCount = 2;
                            weaponStatCount = 1;
                            resistCount = 1;
                            vulnerabilityCount = 0;
                            break;
                        }
                    case ItemRarities.ItemRaritiesEnum.EPIC:
                        {
                            posStatCount = 2;
                            negStatCount = UnityEngine.Random.Range(0, 2);
                            equipCount = 2;
                            weaponCount = 2;
                            weaponStatCount = 1;
                            resistCount = 2;
                            vulnerabilityCount = UnityEngine.Random.Range(0, 2);
                            break;
                        }
                    case ItemRarities.ItemRaritiesEnum.LEGENDARY:
                        {
                            posStatCount = 3;
                            negStatCount = UnityEngine.Random.Range(0, 3);
                            equipCount = 3;
                            weaponCount = 3;
                            weaponStatCount = 2;
                            resistCount = 3;
                            vulnerabilityCount = UnityEngine.Random.Range(0, 3);
                            break;
                        }
                    case ItemRarities.ItemRaritiesEnum.MITHYCAL:
                        {
                            posStatCount = 4;
                            negStatCount = UnityEngine.Random.Range(0, 4);
                            equipCount = 3;
                            weaponCount = 3;
                            weaponStatCount = 2;
                            resistCount = 4;
                            vulnerabilityCount = UnityEngine.Random.Range(0, 4);
                            break;
                        }
                    case ItemRarities.ItemRaritiesEnum.ETERNAL:
                        {
                            posStatCount = 5;
                            negStatCount = UnityEngine.Random.Range(0, 5);
                            equipCount = 4;
                            weaponCount = 4;
                            weaponStatCount = 3;
                            resistCount = 5;
                            vulnerabilityCount = UnityEngine.Random.Range(0, 5);
                            break;
                        }
                    default:
                        {
                            posStatCount = 0;
                            negStatCount = 0;
                            equipCount = 0;
                            weaponCount = 0;
                            weaponStatCount = 0;
                            resistCount = 0;
                            vulnerabilityCount = 0;
                            break;
                        }
                }
            }
        }
        private static ItemParameters Initialize()
        {
            ItemParameters parameters;
            parameters.statModifiers = new List<ModifierType>();
            parameters.weaponModifiers = new List<WeaponModifierType>();
            parameters.weaponStatModifiers = new List<WeaponStatModifierType>();
            parameters.equipmentModifiers = new List<EquipmentModifierType>();
            parameters.resistModifiers = new List<ResistModifierType>();
            parameters.vulnerabilityModifiers = new List<VulnerabilityModifierType>();
            return parameters;
        }
        public static ItemParameters GetRandomItemParameters(ItemSO item, int characterLevel, ItemRarities itemRarity)
        {
            StatsCount newStatsCount = new StatsCount(itemRarity.ItemRarity);

            switch (item.ItemType)
            {
                case ItemSO.ItemTypes.WEAPON:
                case ItemSO.ItemTypes.RANGED_WEAPON:
                    return GetWeaponParameters(newStatsCount, characterLevel);
                case ItemSO.ItemTypes.SHIELD:
                case ItemSO.ItemTypes.ARMOR:
                case ItemSO.ItemTypes.MEDALION:
                case ItemSO.ItemTypes.RING:
                case ItemSO.ItemTypes.HEAD_GEAR:
                case ItemSO.ItemTypes.BRACERS:
                case ItemSO.ItemTypes.BOOTS:
                    return GetEquipmentParameters(newStatsCount, characterLevel);
                case ItemSO.ItemTypes.RANGED_AMMO:
                    return GetAmmoParameters(newStatsCount, characterLevel);
                default:
                    return new ItemParameters();
            }
        }
        private static ItemParameters GetEquipmentParameters(StatsCount newStatsCount, int characterLevel)
        {
            ItemParameters parameters = Initialize();
            if (newStatsCount.equipCount != 0)
                for (int i = 0; i < newStatsCount.equipCount; i++)
                    parameters.equipmentModifiers.Add(GetEquipmentModifierType(characterLevel));
            if (newStatsCount.posStatCount != 0)
                for (int i = 0; i < newStatsCount.posStatCount; i++)
                    parameters.statModifiers.Add(GetPositiveModifierType(characterLevel));
            if (newStatsCount.negStatCount != 0)
                for (int i = 0; i < newStatsCount.negStatCount; i++)
                    parameters.statModifiers.Add(GetNegativeModifierType(characterLevel));
            if (newStatsCount.resistCount != 0)
                for (int i = 0; i < newStatsCount.resistCount; i++)
                    parameters.resistModifiers.Add(GetResistModifierType(characterLevel));
            if (newStatsCount.vulnerabilityCount != 0)
                for (int i = 0; i < newStatsCount.vulnerabilityCount; i++)
                    parameters.vulnerabilityModifiers.Add(GetVulnerabilityModifierType(characterLevel));
            return parameters;
        }
        private static ItemParameters GetWeaponParameters(StatsCount newStatsCount, int characterLevel)
        {
            ItemParameters parameters = Initialize();
            if (newStatsCount.weaponCount != 0)
                for (int i = 0; i < newStatsCount.weaponCount; i++)
                    parameters.weaponModifiers.Add(GetWeaponModifierType(characterLevel));
            if (newStatsCount.weaponStatCount != 0)
                for (int i = 0; i < newStatsCount.weaponStatCount; i++)
                    parameters.weaponStatModifiers.Add(GetWeaponStatModifiers(characterLevel));
            if (newStatsCount.posStatCount != 0)
                for (int i = 0; i < newStatsCount.posStatCount; i++)
                    parameters.statModifiers.Add(GetPositiveModifierType(characterLevel));
            if (newStatsCount.negStatCount != 0)
                for (int i = 0; i < newStatsCount.negStatCount; i++)
                    parameters.statModifiers.Add(GetNegativeModifierType(characterLevel));
            return parameters;
        }
        private static ItemParameters GetAmmoParameters(StatsCount newStatsCount, int characterLevel)
        {
            ItemParameters parameters = Initialize();
            if (newStatsCount.weaponCount != 0)
                for (int i = 0; i < newStatsCount.weaponCount; i++)
                    parameters.weaponModifiers.Add(GetWeaponModifierType(characterLevel));
            return parameters;
        }

        private static ModifierType GetNegativeModifierType(int characterLevel)
        {
            // 1 lvl - -1
            // 2 lvl - -1 - -2
            // 5 lvl - -1 - -5
            // 10 lvl - -1 - -10
            // 20 lvl - -1 - -10
            // 30 lvl - -1 - -10
            // 54 lvl - -1 - -10
            int index = UnityEngine.Random.Range(0, GameManager.Instance.ModifiersListSO.modifiersStatListSO.modifiersStatList.Count);
            int value = characterLevel <= 10 ? UnityEngine.Random.Range(1, characterLevel + 1) : UnityEngine.Random.Range(1, 11);
            return new ModifierType(GameManager.Instance.ModifiersListSO.modifiersStatListSO.modifiersStatList[index], -value);
        }
        private static ModifierType GetPositiveModifierType(int characterLevel)
        {
            // 1 lvl - +1
            // 2 lvl - +1 - +2
            // 5 lvl - +1 - +5
            // 10 lvl - +1 - +10
            // 20 lvl - +1 - +10
            // 30 lvl - +1 - +10
            // 54 lvl - +1 - +10
            int index = UnityEngine.Random.Range(0, GameManager.Instance.ModifiersListSO.modifiersStatListSO.modifiersStatList.Count);
            int value = characterLevel <= 10 ? UnityEngine.Random.Range(1, characterLevel + 1) : UnityEngine.Random.Range(1, 11);
            return new ModifierType(GameManager.Instance.ModifiersListSO.modifiersStatListSO.modifiersStatList[index], value);
        }
        private static WeaponStatModifierType GetWeaponStatModifiers(int characterLevel)
        {
            // 1 lvl - +2% - +22%
            // 2 lvl - +4% - +24%
            // 10 lvl - +20% - +40%
            // 20 lvl - +40% - +60%
            // 30 lvl - +60% - +80%
            // 54 lvl - +108% - +128%
            int index = UnityEngine.Random.Range(0, GameManager.Instance.ModifiersListSO.modifiersWeaponStatListSO.modifiersStatWeaponList.Count);
            int value = UnityEngine.Random.Range(characterLevel * 2, (characterLevel + 10) * 2);
            return new WeaponStatModifierType(GameManager.Instance.ModifiersListSO.modifiersWeaponStatListSO.modifiersStatWeaponList[index], value);
        }
        private static WeaponModifierType GetWeaponModifierType(int characterLevel)
        {
            // 1 lvl - 10 - 30 dmg
            // 2 lvl - 20 - 40 dmg
            // 10 lvl - 100 - 120 dmg
            // 20 lvl - 200 - 220 dmg
            // 30 lvl - 300 - 320 dmg
            // 54 lvl - 540 - 560 dmg
            int index = UnityEngine.Random.Range(0, GameManager.Instance.ModifiersListSO.modifiersWeaponListSO.modifiersWeaponList.Count);
            int value = UnityEngine.Random.Range(characterLevel * 10, (characterLevel + 2) * 10);
            Debug.Log("index and value are " + index + " " + value + " starting return");
            return new WeaponModifierType(GameManager.Instance.ModifiersListSO.modifiersWeaponListSO.modifiersWeaponList[index], value);
        }
        private static EquipmentModifierType GetEquipmentModifierType(int characterLevel)
        {
            // Character HP Scale: lvl*100
            // 1 lvl - 100hp
            // 2 lvl - 200hp
            // 10 lvl - 1000hp
            // 54 lvl - 5400hp

            // 1 lvl - 2hp
            // 2 lvl - 4hp
            // 10 lvl - 20hp
            // 20 lvl - 40hp
            // 54 lvl - 108hp

            //ARMOR AND MR
            // 1 lvl - +1
            // 2 lvl - +1-2
            // 10 lvl - +1-10
            // 54 lvl - +1-54
            // 54*8*4 = 1728AR & MR - 54% RESIST
            // 32p is 1% Resistance.
            int index = UnityEngine.Random.Range(0, GameManager.Instance.ModifiersListSO.modifiersEquipmentListSO.modifiersStatList.Count);
            int value;
            if (GameManager.Instance.ModifiersListSO.modifiersEquipmentListSO.modifiersStatList[index].modifierType == EquipmentModifierSO.ModifierType.HEALTH)
                value = UnityEngine.Random.Range(1, (characterLevel * 2) + 1);
            else
                value = UnityEngine.Random.Range(1, characterLevel + 1);
            return new EquipmentModifierType(GameManager.Instance.ModifiersListSO.modifiersEquipmentListSO.modifiersStatList[index], value);
        }
        private static ResistModifierType GetResistModifierType(int characterLevel)
        {
            // Resists
            // 1 lvl - 1
            // 2 lvl - 1 - 2
            // 10 lvl - 1 - 10
            // 54 lvl - 1 - 54
            // Max OF 8*5*54 = 2160p =  67.5%
            int index = UnityEngine.Random.Range(0, GameManager.Instance.ModifiersListSO.modifiersResistListSO.modifiersResistList.Count);
            int value = UnityEngine.Random.Range(1, characterLevel + 1);
            return new ResistModifierType(GameManager.Instance.ModifiersListSO.modifiersResistListSO.modifiersResistList[index], value);
        }
        private static VulnerabilityModifierType GetVulnerabilityModifierType(int characterLevel)
        {
            // Vulnerability 
            // 1 lvl - 1
            // 2 lvl - 1 - 2
            // 10 lvl - 1 - 10
            // 54 lvl - 1 - 54
            // Max OF 8*5*54 = 2160p =  67.5%
            int index = UnityEngine.Random.Range(0, GameManager.Instance.ModifiersListSO.modifiersVulnerabilityListSO.modifiersVulnerabilityList.Count);
            int value = UnityEngine.Random.Range(1, characterLevel + 1);
            return new VulnerabilityModifierType(GameManager.Instance.ModifiersListSO.modifiersVulnerabilityListSO.modifiersVulnerabilityList[index], -value);
        }
    }
    [Serializable]
    public struct ItemRarities
    {
        public enum ItemRaritiesEnum
        {
            DEFAULT,
            BAD,        // 15%  86-100  W1
            COMMON,     // 22%  64-85   W1
            UNCOMMON,   // 18%  46-63   W2
            RARE,       // 15%  31-45   W2
            EPIC,       // 12%  19-30   W3
            LEGENDARY,  // 9%   10-18   W3
            MITHYCAL,   // 6%   4-9     W4
            ETERNAL,    // 3%   1-3     W4
        }
        public ItemRaritiesEnum ItemRarity;

        public ItemRarities(ItemRaritiesEnum itemRarity)
        {
            this.ItemRarity = itemRarity;
        }

        public static ItemRaritiesEnum GetItemRarity()
        {
            int index = UnityEngine.Random.Range(1, 101);
            return index > 45 ? index <= 63
                                ? ItemRarities.ItemRaritiesEnum.UNCOMMON
                                : index > 85
                                    ? ItemRarities.ItemRaritiesEnum.BAD
                                    : ItemRarities.ItemRaritiesEnum.COMMON
                              : index <= 18
                                ? index <= 9
                                    ? index > 3
                                        ? ItemRarities.ItemRaritiesEnum.MITHYCAL
                                        : ItemRarities.ItemRaritiesEnum.ETERNAL
                                    : ItemRarities.ItemRaritiesEnum.LEGENDARY
                                : index > 30
                                    ? ItemRarities.ItemRaritiesEnum.RARE
                                    : ItemRarities.ItemRaritiesEnum.EPIC;
        }
    }
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
        public ItemRarities itemRarity;
        public ItemContainer itemContainer;
        public ItemParameters itemParameters;


        public InventoryItem(ItemSO item, int quantity, SlotType slotType)
        {
            this.item = item;
            this.quantity = quantity;
            this.slotType = slotType;
            this.itemContainer = ItemContainer.Container;
            this.itemRarity = new ItemRarities(ItemRarities.GetItemRarity());
            this.itemParameters = ItemParameters.GetRandomItemParameters(item, 20, itemRarity);
        }
        public InventoryItem(ItemSO item, int quantity, SlotType slotType, ItemRarities itemRarity, ItemParameters itemParameters)
        {
            this.item = item;
            this.quantity = quantity;
            this.slotType = slotType;
            this.itemContainer = ItemContainer.Container;
            this.itemRarity = itemRarity;
            this.itemParameters = itemParameters;
        }

        public static InventoryItem GetEmptyItem()
        {
            return new InventoryItem
            {
                item = null,
                quantity = 0,
                slotType = SlotType.MAIN_SLOT,
                itemContainer = ItemContainer.Container,
                itemRarity =  new ItemRarities(ItemRarities.ItemRaritiesEnum.DEFAULT),
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
        public enum ItemRarities
        {
            DEFAULT,
            BAD,        // 15%  86-100  W1
            COMMON,     // 22%  64-85   W1
            UNCOMMON,   // 18%  46-63   W2
            RARE,       // 15%  31-45   W2
            EPIC,       // 12%  19-30   W3
            LEGENDARY,  // 9%   10-18   W3
            MITHYCAL,   // 6%   4-9     W4
            ETERNAL,    // 3%   1-3     W4
        }
        public enum ItemContainer
        {
            QSContainer,
        }
        public ItemSO item;
        public int quantity;
        public SlotType slotType;
        public ItemRarities itemRarity;
        public ItemContainer itemContainer;
        public ItemParameters itemParameters;

        public QuickSlotItem(ItemSO item, int quantity, SlotType slotType)
        {
            this.item = item;
            this.quantity = quantity;
            this.slotType = slotType;
            this.itemContainer = ItemContainer.QSContainer;
            this.itemRarity = ItemRarities.DEFAULT;
            this.itemParameters = new ItemParameters();
        }
        public static QuickSlotItem GetEmptyQuickSlotItem()
        {
            return new QuickSlotItem
            {
                item = null,
                quantity = 0,
                slotType = SlotType.QUICK_SLOT,
                itemContainer = ItemContainer.QSContainer,
                itemRarity = ItemRarities.DEFAULT,
                itemParameters = new ItemParameters(),
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
        public ItemRarities itemRarity;
        public ItemContainer itemContainer;
        public ItemParameters itemParameters;

        public EquipmentItem(ItemSO item, int quantity, SlotType slotType)
        {
            this.item = item;
            this.quantity = quantity;
            this.slotType = slotType;
            this.itemContainer = ItemContainer.EquipmentContainer;
            this.itemRarity = new ItemRarities(ItemRarities.ItemRaritiesEnum.DEFAULT);
            this.itemParameters = new ItemParameters();
        }
        public EquipmentItem(SlotType type)
        {
            item = null;
            quantity = 0;
            slotType = type;
            itemContainer = ItemContainer.EquipmentContainer;
            itemRarity = new ItemRarities(ItemRarities.ItemRaritiesEnum.DEFAULT);
            itemParameters = new ItemParameters();
        }
        public EquipmentItem(ItemSO item, int quantity, SlotType slotType, ItemRarities itemRarity, ItemParameters itemParameters)
        {
            this.item = item;
            this.quantity = quantity;
            this.slotType = slotType;
            this.itemContainer = ItemContainer.EquipmentContainer;
            this.itemRarity = itemRarity;
            this.itemParameters = itemParameters;
        }
        public static EquipmentItem GetEmptyEquipmentItem()
        {
            return new EquipmentItem
            {
                item = null,
                quantity = 0,
                slotType = SlotType.DEFAULT,
                itemContainer = ItemContainer.EquipmentContainer,
                itemRarity = new ItemRarities(ItemRarities.ItemRaritiesEnum.DEFAULT),
                itemParameters = new ItemParameters(),
        };
        }
    }
}
#endregion