using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class UINotifications : MonoBehaviour
{
    [SerializeField] private UINotificationText notificationPrefab;
    [SerializeField] private RectTransform notificationPanel;
    public enum Notifications
    {
        WRONG_ITEM_TYPE,
    }
    public string[] NotificationsStrings =
    {
        "Wrong Item Type. Item can't be equiped here!",
    };
    public Notifications Notification = Notifications.WRONG_ITEM_TYPE;
    public void ThrowNotification(Notifications notificationType)
    {
        UINotificationText notification = CreateNotification();
        notification.transform.SetParent(notificationPanel);
        notification.SetText(NotificationsStrings[Convert.ToInt32(notificationType)]);
        notification.StartDisplay();
    }

    private UINotificationText CreateNotification() => Instantiate(notificationPrefab, Vector3.zero, Quaternion.identity); 
}
