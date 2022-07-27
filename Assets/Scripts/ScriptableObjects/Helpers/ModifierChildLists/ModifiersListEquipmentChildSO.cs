using Inventory.SO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Helpers.SO
{
    [CreateAssetMenu(fileName = "ModifiersListEquipment", menuName = "Helpers/MainModifiersListEqipment")]
    public class ModifiersListEquipmentChildSO : ScriptableObject
    {
        [NonReorderable] public List<EquipmentModifierSO> modifiersStatList = new List<EquipmentModifierSO>();
    }
}