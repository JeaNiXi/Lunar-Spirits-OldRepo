using Helpers.SO;
using Inventory.UI;
using Managers;
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
        public event Action<UINotifications.Notifications>
            ThrowNotification;
        public event Action<EquipmentItem>
            OnItemEquipped,
            OnItemUnequipped;
        [SerializeField] [NonReorderable] private List<InventoryItem> Container = new List<InventoryItem>(24);
        [SerializeField] [NonReorderable] private List<QuickSlotItem> QSContainer = new List<QuickSlotItem>(2);
        [SerializeField] [NonReorderable] private List<EquipmentItem> EquipContainer = new List<EquipmentItem>(11);
        [SerializeField] [NonReorderable] private List<InventoryItem> LootContainer = new List<InventoryItem>();

        private int Rows { get => Container.Count / 6; }
        private const int MAX_ITEM_SLOTS = 42;
        private const int MIN_ITEM_SLOTS = 24;
        private const int MAX_EQUIPMENT_SLOTS = 11;
        private const int QUICK_SLOT_COUNT = 2;

        public int GetInventorySize() => Container.Count;
        public List<InventoryItem> GetItemList() => Container;
        public List<QuickSlotItem> GetQuickSlotList() => QSContainer;
        public List<EquipmentItem> GetEquipmentItemsList() => EquipContainer;
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
                        Container[destIndex] = new InventoryItem(Container[originIndex].item, Container[originIndex].quantity, Container[originIndex].slotType, Container[originIndex].itemRarity, Container[originIndex].baseModifiers, Container[originIndex].weaponModifiers);
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
                        EquipContainer[destIndex] = new EquipmentItem(Container[originIndex].item, Container[originIndex].quantity, EquipContainer[destIndex].slotType);
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
        public enum ItemRarities
        {
            DEFAULT,
            BAD,        // 15%  86-100
            COMMON,     // 22%  64-85
            UNCOMMON,   // 18%  46-63
            RARE,       // 15%  31-45
            EPIC,       // 12%  19-30
            LEGENDARY,  // 9%   10-18
            MITHYCAL,   // 6%   4-9
            ETERNAL,    // 3%   1-3
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

        [NonReorderable] public List<ModifierType> baseModifiers;
        [NonReorderable] public List<WeaponModifierType> weaponModifiers;

        public InventoryItem(ItemSO item, int quantity, SlotType slotType)
        {
            this.item = item;
            this.quantity = quantity;
            this.slotType = slotType;
            this.itemContainer = ItemContainer.Container;
            this.itemRarity = GetItemRarity();
            this.baseModifiers = GetBaseModifiersList(item, this.itemRarity);
            this.weaponModifiers = GetWeaponModifiersList(item, this.itemRarity);
        }
        public InventoryItem(ItemSO item, int quantity, SlotType slotType, ItemRarities itemRarity, List<ModifierType> baseModifiers, List<WeaponModifierType> weaponModifiers)
        {
            this.item = item;
            this.quantity = quantity;
            this.slotType = slotType;
            this.itemContainer = ItemContainer.Container;
            this.itemRarity = itemRarity;
            this.baseModifiers = baseModifiers;
            this.weaponModifiers = weaponModifiers;
        }
        public static List<ModifierType> GetBaseModifiersList(ItemSO itemSO, ItemRarities itemRarity)
        {
            if (itemSO.ItemType == ItemSO.ItemTypes.SHIELD)
            {
                EquipmentSO equipmentSO = itemSO as EquipmentSO;
                if (equipmentSO.modifierTypes.Count == 0)
                    return GameManager.Instance.ModifiersListSO.GenerateStatModifiersList(4, itemRarity, true);
                else
                    return equipmentSO.modifierTypes;
            }
            else if (itemSO.ItemType != ItemSO.ItemTypes.POTION
               && itemSO.ItemType != ItemSO.ItemTypes.INGREDIENT
               && itemSO.ItemType != ItemSO.ItemTypes.RANGED_AMMO
               && itemSO.ItemType != ItemSO.ItemTypes.QUEST_ITEM
               && itemSO.ItemType != ItemSO.ItemTypes.MISC)
            {
                if (itemSO.ItemType == ItemSO.ItemTypes.WEAPON || itemSO.ItemType == ItemSO.ItemTypes.RANGED_WEAPON)
                {
                    WeaponSO weaponSO = itemSO as WeaponSO;
                    if (weaponSO.modifierTypes.Count == 0)
                        return GameManager.Instance.ModifiersListSO.GenerateStatModifiersList(4, itemRarity, false);
                    else
                        return weaponSO.modifierTypes;
                }
                else
                {
                    EquipmentSO equipmentSO = itemSO as EquipmentSO;
                    if (equipmentSO.modifierTypes.Count == 0)
                        return GameManager.Instance.ModifiersListSO.GenerateStatModifiersList(4, itemRarity, false);
                    else
                        return equipmentSO.modifierTypes;
                }
            }
            return new List<ModifierType>();
        }
        public static List<WeaponModifierType> GetWeaponModifiersList(ItemSO itemSO, ItemRarities itemRarity)
        {
            if (itemSO.ItemType == ItemSO.ItemTypes.WEAPON || itemSO.ItemType == ItemSO.ItemTypes.RANGED_WEAPON)
            {
                WeaponSO weaponSO = itemSO as WeaponSO;
                if (weaponSO.weaponModifierTypes.Count == 0)
                    return GameManager.Instance.ModifiersListSO.GenerateWeaponModifiersList(4, itemRarity, false);
                return weaponSO.weaponModifierTypes;
            }
            else if (itemSO.ItemType == ItemSO.ItemTypes.RANGED_AMMO)
            {
                WeaponSO weaponSO = itemSO as WeaponSO;
                if (weaponSO.weaponModifierTypes.Count == 0)
                    return GameManager.Instance.ModifiersListSO.GenerateWeaponModifiersList(4, itemRarity, true);
                return weaponSO.weaponModifierTypes;
            }
            return new List<WeaponModifierType>();
        }
        public static ItemRarities GetItemRarity()
        {
            int index = UnityEngine.Random.Range(1, 101);
            return index > 45 ? index <= 63
                                ? ItemRarities.UNCOMMON
                                : index > 85
                                    ? ItemRarities.BAD
                                    : ItemRarities.COMMON
                              : index <= 18
                                ? index <= 9
                                    ? index > 3
                                        ? ItemRarities.MITHYCAL
                                        : ItemRarities.ETERNAL
                                    : ItemRarities.LEGENDARY
                                : index > 30
                                    ? ItemRarities.RARE
                                    : ItemRarities.EPIC;
        }
        public static InventoryItem GetEmptyItem()
        {
            return new InventoryItem
            {
                item = null,
                quantity = 0,
                slotType = SlotType.MAIN_SLOT,
                itemContainer = ItemContainer.Container,
                itemRarity = ItemRarities.DEFAULT,
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