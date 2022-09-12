using Character;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HydroSlime : Slimes
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
