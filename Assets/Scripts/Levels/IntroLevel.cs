using Character;
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

        [SerializeField] LevelDialoguesSO A01;

        private Animator cAnim;
        private int currentDialogueIndex;
        private bool isDialogueActive;

        private void Start()
        {
            MainCharacter = GameManager.Instance.MainCharacter;
            if (DialogueManager.Instance.IsDialogueScreenActive)
                DialogueManager.Instance.ClearDialogue();
            cAnim = MainCharacter.GetComponent<Animator>();
            cAnim.Play("BeingDowned");
            StartCoroutine(SetEnabledGameState(5f));
        }
        //private void Update()
        //{
        //    if(isDialogueActive && Mouse.current.leftButton.wasPressedThisFrame)

        //}
        private IEnumerator SetEnabledGameState(float time)
        {
            yield return new WaitForSeconds(time);
            GameManager.Instance.GameState = GameManager.GameStates.ENABLED;
            InputSystem.onAnyButtonPress.CallOnce(button => InitializeIntroDialogue(button));
            yield break;
        }
        private void InitializeIntroDialogue(InputControl pressedButton)
        {
            DialogueManager.Instance.ToggleFullDialogueView(true);
            DialogueManager.Instance.WriteLine(A01.localizedStringsList[0].GetLocalizedString());
        }
    }
}