using Helpers.SO;
using Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Levels
{
    public class B00 : MonoBehaviour
    {
        public DialogueHelperSO dialogueOption;
        public void OnEnable()
        {
            DialogueManager.Instance.InitDialogueScreen(dialogueOption);
        }
    }
}