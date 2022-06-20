using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Inventory.UI
{
    public class UIMouseFollower : MonoBehaviour
    {
        [SerializeField] private Image itemImage;
        [SerializeField] private TMP_Text quantity;
        public enum EquipSlots
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
            _12_QUICK_SLOT
        }
        public EquipSlots FollowerSlotType = EquipSlots._0_DEFAULT;

        public List<EquipSlots> slotsToEquip = new List<EquipSlots>();

        public string FollowerType { get; set; }

        public int ItemIndex { get; set; }
        public bool IsActive() => isActiveAndEnabled == true;
        public void ToggleMouseFollower(bool value)
        {
            slotsToEquip.Clear();
            gameObject.SetActive(value);
        }

        public void SetUpData(Sprite sprite, int quantity)
        {
            itemImage.sprite = sprite;
            this.quantity.text = quantity.ToString();
        }
        private void Update()
        {
            gameObject.transform.position = Input.mousePosition;
        }
    }
}