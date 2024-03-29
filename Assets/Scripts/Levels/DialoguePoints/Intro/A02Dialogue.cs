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
        public Animator mainAnimator;
        public Animator cutsceneAnimator;
        public GameObject helperEnding;

        public bool TriggerEntered;
        public bool SkipCutscene;

        public AudioClip dialogueAudio;

        bool isDialogueCalled;
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (TriggerEntered)
                return;
            if(collision.gameObject.CompareTag("Player"))
            {
                if (!SkipCutscene)
                {
                    mainAnimator.SetBool("cutscene_one", true);
                    cutsceneAnimator.Play("EllynWalkingDown");
                    AudioManager.Instance.StopAudio();
                }
                else
                    StartDialogueAfterIntroAnim();
                TriggerEntered = true;
            }
        }
        public void StartDialogueAfterIntroAnim()
        {
            DialogueManager.Instance.SetEndingReferance(helperEnding);
            mainAnimator.SetBool("cutscene_one", false);
            if (!isDialogueCalled)
                DialogueManager.Instance.InitDialogueScreen(dialogueHelper);
            AudioManager.Instance.PlayBackgroundAudio(dialogueAudio);
            isDialogueCalled = true;
        }
    }
}