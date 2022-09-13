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
        [SerializeField] AudioClip introTheme;
        
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
            AudioManager.Instance.PlayBackgroundAudio(introTheme);
            MainCharacter = GameManager.Instance.MainCharacter;
            MainCharacter.gameObject.transform.position = new Vector3(-6.8f, -0.16f, 0);
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