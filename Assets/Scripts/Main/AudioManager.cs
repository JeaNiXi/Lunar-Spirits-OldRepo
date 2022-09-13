using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Managers
{
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager Instance;
        [SerializeField] private AudioSource audioSource;

        private float VolumeFadeTime = 2.0f;

        private void Awake()
        {
            if (Instance != null && Instance != this)
                Destroy(this);
            else
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
        }

        public void PlayBackgroundAudio(AudioClip audio)
        {
            audioSource.clip = audio;
            audioSource.Play();
        }
        public void StopAudio()
        {
            StartCoroutine(AudioFadeCoroutine());
        }
        private IEnumerator AudioFadeCoroutine()
        {
            while (audioSource.volume > 0)
            {
                audioSource.volume -= Time.deltaTime / VolumeFadeTime;
                yield return null;
            }
            audioSource.Stop();
            audioSource.volume = 0.5f;
            yield break;
        }
    }
}