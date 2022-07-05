using Actor.SO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Character
{
    public class ActorManager : MonoBehaviour
    {
        [SerializeField] private ActorSO actorSO;

        public void GetHit()
        {
            actorSO.GetHit();
        }
    }
}