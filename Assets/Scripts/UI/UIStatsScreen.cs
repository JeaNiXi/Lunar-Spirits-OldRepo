using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using Inventory.SO;
using Character;

namespace Inventory.UI
{
    public class UIStatsScreen : MonoBehaviour
    {
        public TMP_Text StrengthText;
        public TMP_Text DexterityText;
        public TMP_Text ConstitutionText;
        public TMP_Text IntelligenceText;
        public TMP_Text WisdomText;
        public TMP_Text CharismaText;

        public TMP_Text CharacterLevel;
        public TMP_Text HealthText;
        public TMP_Text ArmorText;
        public TMP_Text MagicResText;

        public TMP_Text FireRes;
        public TMP_Text WaterRes;
        public TMP_Text EarthRes;
        public TMP_Text AirRes;
        public TMP_Text PoisonRes;
        public TMP_Text LightningRes;
        public TMP_Text PhysicalRes;
        public TMP_Text LightRes; 
        public TMP_Text DarkRes;

        public TMP_Text FireVulnerability;
        public TMP_Text WaterVulnerability;
        public TMP_Text EarthVulnerability;
        public TMP_Text AirVulnerability;
        public TMP_Text PoisonVulnerability;
        public TMP_Text LightningVulnerability;
        public TMP_Text PhysicalVulnerability;
        public TMP_Text LightVulnerability;
        public TMP_Text DarkVulnerability;

        public void InitializeStatsUI(CharacterManager character, List<InventoryItem> equippedItemsList)
        {
            int tmpStrength = character.ActorParams.MainStrength.Level;
            int tmpDexterity = character.ActorParams.MainDexterity.Level;
            int tmpConstitution = character.ActorParams.MainConstitution.Level;
            int tmpIntelligence = character.ActorParams.MainIntelligence.Level;
            int tmpWisdom = character.ActorParams.MainWisdom.Level;
            int tmpCharisma = character.ActorParams.MainCharisma.Level;

            if (equippedItemsList.Count != 0)
                foreach (InventoryItem item in equippedItemsList)
                {
                    if (item.IsEmpty)
                        continue;
                    if (item.itemParameters.statModifiers.Count != 0)
                    {
                        for (int i = 0; i < item.itemParameters.statModifiers.Count; i++)
                        {
                            if (item.itemParameters.statModifiers[i].Modifier.modifierType == ModifiersSO.ModifierType.STRENGTH)
                            {
                                tmpStrength += item.itemParameters.statModifiers[i].Value;
                                continue;
                            }
                            if (item.itemParameters.statModifiers[i].Modifier.modifierType == ModifiersSO.ModifierType.DEXTERITY)
                            {
                                tmpDexterity += item.itemParameters.statModifiers[i].Value;
                                continue;
                            }
                            if (item.itemParameters.statModifiers[i].Modifier.modifierType == ModifiersSO.ModifierType.CONSTITUTION)
                            {
                                tmpConstitution += item.itemParameters.statModifiers[i].Value;
                                continue;
                            }
                            if (item.itemParameters.statModifiers[i].Modifier.modifierType == ModifiersSO.ModifierType.INTELLIGENCE)
                            {
                                tmpIntelligence += item.itemParameters.statModifiers[i].Value;
                                continue;
                            }
                            if (item.itemParameters.statModifiers[i].Modifier.modifierType == ModifiersSO.ModifierType.WISDOM)
                            {  
                                tmpWisdom += item.itemParameters.statModifiers[i].Value;
                                continue;
                            }
                            if (item.itemParameters.statModifiers[i].Modifier.modifierType == ModifiersSO.ModifierType.CHARISMA)
                            {
                                tmpCharisma += item.itemParameters.statModifiers[i].Value;
                                continue;
                            }
                        }
                    }
                }
            StrengthText.text = tmpStrength.ToString();
            DexterityText.text = tmpDexterity.ToString();
            ConstitutionText.text = tmpConstitution.ToString();
            IntelligenceText.text = tmpIntelligence.ToString();
            WisdomText.text = tmpIntelligence.ToString();
            CharismaText.text = tmpCharisma.ToString();
        }
    }
}