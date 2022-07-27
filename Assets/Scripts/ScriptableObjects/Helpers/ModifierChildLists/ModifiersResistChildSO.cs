using Inventory.SO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Helpers.SO
{
    [CreateAssetMenu(fileName = "ModifiersListResist", menuName = "Helpers/MainModifiersListResist")]
    public class ModifiersResistChildSO : ScriptableObject
    {
        [NonReorderable] public List<ResistModifierSO> modifiersWeaponList = new List<ResistModifierSO>();
    }
}