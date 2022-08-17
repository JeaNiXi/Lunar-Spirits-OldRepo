using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Managers.UI
{
    public class UIDialogueElement : MonoBehaviour
    {
        [SerializeField] private Button dialogueButton;
        [SerializeField] private TMP_Text dialogueText; 
        public void SetAsButton()
        {
            dialogueButton.interactable = true;
        }
        public void SetDialogueText(string text) => dialogueText.text = text;
    }
}