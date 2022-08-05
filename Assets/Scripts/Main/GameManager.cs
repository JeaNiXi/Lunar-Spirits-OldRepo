using Actor.SO;
using Character;
using Helpers.SO;
using Inventory;
using Inventory.SO;
using Inventory.UI;
using Managers.SO;
using Managers.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Localization;
using UnityEngine.SceneManagement;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;

        private DirectoryInfo currentSaveDirectory;

        [field: SerializeField] public ModifiersListSO ModifiersListSO { get; set; }

        public LocalizedStringTable RarityTable = new LocalizedStringTable();

        [field: SerializeField] public GameManagerSO MainGMSO { get; private set; }
        [field: SerializeField] public SpawnPointsSO MainSpawnPoints { get; private set; }
        [field: SerializeField] public SaveSystem MainSaveSystem { get; private set; }
        [field: SerializeField] public CharacterManager MainCharacter { get; private set; }
        public enum GameStates
        {
            DISABLED,
            ENABLED,
            PAUSED,
            PLAYING,
        }
        public GameStates GameState = GameStates.DISABLED;


        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this);
            }
            else
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }

            //Application.targetFrameRate = 60;
            //Initialize();
            //MainCharacter.OnBattlerTriggerEnter += HandleBattleStart;
        }
        //debug.
        //private void Update()
        //{
        //    if (Keyboard.current.shiftKey.wasPressedThisFrame)
        //        Debug.Log(UnityEngine.Random.Range(0, 1));
        //}
        public void SetSaveSlotIndex(int index) => MainGMSO.SetSaveSlot(index);


        #region SceneManagement
        public void LoadNewGameIntro()
        {
            SceneManager.LoadScene(1);
            GameState = GameStates.PLAYING;
            InitializeGameCharacter();
        }
        #endregion



        #region GameManagement
        private void InitializeGameCharacter() // Only called when a New Game is Started.
        {
            MoveCharacterToSpawnPoint(MainSpawnPoints.IntroSceneV3);
        }
        private void MoveCharacterToSpawnPoint(Vector3 spawnPoint)
        {
            CharacterManager.Instance.transform.position = spawnPoint;
        }
        public void ToggleSavePlacePanel(bool value)
        {
            UICanvas.Instance.ToggleSavePlacePanel(value);
        }
        public void SaveGame()
        {
            SetSaveParams();

            SaveData data = new SaveData();
            data.SetMCPosV3(MainCharacter.transform.position);
            data.SetSceneIndex(SceneManager.GetActiveScene().buildIndex);
            var DataToSave = JsonUtility.ToJson(data);
            MainSaveSystem.SaveData(DataToSave);
        }

        //public void SaveGame()
        //{
        //    SaveData data = new SaveData();
        //    data.Add(MainCharacter.transform.position);
        //    var dataToSave = JsonUtility.ToJson(data);
        //    MainSaveSystem.SaveData(dataToSave);
        //}
        public void LoadGame()
        {
            SetSaveParams();

            string DataToLoad = "";
            DataToLoad = MainSaveSystem.LoadData();
            if (String.IsNullOrEmpty(DataToLoad) == false)
            {
                SaveData data = JsonUtility.FromJson<SaveData>(DataToLoad);
                SceneManager.LoadScene(data.SceneIndex);
                MainCharacter.transform.position = data.mainCharacterPosV3.GetV3();
                GameState = GameStates.PLAYING;
            }
            else
            {
                Debug.Log("SOMETHING WRONG WITH LOADING");
            }
        }
        //public void LoadGame()
        //{
        //    string dataToLoad = "";
        //    dataToLoad = MainSaveSystem.LoadData();
        //    if(String.IsNullOrEmpty(dataToLoad) == false)
        //    {
        //        SaveData data = JsonUtility.FromJson<SaveData>(dataToLoad);
        //        MainCharacter.transform.position = data.positionData[0].GetValue();
        //    }
        //    else
        //    {
        //        Debug.Log("LOADED DEFAULT SCENE");
        //    }
        //}
        private void SetSaveParams()
        {
            currentSaveDirectory = Directory.CreateDirectory(Application.persistentDataPath + "/" + GetSaveName(MainGMSO.SaveSlotIndex));
            var fullPath = Path.Combine(Path.Combine(Application.persistentDataPath, currentSaveDirectory.ToString(), MainGMSO.SaveSlotIndex.ToString()));
            MainSaveSystem.SetSaveIndex(MainGMSO.SaveSlotIndex);
            MainSaveSystem.SetSaveName(GetSaveName(MainGMSO.SaveSlotIndex));
            MainSaveSystem.SetSavePath(fullPath);
        }
        #endregion

        #region Utils
        public void ThrowNotification(UINotifications.Notifications notification)
        {
            InventoryController.Instance.UIMainInventory.ThrowNotification(notification);
        }
        private string GetSaveName(int index) => index switch
        {
            1 => "Slot1",
            2 => "Slot2",
            3 => "Slot3",
            4 => "Slot4",
            5 => "Slot5",
            6 => "Slot6",
            _ => "DEFAULT",
        };
        public int GetPlayerLevel() => 1;
        #endregion


        [Serializable]
        public class Vector3Serialization
        {
            public float x, y, z;

            public Vector3Serialization(Vector3 position)
            {
                this.x = position.x;
                this.y = position.y;
                this.z = position.z;
            }

            public Vector3 GetV3()
            {
                return new Vector3(x, y, z);
            }
        }
        [Serializable]
        public class SaveData
        {
            public List<Vector3Serialization> positionData;
            public Vector3Serialization mainCharacterPosV3;
            public int SceneIndex;

            public SaveData()
            {
                positionData = new List<Vector3Serialization>();
            }

            public void Add(Vector3 position)
            {
                positionData.Add(new Vector3Serialization(position));
            }
            public void SetSceneIndex(int index)
            {
                this.SceneIndex = index;
            }
            public void SetMCPosV3(Vector3 position)
            {
                mainCharacterPosV3 = new Vector3Serialization(position);
            }
        }

        //private bool IsInitialized = false;

        //[field: SerializeField] public InventoryController MainInveContPrefab { get; private set; }
        //[field: SerializeField] public UIMainInventory MainUIInveContPrefab { get; private set; }
        //[field: SerializeField] public InventorySO MainInventorySO { get; private set; }
        //[field: SerializeField] public CharacterManager MainCharacter { get; private set; }
        //[field: SerializeField] public InputController MainInputControllerPrefab { get; private set; }
        //[field: SerializeField] public SaveSystem MainSaveSystem { get; private set; }











        //private void Initialize()
        //{
        //    MainCharacter.SetMainInventorySO(MainInventorySO);
        //    InventoryController inventory = Instantiate(MainInveContPrefab, Vector3.zero, Quaternion.identity);
        //    inventory.SetUIMainInventory(MainUIInveContPrefab);
        //    inventory.SetMainInventorySO(MainInventorySO);
        //    inventory.SetMainCharacter(MainCharacter);
        //    InputController input = Instantiate(MainInputControllerPrefab, Vector3.zero, Quaternion.identity);
        //    input.SetMainCharacter(MainCharacter);
        //    input.SetMainInventoryController(inventory);
        //    if(MainGMSO.IsInitialized)
        //        LoadGame();
        //    else
        //        MainGMSO.SetSceneInitializedState(true);
        //}

        //private void HandleBattleStart(BattlerSO battler, GameObject battlerObject)
        //{
        //    MainGMSO.SetLastSceneIndex(SceneManager.GetActiveScene().buildIndex);
        //    MainGMSO.SetBattlerSO(battler);
        //    SaveGame();
        //    SceneManager.LoadScene("BattleScene");
        //}





        //private void OnApplicationQuit()
        //{
        //    MainGMSO.SetSceneInitializedState(false);
        //    MainGMSO.SetDefeatedBattler(false);
        //}
    }
}