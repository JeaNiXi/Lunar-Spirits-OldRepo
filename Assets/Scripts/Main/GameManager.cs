using Actor.SO;
using Character;
using Inventory;
using Inventory.SO;
using Inventory.UI;
using Managers.SO;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;

        private bool IsInitialized = false;

        //[field: SerializeField] public GameManagerSO MainGMSO { get; private set; }
        //[field: SerializeField] public InventoryController MainInveContPrefab { get; private set; }
        //[field: SerializeField] public UIMainInventory MainUIInveContPrefab { get; private set; }
        //[field: SerializeField] public InventorySO MainInventorySO { get; private set; }
        //[field: SerializeField] public CharacterManager MainCharacter { get; private set; }
        //[field: SerializeField] public InputController MainInputControllerPrefab { get; private set; }
        //[field: SerializeField] public SaveSystem MainSaveSystem { get; private set; }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                DontDestroyOnLoad(gameObject);
                Instance = this;
            }
            //Initialize();
            //MainCharacter.OnBattlerTriggerEnter += HandleBattleStart;
        }
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

        //public void SaveGame()
        //{
        //    SaveData data = new SaveData();
        //    data.Add(MainCharacter.transform.position);
        //    var dataToSave = JsonUtility.ToJson(data);
        //    MainSaveSystem.SaveData(dataToSave);
        //}
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
        //[Serializable]
        //public class SaveData
        //{
        //    public List<Vector3Serialization> positionData;

        //    public SaveData()
        //    {
        //        positionData = new List<Vector3Serialization>();
        //    }

        //    public void Add(Vector3 position)
        //    {
        //        positionData.Add(new Vector3Serialization(position));
        //    }
        //}

        //[Serializable]
        //public class Vector3Serialization
        //{
        //    public float x, y, z;

        //    public Vector3Serialization(Vector3 position)
        //    {
        //        this.x = position.x;
        //        this.y = position.y;
        //        this.z = position.z;
        //    }

        //    public Vector3 GetValue()
        //    {
        //        return new Vector3(x, y, z);
        //    }
        //}
        //private void OnApplicationQuit()
        //{
        //    MainGMSO.SetSceneInitializedState(false);
        //    MainGMSO.SetDefeatedBattler(false);
        //}
    }
}