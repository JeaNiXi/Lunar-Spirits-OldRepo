using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Localization;

namespace Helpers.SO
{
    [CreateAssetMenu(fileName = "Level Dialogues Helper", menuName = "Levels/DialogueCollectionHelper")]
    public class DialogueHelperSO : ScriptableObject
    {
        public enum DialogueType
        {
            A,
            B,
        }
        [NonReorderable] public List<DialogueHelper> DialogueHelpersList = new List<DialogueHelper>();

        [Serializable]
        public struct DialogueHelper
        {
            public LocalizedString localizedString;
            public DialogueType dialogueType;
            public UnityEvent dialogueAction;
        }
    }
}