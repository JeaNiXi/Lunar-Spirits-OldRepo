using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Helpers.SO
{
    [CreateAssetMenu(fileName = "Emotes Helper", menuName = "Helpers/Emotes Helper")]
    public class EmotesSO : ScriptableObject
    {
        [NonReorderable] public List<Sprite> spritesList = new List<Sprite>();
        public AudioClip emoteSound;
    }
}