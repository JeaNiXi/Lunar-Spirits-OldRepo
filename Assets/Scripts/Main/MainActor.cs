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
        //[field: SerializeField] public float BaseHealth { get; set; }
        [field: SerializeField] public int Armor { get; set; }
        [field: SerializeField] public int MagicResist { get; set; }

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
            public float GetHealthBonusValue => Level * 10;
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
        public void InitActor(List<InventoryItem> itemsList)
        {
            HealthBonus = GetHealthLevelBonus() + GetHealthEquipmentBonus(itemsList);
            TotalHealth = HealthBonus + (HealthBonus/100 * MainConstitution.GetHealthBonusValue);
            CurrentHealth = TotalHealth;

            Armor = GetArmorEquipmentBonus(itemsList);
            MagicResist = GetMagicResEquipmentBonus(itemsList);

            FireResistance = GetResistanceModifier(itemsList, ResistModifierSO.ModifierType.FIRE_RES);
            FireVulnerability = GetVulnerabilityModifier(itemsList, VulnerabilityModifierSO.ModifierType.FIRE_VUL);
            WaterResistance = GetResistanceModifier(itemsList, ResistModifierSO.ModifierType.WATER_RES);
            WaterVulnerability = GetVulnerabilityModifier(itemsList, VulnerabilityModifierSO.ModifierType.WATER_VUL);
            EarthResistance = GetResistanceModifier(itemsList, ResistModifierSO.ModifierType.EARTH_RES);
            EarthVulnerability = GetVulnerabilityModifier(itemsList, VulnerabilityModifierSO.ModifierType.EARTH_VUL);
            AirResistance = GetResistanceModifier(itemsList, ResistModifierSO.ModifierType.AIR_RES);
            AirVulnerability = GetVulnerabilityModifier(itemsList, VulnerabilityModifierSO.ModifierType.AIR_VUL);
            PoisonResistance = GetResistanceModifier(itemsList, ResistModifierSO.ModifierType.POISON_RES);
            PoisonVulnerability = GetVulnerabilityModifier(itemsList, VulnerabilityModifierSO.ModifierType.POISON_VUL);
            LightningResistance = GetResistanceModifier(itemsList, ResistModifierSO.ModifierType.LIGHTNING_RES);
            LightningVulnerability = GetVulnerabilityModifier(itemsList, VulnerabilityModifierSO.ModifierType.LIGHTNING_VUL);
            PhysicalResistance = GetResistanceModifier(itemsList, ResistModifierSO.ModifierType.PHYSICAL_RES);
            PhysicalVulnerability = GetVulnerabilityModifier(itemsList, VulnerabilityModifierSO.ModifierType.PHYSICAL_VUL);
            LightResistance = GetResistanceModifier(itemsList, ResistModifierSO.ModifierType.LIGHT_RES);
            LightVulnerability = GetVulnerabilityModifier(itemsList, VulnerabilityModifierSO.ModifierType.LIGHT_VUL);
            DarkResistance = GetResistanceModifier(itemsList, ResistModifierSO.ModifierType.DARK_RES);
            DarkVulnerability = GetVulnerabilityModifier(itemsList, VulnerabilityModifierSO.ModifierType.DARK_VUL);
        }
        public void InitActor()
        {

        }
        private int GetHealthEquipmentBonus(List<InventoryItem> itemsList)
        {
            int tmpHealth = 0;
            if (itemsList.Count != 0) 
                foreach(InventoryItem item in itemsList)
                {
                    if (item.IsEmpty)
                        continue;
                    if (item.itemParameters.equipmentModifiers.Count != 0)
                    {
                        for (int i = 0; i < item.itemParameters.equipmentModifiers.Count; i++)
                        {
                            if (item.itemParameters.equipmentModifiers[i].Modifier.modifierType == EquipmentModifierSO.ModifierType.HEALTH)
                                tmpHealth += item.itemParameters.equipmentModifiers[i].Value;
                        }
                    }
                }
            return tmpHealth;
        }
        private int GetArmorEquipmentBonus(List<InventoryItem> itemsList)
        {
            int tmpArmor = 0;
            if (itemsList.Count != 0)
                foreach (InventoryItem item in itemsList)
                {
                    if (item.IsEmpty)
                        continue;
                    if (item.itemParameters.equipmentModifiers.Count != 0)
                    {
                        for (int i = 0; i < item.itemParameters.equipmentModifiers.Count; i++)
                        {
                            if (item.itemParameters.equipmentModifiers[i].Modifier.modifierType == EquipmentModifierSO.ModifierType.ARMOR)
                                tmpArmor += item.itemParameters.equipmentModifiers[i].Value;
                        }
                    }
                }
            return tmpArmor;
        }
        private int GetMagicResEquipmentBonus(List<InventoryItem> itemsList)
        {
            int tmpMRes = 0;
            if (itemsList.Count != 0)
                foreach (InventoryItem item in itemsList)
                {
                    if (item.IsEmpty)
                        continue;
                    if (item.itemParameters.equipmentModifiers.Count != 0)
                    {
                        for (int i = 0; i < item.itemParameters.equipmentModifiers.Count; i++)
                        {
                            if (item.itemParameters.equipmentModifiers[i].Modifier.modifierType == EquipmentModifierSO.ModifierType.MR)
                                tmpMRes += item.itemParameters.equipmentModifiers[i].Value;
                        }
                    }
                }
            return tmpMRes;
        }
        private int GetHealthLevelBonus() => Level * 100;
        private int GetResistanceModifier(List<InventoryItem> itemList, ResistModifierSO.ModifierType modifierType)
        {
            int tmpModifier = 0;
            if (itemList.Count != 0)
                foreach (InventoryItem item in itemList)
                {
                    if (item.IsEmpty)
                        continue;
                    if (item.itemParameters.resistModifiers.Count != 0)
                    {
                        for (int i = 0; i < item.itemParameters.resistModifiers.Count; i++)
                        {
                            if (item.itemParameters.resistModifiers[i].Modifier.modifierType == modifierType)
                                tmpModifier += item.itemParameters.resistModifiers[i].Value;
                        }
                    }
                }
            return tmpModifier;
        }
        private int GetVulnerabilityModifier(List<InventoryItem> itemList, VulnerabilityModifierSO.ModifierType modifierType)
        {
            int tmpModifier = 0;
            if (itemList.Count != 0)
                foreach (InventoryItem item in itemList)
                {
                    if (item.IsEmpty)
                        continue;
                    if (item.itemParameters.vulnerabilityModifiers.Count != 0)
                    {
                        for (int i = 0; i < item.itemParameters.vulnerabilityModifiers.Count; i++)
                        {
                            if (item.itemParameters.vulnerabilityModifiers[i].Modifier.modifierType == modifierType)
                                tmpModifier += item.itemParameters.vulnerabilityModifiers[i].Value;
                        }
                    }
                }
            return tmpModifier;
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