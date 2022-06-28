using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

using Actor.SO;

namespace Inventory.UI
{
    public class UIStatsScreen : MonoBehaviour
    {
        [SerializeField] private TMP_Text HealthText;

        public void UpdateStatsUI(ActorSO actor)
        {
            HealthText.text = "Health: " + actor.GetCharacterBaseHealth();
        }
    }
}