using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

using Inventory.SO;
using Actor.SO;

namespace Character
{
    public class CharacterManager : MonoBehaviour
    {
        [Header("Main SO")]
        [SerializeField] public InventorySO mainInventorySO;
        [SerializeField] private ActorSO mainActorSO;

        [SerializeField] private Rigidbody2D mainRB2D;
        [SerializeField] private Animator mainAnimator;

        public Vector2 MoveInput { get; set; }
        public bool IsWalking { get; set; }


        private void FixedUpdate()
        {
            if (IsWalking)
            {
                if (Mathf.Abs(MoveInput.x) > 0) 
                {
                    UpdateAnimatorMovementFloat(MoveInput.x, 0);
                    Move(new Vector2(MoveInput.x, 0));
                }
                else
                {
                    UpdateAnimatorMovementFloat(0, MoveInput.y);
                    Move(new Vector2(0, MoveInput.y));
                }
            }
        }
        private void Update()
        {
            UpdateCharacterState();
        }


        #region CharacterInputHandler
        public void Move(Vector2 moveInput)
        {
            mainRB2D.MovePosition(mainRB2D.position +  8f * Time.fixedDeltaTime * moveInput.normalized);
        }
        #endregion


        #region AnimationControllers
        private void UpdateCharacterState()
        {
            IsWalking = (Mathf.Abs(MoveInput.x) + Mathf.Abs(MoveInput.y)) > 0;
            mainAnimator.SetBool("isWalking", IsWalking);
        }
        private void UpdateAnimatorMovementFloat(float X, float Y)
        {
            mainAnimator.SetFloat("XInput", X);
            mainAnimator.SetFloat("YInput", Y);
        }
        #endregion

        public ActorSO GetActorSO()
        {
            return mainActorSO;
        }
        public void Awake()
        {
            Test();
        }
        public InventorySO GetInventorySO() => mainInventorySO;

        public void Test()
        {
            //mainActorSO.SetBaseEndurance(3);
            //mainActorSO.ChangeEndurance(5);
            //mainActorSO.perksList[0].UsePerk();
            //foreach(var perk in mainActorSO.perksList)
            //{
            //    Debug.Log("HAHAHAH");
            //}
            mainActorSO.AddPerk(mainActorSO.perkManager.Agility_FastRunner());
        }
    }
}