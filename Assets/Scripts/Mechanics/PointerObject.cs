using Levels;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Mechanics
{
    public class PointerObject : MonoBehaviour,
        IPointerClickHandler

    {
        public UnityEvent onClickEvent = new UnityEvent();

        private const float FADE_ANIM_DURATION = 0.3f;
        public void OnEnable()
        {
            StartCoroutine(PlayFadeInAnimation());
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            onClickEvent.Invoke();
            StartCoroutine(PlayFadeOutAnimation());
        }
        private void PlayPointerAnimation()
        {
            GetComponent<Animator>().Play("Idle_PointerAnimation");
        }
        private IEnumerator PlayFadeInAnimation()
        {
            Vector3 startScale = Vector3.zero;
            Vector3 endScale = Vector3.one;
            float currentTime = 0;
            GetComponent<AudioSource>().Play();
            while (currentTime < FADE_ANIM_DURATION)
            {
                currentTime += Time.deltaTime;
                transform.localScale = Vector3.Lerp(startScale, endScale, currentTime / FADE_ANIM_DURATION);
                yield return null;
            }
            PlayPointerAnimation();
            yield break;
        }
        private IEnumerator PlayFadeOutAnimation()
        {
            Vector3 startScale = Vector3.one;
            Vector3 endScale = Vector3.zero;
            float currentTime = 0;
            while (currentTime < FADE_ANIM_DURATION)
            {
                currentTime += Time.deltaTime;
                transform.localScale = Vector3.Lerp(startScale, endScale, currentTime / FADE_ANIM_DURATION);
                yield return null;
            }
            Destroy(gameObject);
        }
    }
}