using Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Levels
{
    public class IntroCS01 : MonoBehaviour
    {

        public AudioSource castingPoint;
        public AudioSource trail1;
        public AudioSource trail2;
        public AudioSource trail3;
        public AudioSource trail4;
        public AudioSource trail5;

        public AudioSource Explosion1;
        public AudioSource Explosion2;
        public AudioSource Explosion3;
        public AudioSource Explosion4;
        public AudioSource Explosion5;

        public AudioSource knock;


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

        public void PlayCastingAudio()
        {
            castingPoint.Play();
        }
        public void PlayTrailOne()
        {
            trail1.Play();
        }
        public void PlayTrailTwo()
        {
            trail2.Play();
        }
        public void PlayTrailThree()
        {
            trail3.Play();
        }
        public void PlayTrailFour()
        {
            trail4.Play();
        }
        public void PlayTrailFive()
        {
            trail5.Play();
        }
        public void PlayExplosionOne()
        {
            Explosion1.Play();
        }
        public void PlayExplosionTwo()
        {
            Explosion2.Play();
        }
        public void PlayExplosionThree()
        {
            Explosion3.Play();
        }
        public void PlayExplosionFour()
        {
            Explosion4.Play();
        }
        public void PlayExplosionFive()
        {
            Explosion5.Play();
        }
        public void PlayKnock()
        {
            knock.Play();
        }
    }
}