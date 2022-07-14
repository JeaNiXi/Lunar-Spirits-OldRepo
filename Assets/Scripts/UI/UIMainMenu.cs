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

        private DirectoryInfo currentSaveDirectory;
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
            ChooseSlotButtons.SetActive(true);
        }
        public void LoadGameButtonPressed()
        {

        }
        public void ChoosingFirstSlot()
        {
            SaveSlot = SaveSlots.Slot1;
            SetSaveSlotToManager(SaveSlot);
            CheckForGameStart(SaveSlot);
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
        #endregion



        //DirectoryInfo savePath = Directory.CreateDirectory(Application.persistentDataPath + "/" + "Slot 1");
        //Debug.Log(savePath);
        //var fullPath = Path.Combine(Path.Combine(Application.persistentDataPath, savePath.ToString()), "Save_Index");
        //File.WriteAllText(fullPath, "hahahahah");




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
            Directory.Delete(Application.persistentDataPath + "/" + slot.ToString());
        }
        private void CheckForGameStart(SaveSlots slot)
        {
            if (IsDirectoryEmpty(slot))
            {
                ChooseSlotButtons.SetActive(false);
                StartNewGame();
            }
            else
            {
                ChooseSlotButtons.SetActive(false);
                SaveSlotExistsConfirmationPanel.SetActive(true);
            }
        }
        #endregion

        #region Managers
        private void StartNewGame()
        {
            GameManager.Instance.LoadNewGameIntro();
        }
        #endregion
        //private bool CheckForEmpty()
        //{
        //    if()
        //}
    }
}