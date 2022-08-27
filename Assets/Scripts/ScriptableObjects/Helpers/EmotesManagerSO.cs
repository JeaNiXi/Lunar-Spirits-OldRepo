using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Helpers.SO
{
    [CreateAssetMenu(fileName = "Emotes Manager", menuName = "Helpers/Emotes Manager")]

    public class EmotesManagerSO : ScriptableObject
    {
        public EmotesSO heartEmote;
        public EmotesSO confusionEmote;
    }
}