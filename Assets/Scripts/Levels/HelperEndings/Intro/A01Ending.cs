using Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Levels
{
    public class A01Ending : MonoBehaviour
    {
        public void OnEnable()
        {
            GameManager.Instance.MainCharacter.GetComponent<Animator>().Play("StandingUp");
        }
    }
}