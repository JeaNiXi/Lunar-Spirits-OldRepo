using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIItemDescriptionPanel : MonoBehaviour
{
    public TMP_Text Name;
    public TMP_Text Rarity;
    public TMP_Text Description;

    public void SetUpData(string name, string rarity, string description)
    {
        this.Name.text = name;
        this.Rarity.text = rarity;
        this.Description.text = description;
    }
    public void Toggle(bool val)
    {
        gameObject.SetActive(val);
    }
    public void ResetDescription()
    {
        this.Name.text = "";
        this.Rarity.text = "";
        this.Description.text = "";
    }
}
