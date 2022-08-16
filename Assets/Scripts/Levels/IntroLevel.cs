using Character;
using Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Levels
{
    public class IntroLevel : MonoBehaviour
    {
        [SerializeField] public CharacterManager MainCharacter;

        private Animator cAnim;
        private void Start()
        {
            MainCharacter = GameManager.Instance.MainCharacter;
            cAnim = MainCharacter.GetComponent<Animator>();
            cAnim.Play("BeingDowned");

        }
    }
}