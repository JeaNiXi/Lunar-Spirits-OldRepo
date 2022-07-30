using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using Inventory.SO;

namespace Inventory.UI
{
    public class UIDescriptionPanel : MonoBehaviour
    {
        public UIDescriptionElement descriptionPrefab;
        public TMP_Text nameText;
        public TMP_Text rarityText;
        public TMP_Text descriptionText;
        public RectTransform leftStatPanel;
        public RectTransform rightStatPanel;

        public RectTransform descriptionRectTransform;

        private Vector2 mousePosition;
        private float pivotX;
        private float pivotY;

        private List<UIDescriptionElement> elementsList = new List<UIDescriptionElement>();
        private void Update()
        {
            mousePosition = Mouse.current.position.ReadValue();
            pivotX = mousePosition.x / Screen.width;
            pivotY = mousePosition.y / Screen.height;

            descriptionRectTransform.pivot = new Vector2(pivotX, pivotY);
            gameObject.transform.position = mousePosition;

        }

        public void TogglePanel(bool value)
        {
            gameObject.SetActive(value);
        }
        private UIDescriptionElement CreateDescriptionElement() => Instantiate(descriptionPrefab, Vector3.zero, Quaternion.identity);

        public void SetName(string name)
        {
            nameText.text = name;
        }
        public void SetRarity(string rarity)
        {
            rarityText.text = rarity;
        }
        public void SetDescription(string description)
        {
            descriptionText.text = description;
        }
        public void CreateItemParameters(ItemParameters parameters)
        {
            if (parameters.weaponModifiers.Count != 0)
                for (int i = 0; i < parameters.weaponModifiers.Count; i++)
                {
                    UIDescriptionElement descriptionElement = CreateDescriptionElement();
                    descriptionElement.transform.SetParent(leftStatPanel);
                    descriptionElement.SetValue(parameters.weaponModifiers[i].Value.ToString());
                    descriptionElement.SetStatText(parameters.weaponModifiers[i].Modifier.ModifierName);
                    elementsList.Add(descriptionElement);
                }
            if (parameters.equipmentModifiers.Count != 0)
                for (int i = 0; i < parameters.equipmentModifiers.Count; i++)
                {

                }
            if (parameters.statModifiers.Count != 0)
                for (int i = 0; i < parameters.statModifiers.Count; i++)
                {
                    UIDescriptionElement descriptionElement = CreateDescriptionElement();
                    descriptionElement.transform.SetParent(leftStatPanel);
                    descriptionElement.SetValue(parameters.statModifiers[i].Value.ToString());
                    descriptionElement.SetStatText(parameters.statModifiers[i].Modifier.ModifierName);
                    elementsList.Add(descriptionElement);
                }
            if (parameters.weaponStatModifiers.Count != 0)
                for (int i = 0; i < parameters.weaponStatModifiers.Count; i++)
                {
                    UIDescriptionElement descriptionElement = CreateDescriptionElement();
                    descriptionElement.transform.SetParent(rightStatPanel);
                    descriptionElement.SetValue(parameters.weaponStatModifiers[i].Value.ToString());
                    descriptionElement.SetStatText(parameters.weaponStatModifiers[i].Modifier.ModifierName);
                    elementsList.Add(descriptionElement);
                }
        }
        public void ClearPanel()
        {
            foreach (var element in elementsList)
                element.DestroyElement();
            elementsList.Clear();
            TogglePanel(false);
        }
    }
}