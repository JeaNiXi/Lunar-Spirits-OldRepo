using Inventory.SO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Helpers.SO
{
    [CreateAssetMenu(fileName = "ModifiersListWeapon", menuName = "Helpers/MainModifiersListWeapon")]
    public class ModifiersListWeaponChildSO : ScriptableObject
    {
        [NonReorderable] public List<WeaponModifiersSO> modifiersWeaponList = new List<WeaponModifiersSO>();
    }
}