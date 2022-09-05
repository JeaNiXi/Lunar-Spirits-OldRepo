using Helpers.SO;
using Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class B11B12Ending : MonoBehaviour
{
    public DialogueInGameHelperSO dialogueHelperSO;

    public void OnEnable()
    {
        DialogueManager.Instance.InitInGameDialogueScreen(dialogueHelperSO);
    }
}
