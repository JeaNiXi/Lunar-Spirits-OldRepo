using Character;
using Helpers.SO;
using Levels.SO;
using Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;
using UnityEngine.Localization;

namespace Levels
{
    public class IntroLevel : MonoBehaviour
    {
        [SerializeField] public CharacterManager MainCharacter;
        [SerializeField] public bool SkipIntro = false;
        [SerializeField] LevelDialoguesSO A01;
        [SerializeField] DialogueHelperSO A01Helper;

        private Animator cAnim;

        private void Start()
        {
            if (SkipIntro)
                GameManager.Instance.GameState = GameManager.GameStates.PLAYING;
                return;
            MainCharacter = GameManager.Instance.MainCharacter;
            if (DialogueManager.Instance.IsDialogueScreenActive)
                DialogueManager.Instance.ClearDialogue();
            cAnim = MainCharacter.GetComponent<Animator>();
            cAnim.Play("BeingDowned");
            StartCoroutine(SetEnabledGameState(2f));
        }
        //private void Update()
        //{
        //    if(isDialogueActive && Mouse.current.leftButton.wasPressedThisFrame)

        //}
        private IEnumerator SetEnabledGameState(float time)
        {
            yield return new WaitForSeconds(time);
            GameManager.Instance.GameState = GameManager.GameStates.ENABLED;
            InputSystem.onAnyButtonPress.CallOnce(button => CheckForButton(button));
            yield break;
        }
        private void CheckForButton(InputControl pressedButton)
        {
            if(pressedButton == Keyboard.current.spaceKey)
            {
                InitializeIntroDialogue();
            }
            else
            {
                CallForButton();
            }
        }
        private void CallForButton()
        {
            InputSystem.onAnyButtonPress.CallOnce(button => CheckForButton(button));
        }
        private void InitializeIntroDialogue()
        {
            DialogueManager.Instance.ToggleFullDialogueView(true);
            DialogueManager.Instance.InitText(A01Helper);
            //DialogueManager.Instance.WriteLine(A01.localizedStringsList[0].GetLocalizedString());
        }
        public void hahah()
        { Debug.Log("its working"); }
        public void hahahsds()
        { Debug.Log("its working 2"); }
    }
}