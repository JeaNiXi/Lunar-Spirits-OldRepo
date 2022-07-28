using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Helpers.SO
{
    [CreateAssetMenu(fileName = "ModifiersListWeaponStats", menuName = "Helpers/MainModifiersListWeaponStats")]
    public class ModifiersListWeaponStatChildSO : ScriptableObject
    {
        [NonReorderable] public List<WeaponStatModifiersSO> modifiersStatWeaponList = new List<WeaponStatModifiersSO>();
    }
}