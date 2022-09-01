using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Levels
{
    public class IntroCS01 : MonoBehaviour
    {
        public A02Dialogue dialoguePoint;

        public void SetSceneEnded()
        {
            dialoguePoint.StartDialogueAfterIntroAnim();
        }
    }
}