using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Actor.SO
{
    [CreateAssetMenu(fileName = "New Actor", menuName = "Base/New Actor")]

    public class ActorSO : ScriptableObject
    {
        [field: SerializeField] public float Health;

        [SerializeField] Endurance endurance = new Endurance(1);

        [SerializeField] public List<Perks> perksList = new List<Perks>();
        [SerializeField] public PerkManagerSO perkManager;
        public void Test()
        {
            perksList[0].UsePerk();
        }
        public void AddPerk(Perks perk)
        {
            perksList.Add(perk);
        }

        internal void SetBaseEndurance(int v)
        {
            Endurance.SetBaseValue(v);
        }

        internal void ChangeEndurance(int v)
        {
            throw new NotImplementedException();
        }
    }

    [Serializable]
    public class Endurance
    {
        public int Level;

        public Endurance(int level)
        {
            this.Level = level;
        }

        internal static void SetBaseValue(int v)
        {
            throw new NotImplementedException();
        }
        public float HealthBonus => Level switch
        {
            1 => 100,
            2 => 200,
            _ => 0,
        };
    }

}