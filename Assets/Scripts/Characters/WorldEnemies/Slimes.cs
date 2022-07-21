using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Character
{
    public class Slimes : ActorManager
    {
        private void Start()
        {
            ActorParams.InitActor();
            FindPlayer();
            //InitActor();

            CanJump = true;
            GetJumpDirection();
            PlayIdleAnimation();
        }
        private void Update()
        {
            if (ActorState == STATE.ALIVE)
            {
                ChasePlayer();
                UpdateDirection();
                UpdateActorHP();
            }
        }
    }
}