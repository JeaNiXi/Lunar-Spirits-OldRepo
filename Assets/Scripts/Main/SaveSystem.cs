using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Managers
{
    public class SaveSystem : MonoBehaviour
    {
        public string saveName = "SaveData_";
        [Range(1, 3)]
        public int saveDataIndex = 1;


        public void SaveData(string dataToSave)
        {
            if (WriteToFile(saveName + saveDataIndex, dataToSave))
            {
                Debug.Log("SUCCESSFULLY SAVED DATA");
            }
        }

        public string LoadData()
        {
            string data = "";
            if (ReadFromFile(saveName + saveDataIndex, out data))
            {
                Debug.Log("SUCCESSFULLY LOADED DATA");
            }
            return data;
        }

        private bool WriteToFile(string name, string content)
        {
            var fullPath = Path.Combine(Application.persistentDataPath, name);

            try
            {
                File.WriteAllText(fullPath, content);
                return true;
            }
            catch (Exception e)
            {
                Debug.LogError("ERROR SAVING TO A FILE " + e.Message);
            }
            return false;
        }

        private bool ReadFromFile(string name, out string content)
        {
            var fullPath = Path.Combine(Application.persistentDataPath, name);

            try
            {
                content = File.ReadAllText(fullPath);
                return true;
            }
            catch (Exception e)
            {
                Debug.LogError("ERROR WHEN LOADING THE FILE " + e.Message);
                content = "";
            }
            return false;
        }
    }
}