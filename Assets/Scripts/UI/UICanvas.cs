using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Managers.UI
{
    public class UICanvas : MonoBehaviour
    {
        public static UICanvas Instance;

        [SerializeField] private RectTransform SavePlacePanel;

        public void Awake()
        {
            if (Instance != null && Instance != this)
                Destroy(this);
            else
            {
                Instance = this;
                DontDestroyOnLoad(this);
            }
        }

        public void ToggleSavePlacePanel(bool value)
        {
            SavePlacePanel.gameObject.SetActive(value);
        }
    }
}