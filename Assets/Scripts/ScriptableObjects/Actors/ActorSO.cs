using Inventory.SO;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Actor.SO
{
    [CreateAssetMenu(fileName = "New Actor", menuName = "Base/New Actor")]

    public class ActorSO : ScriptableObject
    {
        public event Action
            OnStatUpdate;

        [field: SerializeField] public float BaseHealth { get; private set; }
        [field: SerializeField] public float TotalHealth { get; private set; }
        [field: SerializeField] public float CurrentHealth { get; private set; }


        private void SetTotalHealth(float value)
        {

        }


        [Header("Base Stats")]
        [SerializeField] Strength strength = new Strength(1);
        [SerializeField] Dexterity dexterity = new Dexterity(1);
        [SerializeField] Constitution constitution = new Constitution(1);
        [SerializeField] Intelligence intelligence = new Intelligence(1);
        [SerializeField] Wisdom wisdom = new Wisdom(1);
        [SerializeField] Charisma charisma = new Charisma(1);

        [Header("Monster Settings")]
        [field: SerializeField] public float JumpStrength;
        [field: SerializeField] public float JumpDelay;





        [SerializeField] public List<Perks> perksList = new List<Perks>();
        [SerializeField] public PerkManagerSO perkManager;


        public void InitializeSO()
        {
            TotalHealth = BaseHealth + constitution.GetHealthBonus;
            CurrentHealth = TotalHealth;
        }





        public void GetHit(EquipmentItem hitWeapon)
        {
            CurrentHealth -= hitWeapon.item.BaseDamage;
        }






        private void UpdateStatUI()
        {
            OnStatUpdate?.Invoke();
        }
        #region BaseStatsSetters
        public void SetBaseHealth(float value)
        {
            BaseHealth += value;
            UpdateStatUI();
        }
        #endregion
        #region BaseStatsGetters
        public float GetCharacterBaseHealth()
        {
            return BaseHealth + constitution.GetHealthBonus;
        }
        #endregion




        public void Test()
        {
            perksList[0].UsePerk();
        }
        public void AddPerk(Perks perk)
        {
            perksList.Add(perk);
        }

        public void SetBaseEndurance(int v)
        {
            Strength.SetBaseValue(v);
        }

        public void ChangeEndurance(int v)
        {
            throw new NotImplementedException();
        }
    }

    [Serializable]
    public struct Strength
    {
        [SerializeField] private int level;

        public Strength(int level)
        {
            this.level = level;
        }
        public static void SetBaseValue(int v)
        {
            throw new NotImplementedException();
        }

    }
    [Serializable]
    public struct Dexterity
    {
        [SerializeField] private int level;
        public Dexterity(int level)
        {
            this.level = level;
        }
    }
    [Serializable]
    public struct Constitution
    {
        [SerializeField] private int level;
        public Constitution(int level)
        {
            this.level = level;
        }
        public float GetHealthBonus => level switch
        {
            1 => 200,
            2 => 400,
            3 => 600,
            4 => 800,
            _ => 0,
        };
    }
    [Serializable]
    public struct Intelligence
    {
        [SerializeField] private int level;
        public Intelligence(int level)
        {
            this.level = level;
        }
    }
    [Serializable]
    public struct Wisdom
    {
        [SerializeField] private int level;
        public Wisdom(int level)
        {
            this.level = level;
        }
    }
    [Serializable]
    public struct Charisma
    {
        [SerializeField] private int level;
        public Charisma(int level)
        {
            this.level = level;
        }
    }
}