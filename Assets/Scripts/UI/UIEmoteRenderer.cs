using Helpers.SO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem; // - TO DELETE LATER;
using UnityEngine.UI;

namespace Managers.UI
{
    public class UIEmoteRenderer : MonoBehaviour
    {
        public Image image;

        private const float ANIMATION_DELAY = 0.5f;
        private const float FADE_ANIM_DURATION = 0.3f;
        private const int REPEAT_INT = 3;

        AudioSource audioSource;

        public EmotesSO currentEmote;
        public bool isPlaying;
        public void Start()
        {
            audioSource = GetComponent<AudioSource>();
            image.enabled = true;
            image.sprite = currentEmote.spritesList[0];
        }
        private void Update()
        {
            if(Keyboard.current.eKey.wasPressedThisFrame && !isPlaying)
            {
                isPlaying = true;
                StartCoroutine(PlayFadeInAnimation());

            }

        }
        public void PlayEmote(EmotesSO emote)
        {

        }

        private IEnumerator PlayEmoteAnimation()
        {
            for (int i = 0; i < REPEAT_INT; i++)
            {
                for (int j = 0; j < currentEmote.spritesList.Count; j++)
                {
                    image.sprite = currentEmote.spritesList[j];
                    yield return new WaitForSeconds(ANIMATION_DELAY);
                }
            }
            StartCoroutine(PlayFadeOutAnimation());
            yield break;
        }
        private IEnumerator PlayFadeInAnimation()
        {
            Vector3 startScale = Vector3.zero;
            Vector3 endScale = Vector3.one;
            float currentTime = 0;
            audioSource.Play();
            while (currentTime < FADE_ANIM_DURATION)
            {
                currentTime += Time.deltaTime;
                transform.localScale = Vector3.Lerp(startScale, endScale, currentTime / FADE_ANIM_DURATION);
                yield return null;
            }
            StartCoroutine(PlayEmoteAnimation());
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