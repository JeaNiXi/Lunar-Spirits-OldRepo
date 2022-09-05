using Helpers.SO;
using Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Levels
{
    public class C02DialoguePoint : MonoBehaviour
    {
        public DialogueInGameHelperSO dialogueHelperSO;
        public bool IsInGameDialogueStarted = false;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if(!IsInGameDialogueStarted)
            {
                IsInGameDialogueStarted = true;
                DialogueManager.Instance.InitInGameDialogueScreen(dialogueHelperSO);
            }
        }
    }
}