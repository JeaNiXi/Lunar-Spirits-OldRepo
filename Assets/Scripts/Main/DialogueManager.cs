using Character;
using Helpers.SO;
using Managers.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Localization;

namespace Managers
{
    public class DialogueManager : MonoBehaviour
    {
        public static DialogueManager Instance;
        [SerializeField] public UIDialogue DialogueUI;

        private bool isAwaitingInput;
        private int currentDialogueIndex;
        private DialogueHelperSO currentDialogueHelperSO;
        private GameObject EndingReference;

        private void Awake()
        {
            if (Instance != null && Instance != this)
                Destroy(this);
            else
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            DialogueUI.OnDialogueTextFinished += SetAwaitingInput;
        }
        public enum DialogueStates
        {
            DISABLED,
            RUNNING,
        }
        public DialogueStates DialogueState = DialogueStates.DISABLED;
        private void Update()
        {
            if (DialogueState == DialogueStates.RUNNING && isAwaitingInput)
                if (Keyboard.current.spaceKey.wasPressedThisFrame)
                    GetNextDialogue();
        }

        public void InitDialogueScreen(DialogueHelperSO helperListSO)
        {
            if (currentDialogueHelperSO != helperListSO && DialogueUI.currentOptionsElements.Count > 0)   
                foreach (var element in DialogueUI.currentOptionsElements)
                    element.DisableButton();
            if (GameManager.Instance.GameState == GameManager.GameStates.PLAYING) 
                GameManager.Instance.GameState = GameManager.GameStates.ENABLED;
            //GameManager.Instance.MakeCharactersIdle();
            if (DialogueState != DialogueStates.RUNNING)
            {
                currentDialogueIndex = 0;
                DialogueState = DialogueStates.RUNNING;
            }
            DialogueUI.UpdateDialogueScreen(helperListSO.DialogueHelpersList[currentDialogueIndex]);
            if (currentDialogueHelperSO == helperListSO) 
                return;
            currentDialogueHelperSO = helperListSO;
        }
        public void SetAwaitingInput()
        {
            isAwaitingInput = true;
        }
        public void GetNextDialogue()
        {
            if (currentDialogueHelperSO.DialogueHelpersList.Count > currentDialogueIndex + 1)
            {
                currentDialogueIndex++;
                isAwaitingInput = false;
                InitDialogueScreen(currentDialogueHelperSO);
            }
            else if (EndingReference != null) 
            {
                DialogueState = DialogueStates.DISABLED;
                isAwaitingInput = false;
                DialogueUI.ClearDialogueUI();
                EndingReference.SetActive(true);
            }
            else
            {
                DialogueState = DialogueStates.DISABLED;
                isAwaitingInput = false;
            }
        }
        public void SetEndingReferance(GameObject referance) =>
            EndingReference = referance;


        //public bool IsDialogueScreenActive => DialogueUI.IsDialogueUIActive;
        //public void ClearDialogue() => DialogueUI.ClearDialogueUI();





        //public void ToggleFullDialogueView(bool value) => DialogueUI.ToggleFullDialogueUI(value);
        //public void WriteLine(string text)
        //{
        //    DialogueUI.WriteDialogueText(text);
        //}
        //public void InitText(DialogueHelperSO helperListSO)
        //{
        //    var stringsList = helperListSO.DialogueHelpersList;
        //    if (DialogueState != DialogueStates.RUNNING)
        //    {
        //        currentDialogueIndex = 0;
        //        DialogueState = DialogueStates.RUNNING;
        //    }
        //    if (helperListSO.DialogueHelpersList[currentDialogueIndex].dialogueType == DialogueHelperSO.DialogueType.A)
        //    {
        //        DialogueUI.WriteDialogueText(stringsList[currentDialogueIndex].localizedString.GetLocalizedString());
        //    }
        //    else
        //    {
        //        DialogueUI.WriteDialogueText(stringsList[currentDialogueIndex].localizedString.GetLocalizedString(), helperListSO.DialogueHelpersList[currentDialogueIndex].dialogueAction);
        //    }
        //    if (currentDialogueHelperSO == helperListSO)
        //        return;
        //    currentDialogueHelperSO = helperListSO;
        //}
        //public void GetNextDialogueSequence()
        //{
        //    if (currentDialogueHelperSO.DialogueHelpersList.Count > currentDialogueIndex + 1) 
        //    {
        //        currentDialogueIndex++;
        //        isAwaitingInput = false;
        //        InitText(currentDialogueHelperSO);
        //    }
        //    else
        //    {
        //        DialogueState = DialogueStates.DISABLED;
        //        isAwaitingInput = false;
        //        Debug.Log("No More Dialogue");
        //    }
        //}

        //public void ShowUpperDialogueScreen(bool value) => DialogueUI.ToggleUpperDialogueUI(value);
        //public void ShowLowerDialogueScreen(bool value) => DialogueUI.ToggleLowerDialogueUI(value);
    }
}