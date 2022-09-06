using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.Events;
using Helpers.SO;
using UnityEngine.InputSystem;

namespace Managers.UI
{
    public class UIDialogueElement : MonoBehaviour
    {

        public event Action
            OnDialogueInitFinished;

        [SerializeField] private Button dialogueButton;
        [SerializeField] private TMP_Text dialogueText;

        private bool isDialogueSkipped = false;
        private bool isWritingDialogue = false;

        public void Update()
        {
            if (!isDialogueSkipped && isWritingDialogue && Keyboard.current.spaceKey.wasPressedThisFrame)
                isDialogueSkipped = true;

        }

        private const float TEXT_DELAY = 0.02f;
        private const float DIALOGUE_DELAY = 0.02f;
        public void SetAsButton()
        {
            dialogueButton.interactable = true;
        }
        public void DisableButton()
        {
            dialogueButton.interactable = false;
        }
        public void SetButtonAction(DialogueHelperSO dialogueActionHelper)
        {
            //dialogueButton.onClick.AddListener(() => Instantiate(dialogueActionPrefab, Vector3.zero, Quaternion.identity));
            dialogueButton.onClick.AddListener(() => DialogueManager.Instance.InitDialogueScreen(dialogueActionHelper));
        }
        public void SetDialogueText(string text)
        {
            StartCoroutine(DialogueWriter(text, TEXT_DELAY));
        }
        public void SetDialogueText(string text, DialogueHelperSO dialogueActionHelper)
        {
            SetAsButton();
            SetButtonAction(dialogueActionHelper);
            StartCoroutine(DialogueWriter(text, DIALOGUE_DELAY));
        }
        public void PrepareLayout(string tmpText)
        {
            dialogueText.text = tmpText;
            Debug.Log(dialogueText.gameObject.GetComponent<RectTransform>().sizeDelta.y);
            GetComponent<RectTransform>().sizeDelta = new Vector2(GetComponent<RectTransform>().sizeDelta.x, dialogueText.gameObject.GetComponent<RectTransform>().sizeDelta.y);
            LayoutRebuilder.ForceRebuildLayoutImmediate(gameObject.GetComponent<RectTransform>());
        }

        private IEnumerator DialogueWriter(string stringText, float delay)
        {
            //PrepareLayout(stringText);
            bool isPayload = true;
            var newString = new System.Text.StringBuilder(stringText.Length);
            isWritingDialogue = true;
            for (int i = 0; i < stringText.Length; i++)
            {
                char c = stringText[i];
                if (c == '<')
                    isPayload = false;
                if (isPayload)
                {
                    newString.Append(c);
                    dialogueText.text = newString.ToString();
                    if (!isDialogueSkipped)
                        yield return new WaitForSeconds(delay);
                    else
                        yield return null;
                }
                else
                    newString.Append(c);
                if (c == '>')
                    isPayload = true;
            }
            isWritingDialogue = false;
            OnDialogueInitFinished?.Invoke();
        }
        public void DeleteElement()
        {
            Destroy(gameObject);
        }

        //private int GetPayloadLength(string s)
        //{
        //    int count = 0;
        //    bool isPayload = true;
        //    for (int i = 0; i < s.Length; i++)
        //    {
        //        char c = s[i];
        //        if (isPayload)
        //        {
        //            if (c == '<')
        //                isPayload = false;
        //            else
        //                count++;
        //        }
        //        else
        //            if (c == '>')
        //            isPayload = true;
        //    }
        //    return count;
        //}
        //private string GetPartialPayload(string s, int typedSoFar)
        //{
        //    var tmpString = new System.Text.StringBuilder(s.Length);
        //    int count = 0;
        //    bool isPayload = true;
        //    for (int i = 0; i < s.Length; i++)
        //    {
        //        char c = s[i];
        //        if (c == '<')
        //            isPayload = false;
        //        if (isPayload && count < typedSoFar)
        //        {
        //            count++;
        //            tmpString.Append(c);
        //        }
        //        else if (!isPayload)
        //            tmpString.Append(c);
        //        if (c == '>')
        //            isPayload = true;
        //    }
        //    return tmpString.ToString();
        //}
    }
}