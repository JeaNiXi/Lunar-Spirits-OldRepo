using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Equip Slot", menuName = "Basic/Equip Slot")]
public class EquipSlotsSO : ScriptableObject
{
    public enum EQUIP_SLOT_TYPE
    {
        HEAD,
        MEDALION,
        RING_1,
        RING_2,
        ARMOR,
        BRACERS,
        BOOTS,
        WEAPON_1,
        WEAPON_2,
        RANGE_WEAPON,
        RANGE_WEAPON_AMMO,
        QUICK_SLOT
    }
    [field: SerializeField] public EQUIP_SLOT_TYPE SlotType { get; set; }
}
