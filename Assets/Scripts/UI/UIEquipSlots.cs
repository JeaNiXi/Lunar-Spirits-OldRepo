using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIEquipSlots : MonoBehaviour, IDropHandler
{
    public EquipSlotsSO slotType;
    [SerializeField] private Image sprite;
    [SerializeField] private TMP_Text quantity;
    [SerializeField] private GameObject quantityObject;

    public event Action<UIEquipSlots> OnItemDroppedOnSlot;

    public void OnDrop(PointerEventData eventData)
    {
        OnItemDroppedOnSlot?.Invoke(this);
    }
}
