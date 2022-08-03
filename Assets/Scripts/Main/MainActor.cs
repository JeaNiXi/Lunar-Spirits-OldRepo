using Inventory.SO;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Actor
{
    [Serializable]
    public struct MainActor
    {
        [field: SerializeField]
        public string Name { get; set; }
        [field: SerializeField] public bool IsLevelDependant { get; set; }
        [field: SerializeField] public int Level { get; set; }

        [field: Space]
        [field: Header("Base Settings")]
        [field: SerializeField] public float BaseHealth { get; set; }
        [field: SerializeField] public float Armor { get; set; }
        [field: SerializeField] public float MagicResist { get; set; }

        [field: Space]
        [field: Header("Base Settings Helpers")]
        [field: SerializeField] public float HealthBonus { get; set; }
        [field: SerializeField] public float TotalHealth { get; set; }
        [field: SerializeField] public float CurrentHealth { get; set; }

        [field: Space]
        [field: Header("Stats")]
        [field: SerializeField] public Strength MainStrength { get; set; }
        [field: SerializeField] public Dexterity MainDexterity { get; set; }
        [field: SerializeField] public Constitution MainConstitution { get; set; }
        [field: SerializeField] public Intelligence MainIntelligence { get; set; }
        [field: SerializeField] public Wisdom MainWisdom { get; set; }
        [field: SerializeField] public Charisma MainCharisma { get; set; }

        [field: Space]
        [field: Header("Resistances")]
        [field: SerializeField] public int FireResistance { get; set; }
        [field: SerializeField] public int WaterResistance { get; set; }
        [field: SerializeField] public int EarthResistance { get; set; }
        [field: SerializeField] public int AirResistance { get; set; }
        [field: SerializeField] public int PoisonResistance { get; set; }
        [field: SerializeField] public int LightningResistance { get; set; }
        [field: SerializeField] public int PhysicalResistance { get; set; }
        [field: SerializeField] public int LightResistance { get; set; }
        [field: SerializeField] public int DarkResistance { get; set; }

        [field: Space]
        [field: Header("Vulnerabilities")]
        [field: SerializeField] public int FireVulnerability { get; set; }
        [field: SerializeField] public int WaterVulnerability { get; set; }
        [field: SerializeField] public int EarthVulnerability { get; set; }
        [field: SerializeField] public int AirVulnerability { get; set; }
        [field: SerializeField] public int PoisonVulnerability { get; set; }
        [field: SerializeField] public int LightningVulnerability { get; set; }
        [field: SerializeField] public int PhysicalVulnerability { get; set; }
        [field: SerializeField] public int LightVulnerability { get; set; }
        [field: SerializeField] public int DarkVulnerability { get; set; }

        #region Stats
        [Serializable]
        public struct Strength
        {
            [field: SerializeField] public int Level { get; private set; }

            public Strength(int level)
            {
                this.Level = level;
            }
            public float ScaleBonus => Level * 0.1f;
        }
        [Serializable]
        public struct Dexterity
        {
            [field: SerializeField] public int Level { get; private set; }
            public Dexterity(int level)
            {
                this.Level = level;
            }
        }
        [Serializable]
        public struct Constitution
        {
            [field: SerializeField] public int Level { get; private set; }
            public Constitution(int level)
            {
                this.Level = level;
            }
            public float GetHealthBonus => Level switch
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
            [field: SerializeField] public int Level { get; private set; }
            public Intelligence(int level)
            {
                this.Level = level;
            }
        }
        [Serializable]
        public struct Wisdom
        {
            [field: SerializeField] public int Level { get; private set; }
            public Wisdom(int level)
            {
                this.Level = level;
            }
        }
        [Serializable]
        public struct Charisma
        {
            [field: SerializeField] public int Level { get; private set; }
            public Charisma(int level)
            {
                this.Level = level;
            }
        }
        #endregion
        #region Base Methods
        public void InitActor()
        {
            CurrentHealth = BaseHealth;
            TotalHealth = BaseHealth + GetHealthBonus();
        }
        private int GetHealthBonus()
        {

            return 0;
        }
        #endregion






        [field: Space]
        [field: Header("Monster Settings")]
        [field: SerializeField] public float JumpStrength { get; set; }
        [field: SerializeField] public float JumpDelay { get; set; }


        public void AddHealthBonus(float value) // Used to collect all the bonuses to Health.
        {
            HealthBonus += value;
        }
        public void GetBaseHit(float value)
        {
            CurrentHealth -= value;
        }
        public void ChangeCurrentHealth(float value) // Used to Apply instant Health change. (Potions)
        {
            CurrentHealth += value;
        }

        public void TESTSETSETEST()
        {

        }




        //    [field: SerializeField] public float BaseHealth { get; private set; }
        //    [field: SerializeField] public float TotalHealth { get; private set; }
        //    [field: SerializeField] public float CurrentHealth { get; private set; }


        //    private void SetTotalHealth(float value)
        //    {

        //    }


        //    //[Header("Base Stats")]
        //    //[SerializeField] Strength strength = new Strength(1);
        //    //[SerializeField] Dexterity dexterity = new Dexterity(1);
        //    //[SerializeField] Constitution constitution = new Constitution(1);
        //    //[SerializeField] Intelligence intelligence = new Intelligence(1);
        //    //[SerializeField] Wisdom wisdom = new Wisdom(1);
        //    //[SerializeField] Charisma charisma = new Charisma(1);

        //    [Header("Monster Settings")]
            //[field: SerializeField] public float JumpStrength;
            //[field: SerializeField] public float JumpDelay;





        //    [SerializeField] public List<Perks> perksList = new List<Perks>();
        //    [SerializeField] public PerkManagerSO perkManager;


        //    public void InitializeSO()
        //    {
        //        //TotalHealth = BaseHealth + constitution.GetHealthBonus;
        //        //CurrentHealth = TotalHealth;
        //    }





        //    public void GetHit(EquipmentItem hitWeapon)
        //    {
        //        CurrentHealth -= hitWeapon.item.BaseDamage;
        //    }






        //    //private void UpdateStatUI()
        //    //{
        //    //    OnStatUpdate?.Invoke();
        //    //}
        //    //#region BaseStatsSetters
        //    //public void SetBaseHealth(float value)
        //    //{
        //    //    BaseHealth += value;
        //    //    UpdateStatUI();
        //    //}
        //    //#endregion
        //    //#region BaseStatsGetters
        //    //public float GetCharacterBaseHealth()
        //    //{
        //    //    return 0; // BaseHealth + constitution.GetHealthBonus;
        //    //}
        //    //#endregion




        //    public void Test()
        //    {
        //        perksList[0].UsePerk();
        //    }
        //    public void AddPerk(Perks perk)
        //    {
        //        perksList.Add(perk);
        //    }

        //    public void SetBaseEndurance(int v)
        //    {
        //        Strength.SetBaseValue(v);
        //    }

        //    public void ChangeEndurance(int v)
        //    {
        //        throw new NotImplementedException();
        //    }
        //}



    }
}