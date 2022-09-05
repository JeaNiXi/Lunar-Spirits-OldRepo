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
        private int currentInGameDialogueIndex;
        private DialogueHelperSO currentDialogueHelperSO;
        private DialogueInGameHelperSO currentDialogueInGameHelperSO;
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
        public enum InGameDialogueStates
        {
            DISABLED,
            RUNNING,
        }

        public DialogueStates DialogueState = DialogueStates.DISABLED;
        public InGameDialogueStates InGameDialogueState = InGameDialogueStates.DISABLED;

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
        public void InitInGameDialogueScreen(DialogueInGameHelperSO dialogueInGameHelperSO)
        {
            if(InGameDialogueState != InGameDialogueStates.RUNNING)
            {
                currentInGameDialogueIndex = 0;
                InGameDialogueState = InGameDialogueStates.RUNNING;
            }
            DialogueUI.UpdateInGameDialogueScreen(dialogueInGameHelperSO.InGameDialogueHelpersList[currentInGameDialogueIndex]);
            if (currentDialogueInGameHelperSO == dialogueInGameHelperSO)
                return;
            currentDialogueInGameHelperSO = dialogueInGameHelperSO;
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
            else if (DialogueUI.GetLastDialogueType() == DialogueHelperSO.DialogueType.B) 
            {
                DialogueState = DialogueStates.DISABLED;
                isAwaitingInput = false;
            }
            else
            {
                if (EndingReference != null)
                {
                    DialogueState = DialogueStates.DISABLED;
                    isAwaitingInput = false;
                    DialogueUI.ClearDialogueUI();
                    EndingReference.SetActive(true);
                    EndingReference = null;
                }
            }
        }
        public void GetNextInGameDialogue()
        {
            if (currentDialogueInGameHelperSO.InGameDialogueHelpersList.Count > currentInGameDialogueIndex + 1)
            {
                currentInGameDialogueIndex++;
                InitInGameDialogueScreen(currentDialogueInGameHelperSO);
            }
            else
            {
                Debug.Log("should disable now");
                DialogueUI.DisableInGameDialoguePanelFull();
            }
        }
        public void SetEndingReferance(GameObject referance) =>
            EndingReference = referance;
    }
}