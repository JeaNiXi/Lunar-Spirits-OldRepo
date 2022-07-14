using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Helpers.SO
{
    [CreateAssetMenu(fileName = "SpawnPoints Collection", menuName = "Helpers/SpawnPointCollection")]

    public class SpawnPointsSO : ScriptableObject
    {
        [field: SerializeField] public Vector3 IntroSceneV3 { get; set; }
    }
}