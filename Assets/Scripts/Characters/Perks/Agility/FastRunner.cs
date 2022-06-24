using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Fast Runner", menuName = "Character/Perks/Fast Runner")]

public class FastRunner : Agility
{
    public void SayHello()
    {
        Debug.Log("Hello");
    }

    public override void UsePerk()
    {
        Debug.Log("Hello");

    }
}
