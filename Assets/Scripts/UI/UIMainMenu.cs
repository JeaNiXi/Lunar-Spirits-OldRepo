using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Managers.UI
{
    public class UIMainMenu : MonoBehaviour
    {
        private readonly int introSceneIndex = 1;

        [SerializeField] private GameObject MainButtons;
        [SerializeField] private GameObject ChooseSlotButtons;
        [SerializeField] private GameObject SaveSlotExistsConfirmationPanel;
        [SerializeField] private GameObject SavePlacePanel;
        [SerializeField] private GameObject SaveInSlotChoosePanel;
        [SerializeField] private GameObject SaveSlotExistsConfirmationPanelInGame;

        private enum SaveSlots
        {
            Slot1,
            Slot2,
            Slot3,
            Slot4,
            Slot5,
            Slot6,
        }
        private SaveSlots SaveSlot = SaveSlots.Slot1;
        private readonly int[] SlotInt = { 1, 2, 3, 4, 5, 6, };

        private void Start()
        {
            CloseAllPanels();
            OpenMainMenu();
        }

        #region OnButtonClickEvents
        public void StartGameButtonPressed()
        {
            MainButtons.SetActive(false);
            StartNewGame();
        }
        public void LoadGameButtonPressed()
        {
            MainButtons.SetActive(false);
            ChooseSlotButtons.SetActive(true);
        }
        public void ChoosingFirstSlot()
        {
            SaveSlot = SaveSlots.Slot1;
            InitSlot(SaveSlot);
        }
        public void ChoosingSecondSlot()
        {
            SaveSlot = SaveSlots.Slot2;
            InitSlot(SaveSlot);
        }
        public void ChoosingThirdSlot()
        {
            SaveSlot = SaveSlots.Slot3;
            InitSlot(SaveSlot);
        }
        public void ChoosingFourthSlot()
        {
            SaveSlot = SaveSlots.Slot4;
            InitSlot(SaveSlot);
        }
        public void ChoosingFifthSlot()
        {
            SaveSlot = SaveSlots.Slot5;
            InitSlot(SaveSlot);
        }
        public void ChoosingSixthSlot()
        {
            SaveSlot = SaveSlots.Slot6;
            InitSlot(SaveSlot);
        }
        private void InitSlot(SaveSlots slot)
        {
            SetSaveSlotToManager(slot);
            CheckForGameLoad(slot);
        }
        public void ChoosingSaveToFirstSlot()
        {
            SaveSlot = SaveSlots.Slot1;
            InitSaveSlot(SaveSlot);
        }
        public void ChoosingSaveToSecondSlot()
        {
            SaveSlot = SaveSlots.Slot2;
            InitSaveSlot(SaveSlot);
        }
        public void ChoosingSaveToThirdSlot()
        {
            SaveSlot = SaveSlots.Slot3;
            InitSaveSlot(SaveSlot);
        }
        public void ChoosingSaveToFourthSlot()
        {
            SaveSlot = SaveSlots.Slot4;
            InitSaveSlot(SaveSlot);
        }
        public void ChoosingSaveToFifthSlot()
        {
            SaveSlot = SaveSlots.Slot5;
            InitSaveSlot(SaveSlot);
        }
        public void ChoosingSaveToSixthSlot()
        {
            SaveSlot = SaveSlots.Slot6;
            InitSaveSlot(SaveSlot);
        }
        private void InitSaveSlot(SaveSlots slot)
        {
            SetSaveSlotToManager(slot);
            CheckForSaveDirEmpty(slot);
        }
        public void NGSlotYButtonPressed()
        {
            SaveSlotExistsConfirmationPanel.SetActive(false);
            RemoveFolder(SaveSlot);
            StartNewGame();
        }
        public void NGSlotNButtonPressed()
        {
            SaveSlotExistsConfirmationPanel.SetActive(false);
            ChooseSlotButtons.SetActive(true);
        }
        public void SlotYButtonPressed()
        {
            SaveSlotExistsConfirmationPanelInGame.SetActive(false);
            GameManager.Instance.SaveGame();
        }
        public void SlotNButtonPressed()
        {
            SaveSlotExistsConfirmationPanelInGame.SetActive(false);
            SaveInSlotChoosePanel.SetActive(true);
        }
        public void ShowChooseSaveSlotPanel()
        {
            SavePlacePanel.SetActive(false);
            SaveInSlotChoosePanel.SetActive(true);
        }
        #endregion


        #region Utils
        private void OpenMainMenu() => MainButtons.SetActive(true);
        private void CloseAllPanels()
        {
            ChooseSlotButtons.SetActive(false);
            SaveSlotExistsConfirmationPanel.SetActive(false);
        }
        private void SetSaveSlotToManager(SaveSlots slot) =>
            GameManager.Instance.SetSaveSlotIndex(SlotInt[Convert.ToInt32(slot)]);
        private bool IsDirectoryEmpty(SaveSlots slot)
        {
            if(Directory.Exists(Application.persistentDataPath + "/" + slot.ToString()))
                return false;
            else
                return true;
        }
        private void RemoveFolder(SaveSlots slot)
        {
            string[] files = Directory.GetFiles(Application.persistentDataPath + "/" + slot.ToString());
            foreach (string file in files)
            {
                File.Delete(file);
            }
            Directory.Delete(Application.persistentDataPath + "/" + slot.ToString());
        }
        private void CheckForGameLoad(SaveSlots slot)
        {
            if (IsDirectoryEmpty(slot))
            {
                GameManager.Instance.ThrowNotification(Inventory.UI.UINotifications.Notifications.SAVE_SLOT_EMPTY);
            }
            else
            {
                ChooseSlotButtons.SetActive(false);
                GameManager.Instance.LoadGame();
            }
        }
        private void CheckForSaveDirEmpty(SaveSlots slot)
        {
            if (IsDirectoryEmpty(slot))
            {
                GameManager.Instance.SaveGame();
                SaveInSlotChoosePanel.SetActive(false);
            }
            else
            {
                SaveInSlotChoosePanel.SetActive(false);
                SaveSlotExistsConfirmationPanelInGame.SetActive(true);
            }
        }
        #endregion

        #region Managers
        private void StartNewGame()
        {
            GameManager.Instance.LoadNewGameIntro();
        }
        #endregion
    }
}