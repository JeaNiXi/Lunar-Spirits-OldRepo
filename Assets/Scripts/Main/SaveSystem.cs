using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Managers
{
    public class SaveSystem : MonoBehaviour
    {
        [field: SerializeField] public string SaveName { get; private set; }
        public void SetSaveName(string name)
        {
            this.SaveName = name;
        }
        [field: SerializeField] public int SaveIndex { get; private set; }
        public void SetSaveIndex(int index)
        {
            this.SaveIndex = index;
        }
        [field: SerializeField] public string FullPath { get; private set; }
        public void SetSavePath(string path)
        {
            FullPath = path;
        }
        public void SaveData(string dataToSave)
        {
            if (WriteToFile(SaveName, dataToSave))
            {
                Debug.Log("SUCCESSFULLY SAVED DATA");
            }
        }
        private bool WriteToFile(string name, string content)
        {
            try
            {
                File.WriteAllText(FullPath, content);
                return true;
            }
            catch (Exception e)
            {
                Debug.LogError("ERROR SAVING TO A FILE " + e.Message);
            }
            return false;
        }

        public string LoadData()
        {
            string data = "";
            if (ReadFromFile(SaveName, out data))
            {
                Debug.Log("SUCCESSFULLY LOADED DATA");
            }
            return data;
        }

        private bool ReadFromFile(string name, out string content)
        {
            try
            {
                content = File.ReadAllText(FullPath);
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