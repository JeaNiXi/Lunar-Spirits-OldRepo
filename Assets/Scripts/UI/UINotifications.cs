using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Inventory.UI
{
    public class UINotifications : MonoBehaviour
    {
        [SerializeField] private UINotificationText notificationPrefab;
        [SerializeField] private RectTransform notificationPanel;
        public enum Notifications
        {
            WRONG_ITEM_TYPE,
            NO_INVENTORY_SPACE_LEFT,
            SAME_ITEM_IN_SLOT,
            CAN_INTERACT_WITH_OBJECT,
            SAVE_SLOT_EMPTY,
            CHEST_NEARBY,
            ITEM_ADDED,
        }
        private readonly string[] NotificationsStrings =
        {
            "Wrong Item Type. Item can't be equiped here!",
            "No Space In Inventory!",
            "Same Item Already in Slot!",
            "This is an object I can interact with!",
            "This save slot is empty!",
            "I sense something nearby!",
            "Item Added: "
        };
        public Notifications Notification = Notifications.WRONG_ITEM_TYPE;
        public void ThrowNotification(Notifications notificationType)
        {
            UINotificationText notification = CreateNotification();
            notification.transform.SetParent(notificationPanel);
            notification.SetText(NotificationsStrings[Convert.ToInt32(notificationType)]);
            notification.StartDisplay();
        }
        public void ThrowNotification(Notifications notificationType, string itemName)
        {
            UINotificationText notification = CreateNotification();
            notification.transform.SetParent(notificationPanel);
            notification.SetText(NotificationsStrings[Convert.ToInt32(notificationType)] + itemName);
            notification.StartDisplay();
        }

        private UINotificationText CreateNotification() => Instantiate(notificationPrefab, Vector3.zero, Quaternion.identity);
    }
}