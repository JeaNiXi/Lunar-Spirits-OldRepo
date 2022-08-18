using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Managers.UI
{
    public class UIDialogue : MonoBehaviour
    {
        [SerializeField] private RectTransform upperDialogueScreen;
        [SerializeField] private RectTransform lowerDialogueScreen;
        [SerializeField] private Image upperDialogueLeftImage;
        [SerializeField] private Image upperDialogueRightImage;
        [SerializeField] private Image lowerDialogueImage;
        [SerializeField] private Image leftImageSprite;
        [SerializeField] private Image rightImageSprite;

        [SerializeField] private RectTransform dialogueParentTransform;

        [SerializeField] private UIDialogueElement dialoguePrefab;

        [SerializeField] private Color colorFullAlpha;
        [SerializeField] private Color colorZeroAlpha;



        public event Action
            OnDialogueTextFinished;

        private const float DIALOGUE_FADE_TIME = 4f;


        public void ToggleDialogueUI(RectTransform transform, Image image, bool value)
        {
            if (value)
                StartCoroutine(FadeDialoguePanelToFullAlpha(transform, image, DIALOGUE_FADE_TIME));
            else
                StartCoroutine(FadeDialoguePanelToZeroAlpha(transform, image, DIALOGUE_FADE_TIME));
        }
        public void ToggleFullDialogueUI(bool value)
        {
            ToggleDialogueUI(upperDialogueScreen, upperDialogueLeftImage, value);
            ToggleDialogueUI(upperDialogueScreen, upperDialogueRightImage, value);
            ToggleDialogueUI(lowerDialogueScreen, lowerDialogueImage, value);
        }
        public void ClearDialogueUI()
        {
            upperDialogueLeftImage.color = colorZeroAlpha;
            upperDialogueRightImage.color = colorZeroAlpha;
            upperDialogueScreen.gameObject.SetActive(false);
            lowerDialogueImage.color = colorZeroAlpha;
            lowerDialogueScreen.gameObject.SetActive(false);
        }
        
        public UIDialogueElement CreateDialogueElement() => Instantiate(dialoguePrefab, Vector3.zero, Quaternion.identity);
        public bool IsDialogueUIActive => upperDialogueScreen.gameObject.activeSelf || lowerDialogueScreen.gameObject.activeSelf;

        public void WriteDialogueText(string text)
        {
            UIDialogueElement dialogueElement = CreateDialogueElement();
            dialogueElement.transform.SetParent(dialogueParentTransform);
            EnableActions(dialogueElement);
            dialogueElement.SetDialogueText(text);
        }
        public void WriteDialogueText(string text, UnityEvent eventAction)
        {
            UIDialogueElement dialogueElement = CreateDialogueElement();
            dialogueElement.transform.SetParent(dialogueParentTransform);
            dialogueElement.SetAsButton();
            dialogueElement.SetDialogueText(text, eventAction);
            DialogueManager.Instance.GetNextDialogueSequence();
        }
        private void EnableActions(UIDialogueElement dialogueElement)
        {
            dialogueElement.OnDialogueInitFinished += HandleDialogueWritingFinishing;
        }
        private void DisableAction(UIDialogueElement dialogueElement)
        {
            dialogueElement.OnDialogueInitFinished -= HandleDialogueWritingFinishing;
        }
        private void HandleDialogueWritingFinishing()
        {
            OnDialogueTextFinished?.Invoke();
        }
        private IEnumerator FadeDialoguePanelToFullAlpha(RectTransform transform, Image image, float animationTime)
        {
            if(!transform.gameObject.activeSelf)
                transform.gameObject.SetActive(true);
            while (lowerDialogueImage.color.a < 1.0f)
            {
                image.color = new Color(
                    image.color.r,
                    image.color.g,
                    image.color.b,
                    image.color.a + Time.deltaTime / animationTime);
                yield return null;
            }
            yield break;
        }
        private IEnumerator FadeDialoguePanelToZeroAlpha(RectTransform transform, Image image, float animationTime)
        {
            while (image.color.a > 0)
            {
                image.color = new Color(
                    image.color.r,
                    image.color.g,
                    image.color.b,
                    image.color.a - Time.deltaTime / animationTime);
                yield return null;
            }
            if (transform.gameObject.activeSelf)
                transform.gameObject.SetActive(false);
            yield break;
        }
        
    }
}