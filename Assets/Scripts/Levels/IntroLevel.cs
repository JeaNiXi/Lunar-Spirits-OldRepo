using Character;
using Helpers.SO;
using Managers;
using Mechanics;
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
        [SerializeField] DialogueHelperSO A01Helper;
        [SerializeField] GameObject HelperEnding;

        private Animator cAnim;

        //LEVEL POINTER OBJECTS
        public PointerObject introPointer;

        private void Start()
        {
            if (SkipIntro)
            {
                GameManager.Instance.GameState = GameManager.GameStates.PLAYING;
                return;
            }
            MainCharacter = GameManager.Instance.MainCharacter;

            cAnim = MainCharacter.GetComponent<Animator>();
            cAnim.Play("BeingDowned");
            StartCoroutine(SetEnabledGameState(2f));
            DialogueManager.Instance.SetEndingReferance(HelperEnding);
        }
        private IEnumerator SetEnabledGameState(float time)
        {
            yield return new WaitForSeconds(time);
            GameManager.Instance.GameState = GameManager.GameStates.ENABLED;
            introPointer.gameObject.SetActive(true);
            yield break;
        }
        public void InitializeIntroDialogue() =>
            DialogueManager.Instance.InitDialogueScreen(A01Helper);
    }
}