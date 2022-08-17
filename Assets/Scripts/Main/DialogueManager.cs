using Managers.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Managers
{
    public class DialogueManager : MonoBehaviour
    {
        public static DialogueManager Instance;
        [SerializeField] public UIDialogue DialogueUI;

        private void Awake()
        {
            if (Instance != null && Instance != this)
                Destroy(this);
            else
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
        }
        public enum DialogueStates
        {
            DISABLED,
            RUNNING,
        }
        public DialogueStates DialogueState = DialogueStates.DISABLED;
        public bool IsDialogueScreenActive => DialogueUI.IsDialogueUIActive;
        public void ClearDialogue() => DialogueUI.ClearDialogueUI();
        public void ToggleFullDialogueView(bool value) => DialogueUI.ToggleFullDialogueUI(value);
        public void WriteLine(string text)
        {
            DialogueUI.WriteDialogueText(text);
        }
        //public void ShowUpperDialogueScreen(bool value) => DialogueUI.ToggleUpperDialogueUI(value);
        //public void ShowLowerDialogueScreen(bool value) => DialogueUI.ToggleLowerDialogueUI(value);
    }
}