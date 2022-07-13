using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

using Inventory.SO;
using Actor.SO;
using Inventory;
using System;

namespace Character
{
    public class CharacterManager : MonoBehaviour
    {
        public event Action<BattlerSO, GameObject>
            OnBattlerTriggerEnter;

        public InventorySO MainInventorySO { get; private set; }
        public void SetMainInventorySO(InventorySO inventory)
        {
            MainInventorySO = inventory;
        }
        [SerializeField] private ActorSO mainActorSO;
        [SerializeField] private Rigidbody2D mainRB2D;
        [SerializeField] private Animator mainAnimator;

        public EquipmentItem MainWeapon { get; private set; }
        public EquipmentItem SecondaryWeapon { get; private set; }

        private float lastX = 1;
        private float lastY = 0;

        private const float KNOCKBACK_STRENGTH = 20f;
        private const float KNOCKBACK_DELAY = 0.1f;

        [SerializeField] private Transform mainWeaponSlot;
        [SerializeField] private EquipItem equipItemPrefab;

        public Transform WeaponCircleOrigin;
        public float WeaponCircleRadius;

        public Vector2 MoveInput { get; set; }
        public bool IsWalking { get; set; }
        public bool IsAttacking { get; set; }
        public bool IsKnockedback { get; set; }


        public void Start()
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
            if(!IsAttacking && !IsKnockedback)
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
                        MainWeapon = new EquipmentItem(EquipmentItem.SlotType.WEAPON_MAIN);
                    }
                    else
                    {
                        DeleteChildObjects(mainWeaponSlot);
                        MainWeapon = item;
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
            item.EquipmentItem = MainWeapon;
            item.SetSprite(item.EquipmentItem.item.ItemImage);
        }
        private void DeleteChildObjects(Transform parent)
        {
            foreach (Transform child in parent)
                Destroy(child.gameObject);
        }
        private EquipItem CreateEquipItem() => Instantiate(equipItemPrefab, Vector3.zero, Quaternion.identity);
        #endregion




        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Vector3 position = WeaponCircleOrigin == null ? Vector3.zero : WeaponCircleOrigin.position;
            Gizmos.DrawWireSphere(position, WeaponCircleRadius);
        }
        public void DetectColliders() // Is called in animation frame.
        {
            foreach(Collider2D collider in Physics2D.OverlapCircleAll(WeaponCircleOrigin.position,WeaponCircleRadius))
            {
                if (collider.isTrigger)
                    continue;
                else
                {
                    ActorManager actor;
                    if (actor = collider.GetComponent<ActorManager>())
                    {
                        actor.GetHit(this.gameObject.transform.position);
                    }
                }
            }
        }
        public void OnCollisionEnter2D(Collision2D collision)
        {
            if(collision.gameObject.CompareTag("Enemy"))
            {
                DoKnockback(collision.gameObject.transform.position);
            }
        }
        private void DoKnockback(Vector2 originPos)
        {
            StopAllCoroutines();
            IsKnockedback = true;
            Vector2 direction = ((Vector2)gameObject.transform.localPosition - originPos).normalized;
            mainRB2D.velocity = direction * KNOCKBACK_STRENGTH;
            StartCoroutine(StopKnockback(KNOCKBACK_DELAY));
        }
        IEnumerator StopKnockback(float delay)
        {
            yield return new WaitForSeconds(delay);
            IsKnockedback = false;
            mainRB2D.velocity = Vector2.zero;
        }
        public void OnTriggerEnter2D(Collider2D collision)
        {
            if(collision.CompareTag("Battler"))
            {
                BattlerManager mainBattler;
                if(mainBattler = collision.GetComponent<BattlerManager>())
                {
                    OnBattlerTriggerEnter?.Invoke(mainBattler.GetBattlerSO(),collision.gameObject);
                }
                Debug.Log("Battle Should Start");
            }
        }






        public ActorSO GetActorSO()
        {
            return mainActorSO;
        }

        public InventorySO GetInventorySO() => MainInventorySO;

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