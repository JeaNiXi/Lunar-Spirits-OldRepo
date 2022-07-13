using Managers.SO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Managers
{
    public class BattleController : MonoBehaviour
    {
        [field: SerializeField] public GameManagerSO MainGMSO;

        private Vector3 bossPosition = new Vector3(10f, 0f, 0f);
        private void Awake()
        {
            Debug.Log("Wellcome TO Grande Battle!");
            Debug.Log("Battler is! " + MainGMSO.MainBattlerSO.BossName);
            Instantiate(MainGMSO.MainBattlerSO.BattlerPrefab, bossPosition, Quaternion.identity);
        }
        public void LoadLastLevel()
        {
            MainGMSO.SetDefeatedBattler(true);
            SceneManager.LoadScene(MainGMSO.LastSceneIndex);
        }
    }
}