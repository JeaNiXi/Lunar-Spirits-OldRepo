using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

namespace Helpers.SO
{
    [CreateAssetMenu(fileName = "Level InGame Dialogues Helper", menuName = "Levels/InGame DialogueCollectionHelper")]
    public class DialogueInGameHelperSO : ScriptableObject
    {
        [NonReorderable] public List<InGameDialogueHelper> InGameDialogueHelpersList = new List<InGameDialogueHelper>();

        [Serializable]
        public struct InGameDialogueHelper
        {
            public Sprite inGameSprite;
            public LocalizedString localizedString;
            public EmotesSO emoteToPlay;
            public AudioClip clipToPlay;
            [Space]
            [Header("EDITING")]
            public bool clearSprite;
            public bool clearTextPanel;
            public float existTime;
        }
    }
}