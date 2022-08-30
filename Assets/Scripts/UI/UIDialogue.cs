using Helpers.SO;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
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
        [SerializeField] private Image scrollbarImage;
        [SerializeField] private Image scrollbarHandlerImage;

        [SerializeField] private RectTransform mainEmoteParent;
        [SerializeField] private RectTransform listenerEmotePanel;
        [SerializeField] private TMP_Text mainName;
        [SerializeField] private TMP_Text listenerName;

        [SerializeField] private RectTransform dialogueParentTransform;

        [SerializeField] private UIDialogueElement dialoguePrefab;
        [SerializeField] private UIEmoteRenderer emotePrefab;

        [SerializeField] private Color colorFullAlpha;
        [SerializeField] private Color colorZeroAlpha;

        public AudioSource audioSource;

        public List<UIDialogueElement> currentDialogueElements = new List<UIDialogueElement>();
        public List<UIDialogueElement> currentOptionsElements = new List<UIDialogueElement>();

        public enum DIALOGUE_TYPE
        {
            MAIN,
            LISTENER,
        }

        public event Action
            OnDialogueTextFinished;

        private const float DIALOGUE_FADE_TIME = 2f;
        private const float CHARACTER_SPRITE_FADE_TIME = 2f;

        public void EnableDialoguePanel()
        {
            StartCoroutine(FadeDialoguePanelToFullAlpha(lowerDialogueImage, DIALOGUE_FADE_TIME));
            StartCoroutine(FadeDialoguePanelToFullAlpha(scrollbarImage, DIALOGUE_FADE_TIME));
            StartCoroutine(FadeDialoguePanelToFullAlpha(scrollbarHandlerImage, DIALOGUE_FADE_TIME));
        }
        public void DisableDialoguePanel()
        {
            StartCoroutine(FadeDialoguePanelToZeroAlpha(lowerDialogueImage, DIALOGUE_FADE_TIME));
            StartCoroutine(FadeDialoguePanelToZeroAlpha(scrollbarImage, DIALOGUE_FADE_TIME));
            StartCoroutine(FadeDialoguePanelToZeroAlpha(scrollbarHandlerImage, DIALOGUE_FADE_TIME));
        }
        public void ClearDialogueUI()
        {
            for (int i = 0; i < currentDialogueElements.Count; i++)
            {
                DisableAction(currentDialogueElements[i]);
                currentDialogueElements[i].DeleteElement();
            }
            DisableDialoguePanel();
            ClearCharacterSprites();
        }
        public void ClearCharacterSprites()
        {
            ClearMainSprite();
            ClearListenerSprite();
        }
        public void ClearMainSprite()
        {
            if (upperDialogueLeftImage.color.a != 0)
                StartCoroutine(FadeCharacterSpriteToZeroAlpha(upperDialogueLeftImage, CHARACTER_SPRITE_FADE_TIME));
        }
        public void ClearListenerSprite()
        {
            if (upperDialogueRightImage.color.a != 0) 
                StartCoroutine(FadeCharacterSpriteToZeroAlpha(upperDialogueRightImage, CHARACTER_SPRITE_FADE_TIME));
        }

        public UIDialogueElement CreateDialogueElement() => Instantiate(dialoguePrefab, Vector3.zero, Quaternion.identity);
        public UIEmoteRenderer CreateEmoteElement() => Instantiate(emotePrefab, Vector3.zero, Quaternion.identity);
        public bool IsDialogueUIActive => upperDialogueScreen.gameObject.activeSelf || lowerDialogueScreen.gameObject.activeSelf;

        public void UpdateDialogueScreen(DialogueHelperSO.DialogueHelper currentDialogue)
        {
            if (lowerDialogueImage.color.a == 0) 
                EnableDialoguePanel();
            switch (currentDialogue.dialogueType)
            {
                case DialogueHelperSO.DialogueType.A:
                    //Skipping Audio Step
                    if(currentDialogue.clearMainSprite)
                    {
                        //Clear Sprite
                    }
                    else
                    {
                        if (currentDialogue.mainExpression != null)
                        {
                            if (upperDialogueLeftImage.sprite != null)
                            {
                                if (upperDialogueLeftImage.sprite == currentDialogue.mainExpression)
                                    return;
                                else
                                {
                                    //Swap Sprites
                                }
                            }
                            else
                            {
                                upperDialogueLeftImage.sprite = currentDialogue.mainExpression;
                                StartCoroutine(FadeCharacterSpriteToFullAlpha(upperDialogueLeftImage, currentDialogue.spriteChangeAnimationTime));
                            }
                        }
                    }
                    if(currentDialogue.clearListenerSprite)
                    {

                    }
                    else
                    {
                        if (currentDialogue.listenerExpression != null)
                        {
                            if (upperDialogueRightImage.sprite != null)
                            {
                                if (upperDialogueRightImage.sprite == currentDialogue.listenerExpression)
                                    return;
                                else
                                {
                                    upperDialogueRightImage.sprite = currentDialogue.listenerExpression;
                                }
                            }
                            {
                                upperDialogueRightImage.sprite = currentDialogue.listenerExpression;
                                StartCoroutine(FadeCharacterSpriteToFullAlpha(upperDialogueRightImage, currentDialogue.spriteChangeAnimationTime));
                            }
                        }
                    }
                    if (currentDialogue.dialogueAudioClip != null)
                    {
                        if (audioSource.isPlaying)
                            audioSource.Stop();
                        audioSource.PlayOneShot(currentDialogue.dialogueAudioClip);
                    }
                    if (currentDialogue.listenerEmoteToPlay != null)
                    {
                        UIEmoteRenderer newEmote = CreateEmoteElement();
                        newEmote.transform.SetParent(listenerEmotePanel);
                        newEmote.PlayEmote(currentDialogue.listenerEmoteToPlay);
                    }
                    UIDialogueElement dialogueElement = CreateDialogueElement();
                    dialogueElement.transform.SetParent(dialogueParentTransform);
                    currentDialogueElements.Add(dialogueElement);
                    EnableActions(dialogueElement);
                    dialogueElement.SetDialogueText(currentDialogue.localizedString.GetLocalizedString());
                    break;
                case DialogueHelperSO.DialogueType.B:
                    UIDialogueElement dialogueOptionElement = CreateDialogueElement();
                    dialogueOptionElement.transform.SetParent(dialogueParentTransform);
                    currentOptionsElements.Add(dialogueOptionElement);
                    dialogueOptionElement.SetDialogueText(currentDialogue.localizedString.GetLocalizedString(), currentDialogue.dialogueActionHelper);
                    DialogueManager.Instance.GetNextDialogue();
                    break;
                default:
                    break;
            }
        }

        //public void WriteDialogueText(string text)
        //{
        //    UIDialogueElement dialogueElement = CreateDialogueElement();
        //    dialogueElement.transform.SetParent(dialogueParentTransform);
        //    EnableActions(dialogueElement);
        //    dialogueElement.SetDialogueText(text);
        //}
        //public void WriteDialogueText(string text, UnityEvent eventAction)
        //{
        //    UIDialogueElement dialogueElement = CreateDialogueElement();
        //    dialogueElement.transform.SetParent(dialogueParentTransform);
        //    dialogueElement.SetAsButton();
        //    dialogueElement.SetDialogueText(text, eventAction);
        //    //DialogueManager.Instance.GetNextDialogueSequence();
        //}
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
        //public void EnableEmote(EmotesSO emoteType, DIALOGUE_TYPE dialogueType)
        //{
        //    UIEmoteRenderer newEmote = CreateEmoteElement();
        //    switch(dialogueType)
        //    {
        //        case DIALOGUE_TYPE.MAIN:
        //            newEmote.transform.SetParent(mainEmoteParent);
        //            break;
        //        case DIALOGUE_TYPE.LISTENER:
        //            newEmote.transform.SetParent(listenerEmotePanel);
        //            break;
        //        default:
        //            break;
        //    }
        //    newEmote.PlayEmote(emoteType);
        //}
        private IEnumerator FadeDialoguePanelToFullAlpha(Image image, float animationTime)
        {
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
        private IEnumerator FadeDialoguePanelToZeroAlpha(Image image, float animationTime)
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
            yield break;
        }
        private IEnumerator FadeCharacterSpriteToFullAlpha(Image image, float animationTime)
        {
            if (image.color.a == 1.0f)
                yield break;
            float aTime;
            if (animationTime != 0)
                aTime = animationTime;
            else
                aTime = CHARACTER_SPRITE_FADE_TIME;
            while (image.color.a < 1.0f)
            {
                image.color = new Color(
                    image.color.r,
                    image.color.g,
                    image.color.b,
                    image.color.a + Time.deltaTime / aTime);
                yield return null;
            }
            yield break;
        }
        private IEnumerator FadeCharacterSpriteToZeroAlpha(Image image, float animationTime)
        {
            float aTime;
            if (animationTime != 0)
                aTime = animationTime;
            else
                aTime = CHARACTER_SPRITE_FADE_TIME;
            while (image.color.a > 0)
            {
                image.color = new Color(
                    image.color.r,
                    image.color.g,
                    image.color.b,
                    image.color.a - Time.deltaTime / aTime);
                yield return null;
            }
            image.sprite = null;
            yield break;
        }
    }
}