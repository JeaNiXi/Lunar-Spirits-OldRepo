using Inventory.SO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Inventory.SO.InventoryItem;
using static Inventory.SO.ItemSO;

namespace Helpers.SO
{
    [CreateAssetMenu(fileName = "ModifiersList", menuName = "Helpers/MainModifiersList")]

    public class ModifiersListSO : ScriptableObject
    {
        public ModifiersListStatChildSO modifiersStatListSO;
        public ModifiersListWeaponChildSO modifiersWeaponListSO;
        public ModifiersListWeaponStatChildSO modifiersWeaponStatListSO;
        public ModifiersListEquipmentChildSO modifiersEquipmentListSO;
        public ModifiersResistChildSO modifiersResistListSO;
        public ModifiersVulnerabilityChildSO modifiersVulnerabilityListSO;

    }
}