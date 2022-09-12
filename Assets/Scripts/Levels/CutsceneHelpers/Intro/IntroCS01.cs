using Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Levels
{
    public class IntroCS01 : MonoBehaviour
    {
        public A02Dialogue dialoguePoint;
        private Vector3 newCharPos = new Vector3(43, -13, 0);
        public void SetSceneEnded()
        {
            dialoguePoint.StartDialogueAfterIntroAnim();
        }
        public void SetCharacterPositionAndRotation()
        {
            GameManager.Instance.MainCharacter.transform.position = newCharPos;
            GameManager.Instance.MainCharacter.SetIdleUpAnimation();
        }
    }
}