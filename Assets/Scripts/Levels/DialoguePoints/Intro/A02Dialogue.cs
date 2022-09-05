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

        bool isDialogueCalled;
        private void OnTriggerEnter2D(Collider2D collision)
        {
            //mainAnimator.SetBool("cutscene_one", true);
            //cutsceneAnimator.Play("EllynWalkingDown");
            StartDialogueAfterIntroAnim();
        }
        public void StartDialogueAfterIntroAnim()
        {
            DialogueManager.Instance.SetEndingReferance(helperEnding);
            mainAnimator.SetBool("cutscene_one", false);
            if (!isDialogueCalled)
                DialogueManager.Instance.InitDialogueScreen(dialogueHelper);
            isDialogueCalled = true;
        }
    }
}