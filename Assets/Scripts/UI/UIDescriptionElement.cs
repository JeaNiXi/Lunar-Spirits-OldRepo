using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Inventory.UI
{
    public class UIDescriptionElement : MonoBehaviour
    {
        public TMP_Text valueText;
        public TMP_Text statText;

        public void SetValue(string valueString)
        {
            if (System.Convert.ToInt32(valueString) > 0)
                valueText.text = "+" + valueString;
            else
                valueText.text = valueString;
        }
        public void SetStatText(string statText)
        {
            this.statText.text = statText;
        }
        public void DestroyElement() => Destroy(gameObject);
    }
}