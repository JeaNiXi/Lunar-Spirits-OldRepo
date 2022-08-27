using Managers.SO;
using Managers.UI;
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
        //public enum SpeakerType
        //{
        //    MAIN,
        //    LISTENER,
        //}
        [NonReorderable] public List<DialogueHelper> DialogueHelpersList = new List<DialogueHelper>();

        [Serializable]
        public struct DialogueHelper
        {
            [Header("DIALOGUE TYPE")]
            public DialogueType dialogueType;
            [Space]
            [Header("DIALOGUE EVENTS")]
            public UnityEvent dialogueAction;
            public AudioClip dialogueAudioClip;
            public AudioClip backgroundMusic;
            [Space]
            [Header("UI CHARACTER PARAMETERS")]
            public ActorManagerSO mainActor;
            public ActorManagerSO listenerActor;
            //public SpeakerType speakerType;
            public Sprite mainExpression;
            public Sprite listenerExpression;
            public EmotesSO mainEmoteToPlay;
            public EmotesSO listenerEmoteToPlay;
            public LocalizedString localizedString;
            [Space]
            [Header("SCREEN EDIT")]
            public bool clearMainSprite;
            public bool clearListenerSprite;
            [Space]
            [Header("OPTIONAL PARAMS")]
            public float spriteChangeAnimationTime;
        }
    }
}