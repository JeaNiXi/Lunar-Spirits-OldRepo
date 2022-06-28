using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Actor.SO
{
    [CreateAssetMenu(fileName = "Perk Manager", menuName = "Base/Perk Manager")]

    public class PerkManagerSO : ScriptableObject
    {
        [SerializeField] public Perks FastRunner;

        public Perks Agility_FastRunner()
        {
            return FastRunner;
        }
    }
}