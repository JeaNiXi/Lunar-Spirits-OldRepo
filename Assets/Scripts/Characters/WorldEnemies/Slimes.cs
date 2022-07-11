using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Character
{
    public class Slimes : ActorManager
    {
        private void Awake()
        {
            //FindPlayer();
            //InitActor();
        }
        private void Start()
        {
            FindPlayer();
            InitActor();

            CanJump = true;
            //IsJumping = true;
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