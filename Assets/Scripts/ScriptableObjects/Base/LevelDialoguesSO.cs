using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

namespace Levels.SO
{
    [CreateAssetMenu(fileName = "Level Dialogues", menuName = "Levels/DialogueCollection")]
    public class LevelDialoguesSO : ScriptableObject
    {
        [SerializeField] public List<LocalizedString> localizedStringsList = new List<LocalizedString>();
    }
}