using Helpers.SO;
using Inventory;
using Inventory.SO;
using Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class B11B12Ending : MonoBehaviour
{
    public DialogueInGameHelperSO dialogueHelperSO;
    public GameObject endingRef;


    public void OnEnable()
    {
        GameManager.Instance.GameState = GameManager.GameStates.PLAYING;
        StartCoroutine(Delay());
    }
    private IEnumerator Delay()
    {
        yield return new WaitForSeconds(3.0f);
        DialogueManager.Instance.SetEndingReferance(endingRef);
        DialogueManager.Instance.InitInGameDialogueScreen(dialogueHelperSO);
        yield break;
    }
}
