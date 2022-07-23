using Inventory.SO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Helpers.SO
{
    [CreateAssetMenu(fileName = "ModifiersListBase", menuName = "Helpers/MainModifiersListBase")]

    public class ModifiersListBaseChildSO : ScriptableObject
    {
        [NonReorderable] public List<ModifiersSO> modifiersStatList = new List<ModifiersSO>();
    }
}