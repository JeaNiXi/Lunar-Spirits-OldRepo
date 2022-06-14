using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIItemActionPanel : MonoBehaviour
{
    [SerializeField] private GameObject buttonPrefab;
    public void TogglePanel(bool value)
    {
        if (value == true)
        {
            gameObject.SetActive(value);
            gameObject.transform.position = Input.mousePosition;
        }
        else
        {
            RemoveAllButtons();
            gameObject.SetActive(false);
        }
    }
    public void AddButton(string name, Action onClickAction)
    {
        GameObject button = Instantiate(buttonPrefab, transform);
        button.GetComponent<Button>().onClick.AddListener(() => onClickAction());
        button.GetComponent<Button>().onClick.AddListener(() => TogglePanel(false));
        button.GetComponentInChildren<TMPro.TMP_Text>().text = name;
    }
    public void RemoveAllButtons()
    {
        foreach(Transform button in transform)
        {
            Destroy(button.gameObject);
        }
    }
    public bool IsPanelActive()
    {
        if (gameObject.activeSelf)
            return true;
        else
            return false;
    }
}