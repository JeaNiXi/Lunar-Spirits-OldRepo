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

        public void InitializeStatsUI(CharacterManager character, List<InventoryItem> equippedItemsList)
        {
            int tmpStrength = character.ActorParams.MainStrength.Level;
            if (equippedItemsList.Count != 0)
                foreach (InventoryItem item in equippedItemsList)
                {
                    if (item.IsEmpty)
                        continue;
                    if (item.itemParameters.statModifiers.Count != 0)
                    {
                        Debug.Log(item.itemParameters.statModifiers.Count);
                        for (int i = 0; i < item.itemParameters.statModifiers.Count; i++)
                        {
                            if (item.itemParameters.statModifiers[i].Modifier.modifierType == ModifiersSO.ModifierType.STRENGTH)
                                tmpStrength += item.itemParameters.statModifiers[i].Value;
                        }
                    }
                }
            StrengthText.text = tmpStrength.ToString();
        }
    }
}