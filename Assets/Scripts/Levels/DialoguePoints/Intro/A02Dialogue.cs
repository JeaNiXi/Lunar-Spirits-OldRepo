using Helpers.SO;
using Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Levels
{
    public class A02Dialogue : MonoBehaviour
    {
        public DialogueHelperSO dialogueHelper;
        bool isDialogueCalled;
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if(!isDialogueCalled)
                DialogueManager.Instance.InitDialogueScreen(dialogueHelper);
            isDialogueCalled = true;
        }
    }
}