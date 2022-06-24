using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Actor.SO
{
    [CreateAssetMenu(fileName = "New Actor", menuName = "Base/Actor")]

    public class ActorSO : ScriptableObject
    {
        [field: SerializeField] public float Health;
    }
}