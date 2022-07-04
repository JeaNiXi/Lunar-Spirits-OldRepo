using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

using Inventory.SO;
using Actor.SO;
using Inventory;

namespace Character
{
    public class CharacterManager : MonoBehaviour
    {
        [Header("Main SO")]
        [SerializeField] public InventorySO mainInventorySO;
        [SerializeField] private ActorSO mainActorSO;

        [SerializeField] private Rigidbody2D mainRB2D;
        [SerializeField] private Animator mainAnimator;

        private EquipmentItem mainWeapon;
        private EquipmentItem secondaryWeapon;

        private float lastX = 1;
        private float lastY = 0;

        [SerializeField] private Transform mainWeaponSlot;
        [SerializeField] private EquipItem equipItemPrefab;

        public Vector2 MoveInput { get; set; }
        public bool IsWalking { get; set; }
        public bool IsAttacking { get; set; }
        public void Awake()
        {
            UpdateAnimatorMovementFloat(new Vector2((int)MoveInput.x, (int)MoveInput.y));
        }
        private void FixedUpdate()
        {
            if (IsWalking)
            {
                UpdateAnimatorMovementFloat(new Vector2((int)MoveInput.x, (int)MoveInput.y));
                Move(MoveInput);
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
        public void StartAttack()
        {
            IsAttacking = true;
            UpdateAttackState();
        }
        public void StopAttack()
        {
            IsAttacking = false;
            UpdateAttackState();
        }
        #endregion


        #region AnimationControllers
        private void UpdateCharacterState()
        {
            if(!IsAttacking)
            {
                IsWalking = (Mathf.Abs(MoveInput.x) + Mathf.Abs(MoveInput.y)) > 0;
                mainAnimator.SetBool("isWalking", IsWalking);
            }
            else
            {
                IsWalking = false;
            }
        }
        private void UpdateAnimatorMovementFloat(Vector2 moveInput)
        {
            if (moveInput.x == 0 && moveInput.y == 0) 
            {
                SetUpMoveAnim(lastX, lastY);
            }
            else
            {
                SetUpMoveAnim(moveInput.x, moveInput.y);
            }
        }
        private void SetUpMoveAnim(float X, float Y)
        {
            mainAnimator.SetFloat("XInput", X);
            mainAnimator.SetFloat("YInput", Y);
            lastX = X;
            lastY = Y;
        }
        private void UpdateAttackState()
        {
            mainAnimator.SetBool("isAttacking", IsAttacking);
        }
        #endregion

        #region EquipmentHandler
        public void SetUpEquipment(List<EquipmentItem> equipmentList)
        {
            foreach (EquipmentItem item in equipmentList)
            {
                if (item.slotType == EquipmentItem.SlotType.WEAPON_MAIN)
                {
                    if(item.IsEmpty)
                    {
                        DeleteChildObjects(mainWeaponSlot);
                        mainWeapon = new EquipmentItem(EquipmentItem.SlotType.WEAPON_MAIN);
                    }
                    else
                    {
                        DeleteChildObjects(mainWeaponSlot);
                        mainWeapon = item;
                        DrawMainWeapon();
                    }    
                }
                //if (item.slotType == EquipmentItem.SlotType.WEAPON_SECONDARY)
                //    secondaryWeapon = item;
            }
        }
        private void DrawMainWeapon()
        {
            EquipItem item = CreateEquipItem();
            item.transform.SetParent(mainWeaponSlot);
            item.transform.localPosition = Vector3.zero;
            item.transform.localRotation = Quaternion.identity;
            item.transform.localScale = Vector3.one;
            item.EquipmentItem = mainWeapon;
            item.SetSprite(item.EquipmentItem.item.ItemImage);
        }
        private void DeleteChildObjects(Transform parent)
        {
            foreach (Transform child in parent)
                Destroy(child.gameObject);
        }
        private EquipItem CreateEquipItem() => Instantiate(equipItemPrefab, Vector3.zero, Quaternion.identity);
        #endregion





        public ActorSO GetActorSO()
        {
            return mainActorSO;
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