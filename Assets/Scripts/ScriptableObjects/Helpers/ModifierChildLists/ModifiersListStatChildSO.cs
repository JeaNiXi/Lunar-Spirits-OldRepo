using Inventory.SO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Helpers.SO
{
    [CreateAssetMenu(fileName = "ModifiersListStat", menuName = "Helpers/MainModifiersListStat")]
    public class ModifiersListStatChildSO : ScriptableObject
    {
        [NonReorderable] public List<ModifiersSO> modifiersStatList = new List<ModifiersSO>();
    }
}