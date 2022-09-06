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
        [Space]
        [Header("IN GAME DIALOGUE")]
        [SerializeField] private Image InGameDSprite;
        [SerializeField] private TMP_Text InGameDText;
        [SerializeField] private RectTransform InGameDEmoteParent;
        [SerializeField] private Image PanelImage;

        public AudioSource audioSource;

        public List<UIDialogueElement> currentDialogueElements = new List<UIDialogueElement>();
        public List<UIDialogueElement> currentOptionsElements = new List<UIDialogueElement>();

        DialogueHelperSO.DialogueType LastDialogueType;

        public enum DIALOGUE_TYPE
        {
            MAIN,
            LISTENER,
        }

        public event Action
            OnDialogueTextFinished;

        private const float DIALOGUE_FADE_TIME = 2.0f;
        private const float IN_GAME_DIALOGUE_TIME = 1.5f;
        private const float IN_GAME_DIALOGUE_TEXT_TIME = 1.5f;
        private const float IN_GAME_DIALOGUE_EXIST_TIME = 8.0f;
        private const float CHARACTER_SPRITE_FADE_TIME = 2.0f;

        public DialogueHelperSO.DialogueType GetLastDialogueType() => LastDialogueType;
            

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
            for (int i = 0; i < currentOptionsElements.Count; i++)
            {
                currentOptionsElements[i].DeleteElement();
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

        //public void EnableInGameDialoguePanelFull()
        //{
        //    StartCoroutine(FadeDialoguePanelToFullAlpha(InGameDSprite, IN_GAME_DIALOGUE_TIME));
        //    StartCoroutine(FadeDialoguePanelToFullAlpha(PanelImage, IN_GAME_DIALOGUE_TIME));
        //    StartCoroutine(FadeTextToFullAlpha(InGameDText, IN_GAME_DIALOGUE_TEXT_TIME));
        //}
        //public void EnableInGameDialogueMainSprite()
        //{
        //    StartCoroutine(FadeDialoguePanelToFullAlpha(InGameDSprite, IN_GAME_DIALOGUE_TIME));
        //}
        //public void EnableInGameDialoguePanelSprite()
        //{
        //    StartCoroutine(FadeDialoguePanelToFullAlpha(PanelImage, IN_GAME_DIALOGUE_TIME));
        //    StartCoroutine(FadeTextToFullAlpha(InGameDText, IN_GAME_DIALOGUE_TEXT_TIME));
        //}


        public void DisableInGameDialoguePanelFull()
        {
            DisableInGameDialogueMainSprite();
            DisableInGameDialoguePanelSprite();
        }
        public void DisableInGameDialogueMainSprite()
        {
            StartCoroutine(FadeInGameDialogueCharacterToZeroAlpha(InGameDSprite, IN_GAME_DIALOGUE_TIME));
        }
        public void DisableInGameDialoguePanelSprite()
        {
            StartCoroutine(FadeInGameDialoguePanelToZeroAlpha(PanelImage, IN_GAME_DIALOGUE_TIME));
            StartCoroutine(FadeTextToZeroAlpha(InGameDText, IN_GAME_DIALOGUE_TEXT_TIME));
        }




        public UIDialogueElement CreateDialogueElement() => Instantiate(dialoguePrefab, Vector3.zero, Quaternion.identity);
        public UIEmoteRenderer CreateEmoteElement() => Instantiate(emotePrefab, Vector3.zero, Quaternion.identity);
        public bool IsDialogueUIActive => upperDialogueScreen.gameObject.activeSelf || lowerDialogueScreen.gameObject.activeSelf;

        public void UpdateDialogueScreen(DialogueHelperSO.DialogueHelper currentDialogue)
        {
            if (lowerDialogueImage.color.a < 1)
                EnableDialoguePanel();
            LastDialogueType = currentDialogue.dialogueType;
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
                                    //Swap Sprites
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
                                    upperDialogueRightImage.sprite = currentDialogue.listenerExpression;
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
        public void UpdateInGameDialogueScreen(DialogueInGameHelperSO.InGameDialogueHelper currentDialogue)
        {
            if (currentDialogue.clearSprite)
                throw new NotImplementedException();
            else if (currentDialogue.inGameSprite != null)
            {
                if (InGameDSprite.sprite == null)
                {
                    InGameDSprite.sprite = currentDialogue.inGameSprite;
                    StartCoroutine(FadeInGameDialogueCharacterToFullAlpha(InGameDSprite, IN_GAME_DIALOGUE_TIME));
                }
                else
                {
                    InGameDSprite.sprite = currentDialogue.inGameSprite;
                }
            }
            if (currentDialogue.clearTextPanel)
                throw new NotImplementedException();
            else
            {
                if (currentDialogue.localizedString != null)
                {
                    InGameDText.text = currentDialogue.localizedString.GetLocalizedString();
                    StartCoroutine(FadeInGameDialoguePanelToFullAlpha(PanelImage, IN_GAME_DIALOGUE_TIME));
                    StartCoroutine(FadeTextToFullAlpha(InGameDText, IN_GAME_DIALOGUE_TEXT_TIME));
                }
                else
                {
                    InGameDText.text = currentDialogue.localizedString.GetLocalizedString();
                }
            }
            if (currentDialogue.emoteToPlay != null)
            {
                UIEmoteRenderer newEmote = CreateEmoteElement();
                newEmote.transform.SetParent(InGameDEmoteParent);
                newEmote.PlayEmote(currentDialogue.emoteToPlay);
            }
            ////SKIPING SOUND ETC.
            if (currentDialogue.existTime != 0)
                StartCoroutine(InGameDialogueTimer(currentDialogue.existTime));
            else
                StartCoroutine(InGameDialogueTimer(IN_GAME_DIALOGUE_EXIST_TIME));
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

        private IEnumerator FadeDialoguePanelToFullAlpha(Image image, float animationTime)
        {
            while (image.color.a < 1.0f)
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



        private IEnumerator FadeTextToFullAlpha(TMP_Text text, float animationTime)
        {
            while (text.color.a < 1.0f)
            {
                text.color = new Color(
                    text.color.r,
                    text.color.g,
                    text.color.b,
                    text.color.a + Time.deltaTime / animationTime);
                yield return null;
            }
            yield break;
        }
        private IEnumerator FadeTextToZeroAlpha(TMP_Text text, float animationTime)
        {
            while (text.color.a > 0)
            {
                text.color = new Color(
                    text.color.r,
                    text.color.g,
                    text.color.b,
                    text.color.a - Time.deltaTime / animationTime);
                yield return null;
            }
            text.text = "";
            yield break;
        }
        private IEnumerator FadeInGameDialogueCharacterToFullAlpha(Image image, float animationTime)
        {
            while (image.color.a < 1.0f)
            {
                Debug.Log(image.color.a);
                image.color = new Color(
                    image.color.r,
                    image.color.g,
                    image.color.b,
                    image.color.a + Time.deltaTime / animationTime);
                yield return null;
            }
            yield break;
        }
        private IEnumerator FadeInGameDialogueCharacterToZeroAlpha(Image image, float animationTime)
        {
            Debug.Log("so color is = " + image.color.a);
            while (image.color.a > 0)
            {
                Debug.Log(image.color.a);
                image.color = new Color(
                    image.color.r,
                    image.color.g,
                    image.color.b,
                    image.color.a - Time.deltaTime / animationTime);
                yield return null;
            }
            image.sprite = null;
            yield break;
        }
        private IEnumerator FadeInGameDialoguePanelToFullAlpha(Image image, float animationTime)
        {
            while (image.color.a < 1.0f)
            {
                Debug.Log(image.color.a);
                image.color = new Color(
                    image.color.r,
                    image.color.g,
                    image.color.b,
                    image.color.a + Time.deltaTime / animationTime);
                yield return null;
            }
            yield break;
        }
        private IEnumerator FadeInGameDialoguePanelToZeroAlpha(Image image, float animationTime)
        {
            Debug.Log("so color is = " + image.color.a);
            while (image.color.a > 0)
            {
                Debug.Log(image.color.a);
                image.color = new Color(
                    image.color.r,
                    image.color.g,
                    image.color.b,
                    image.color.a - Time.deltaTime / animationTime);
                yield return null;
            }
            yield break;
        }
        private IEnumerator InGameDialogueTimer(float time)
        {
            Debug.Log("EXIST CORO STARTED");
            yield return new WaitForSeconds(time);
            Debug.Log("EXIST CORO STOPPING");
            DialogueManager.Instance.GetNextInGameDialogue();
            yield break;
        }    
    }
}