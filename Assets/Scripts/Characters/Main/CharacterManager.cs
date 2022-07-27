using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

using Inventory.SO;
using Actor;
using Actor.SO;
using Inventory;
using System;
using Managers;
using Mechanics;

namespace Character
{
    public class CharacterManager : MonoBehaviour
    {
        public static CharacterManager Instance;

        public event Action<BattlerSO, GameObject>
            OnBattlerTriggerEnter;

        public InventorySO MainInventorySO { get; private set; }
        public void SetMainInventorySO(InventorySO inventory)
        {
            MainInventorySO = inventory;
        }
        //[SerializeField] private ActorSO mainActorSO;
        [SerializeField] private Rigidbody2D mainRB2D;
        [SerializeField] private Animator mainAnimator;

        [SerializeField] public MainActor ActorParams = new MainActor();

        public EquipmentItem MainWeapon { get; private set; }
        public EquipmentItem SecondaryWeapon { get; private set; }

        private float lastX = 1;
        private float lastY = 0;

        private const float KNOCKBACK_STRENGTH = 20f;
        private const float KNOCKBACK_DELAY = 0.1f;

        [SerializeField] public Collider2D currentChestCollider { get; private set; }

        [SerializeField] private Transform mainWeaponSlot;
        [SerializeField] private EquipItem equipItemPrefab;

        public Transform WeaponCircleOrigin;
        public float WeaponCircleRadius;

        public Vector2 MoveInput { get; set; }
        public bool IsWalking { get; set; }
        public bool IsAttacking { get; set; }
        public bool IsKnockedback { get; set; }
        public bool IsAtSavePlace { get; set; }
        public bool IsAtChest { get; set; }
        public bool SaveNotificationShowed { get; set; }
        public bool ChestNotificationShowed { get; set; }

        private void Awake()
        {
            if (Instance != null && Instance != this)
                Destroy(this);
            else
            {
                Instance = this;
                DontDestroyOnLoad(this);
            }
        }
        private void Start()
        {
            UpdateAnimatorMovementFloat(new Vector2((int)MoveInput.x, (int)MoveInput.y));
        }
        private void FixedUpdate()
        {
            if (GameManager.Instance.GameState == GameManager.GameStates.PLAYING)
                if (IsWalking)
                {
                    UpdateAnimatorMovementFloat(new Vector2((int)MoveInput.x, (int)MoveInput.y));
                    Move(MoveInput);
                }
        }
        private void Update()
        {
            if (GameManager.Instance.GameState == GameManager.GameStates.PLAYING)
                UpdateCharacterState();
        }







        #region CharacterInputHandler
        public void Move(Vector2 moveInput)
        {
            mainRB2D.MovePosition(mainRB2D.position + 8f * Time.fixedDeltaTime * moveInput.normalized);
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
            if (!IsAttacking && !IsKnockedback)
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
                    if (item.IsEmpty)
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
        private float GetScalingBonus(string scaleType)
        {
            switch (scaleType)
            {
                case "Strength":
                    return ActorParams.mainStrength.ScaleBonus;
                default:
                    return 0;
            }
        }
        #endregion




        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Vector3 position = WeaponCircleOrigin == null ? Vector3.zero : WeaponCircleOrigin.position;
            Gizmos.DrawWireSphere(position, WeaponCircleRadius);
        }
        public void DetectColliders() // Is called in animation frame.
        {
            foreach (Collider2D collider in Physics2D.OverlapCircleAll(WeaponCircleOrigin.position, WeaponCircleRadius))
            {
                if (collider.isTrigger)
                    continue;
                else
                {
                    ActorManager actor;
                    if (actor = collider.GetComponent<ActorManager>())
                    {
                        if (MainWeapon.item is WeaponSO weaponSO)
                        {
                            foreach (var modifier in MainWeapon.item.itemParameters.weaponModifiers)
                            {
                                modifier.Modifier.ApplyModifier(actor, modifier.Value + (int)(modifier.Value * GetScalingBonus(weaponSO.scaleType.ToString())));
                            }
                            actor.GetHit(this.gameObject.transform.position);
                        }
                    }
                }
            }
        }
        public void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Enemy"))
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
            if (collision.CompareTag("Battler"))
            {
                BattlerManager mainBattler;
                if (mainBattler = collision.GetComponent<BattlerManager>())
                {
                    OnBattlerTriggerEnter?.Invoke(mainBattler.GetBattlerSO(), collision.gameObject);
                }
                Debug.Log("Battle Should Start");
            }
            if (collision.CompareTag("SavePlace"))
            {
                if (!IsAtSavePlace)
                {
                    IsAtSavePlace = true;
                    if (!SaveNotificationShowed)
                    {
                        GameManager.Instance.ThrowNotification(Inventory.UI.UINotifications.Notifications.CAN_INTERACT_WITH_OBJECT);
                        SaveNotificationShowed = true;
                    }
                }
            }
            if (collision.CompareTag("TreasureChest"))
            {
                if (!IsAtChest)
                {
                    IsAtChest = true;
                    if (!ChestNotificationShowed)
                    {
                        GameManager.Instance.ThrowNotification(Inventory.UI.UINotifications.Notifications.CHEST_NEARBY);
                        ChestNotificationShowed = true;
                    }
                    currentChestCollider = collision;
                }
            }
        }
        public void UseTreasureChest(Collider2D collision)
        {
            TreasureChest Chest = collision.GetComponentInParent<TreasureChest>();
            if (!Chest.IsOpened)
            {
                if (!Chest.WasLooted)
                {
                    Chest.WasLooted = true;

                    Debug.Log("First Time Opening, Generating Loot");
                    Chest.SetOpenedState(true);
                    InventoryController.Instance.ShowLootPanel(Chest.GetLootItems(1));
                }
                else
                {
                    Debug.Log("Was looted Already, Showing old content");
                    Chest.SetOpenedState(true);
                    InventoryController.Instance.ShowLootPanel(Chest.GetLootItems(1));
                }
            }
            else
            {
                Debug.Log("Closing Loot Panel");
                Chest.SetOpenedState(false);
                InventoryController.Instance.HideLootPanel();
            }
        }
        public void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.CompareTag("SavePlace"))
            {
                GameManager.Instance.ToggleSavePlacePanel(false);
                IsAtSavePlace = false;
                SaveNotificationShowed = false;
            }
            if (collision.CompareTag("TreasureChest"))
            {
                TreasureChest Chest = collision.GetComponentInParent<TreasureChest>();
                if (Chest.IsOpened)
                {
                    Debug.Log("Closing Loot Panel");
                    Chest.SetOpenedState(false);
                    InventoryController.Instance.HideLootPanel();
                }
                ChestNotificationShowed = false;
                IsAtChest = false;
            }
        }






        //public ActorSO GetActorSO()
        //{
        //    return mainActorSO;
        //}

        //public InventorySO GetInventorySO() => MainInventorySO;

        //public void Test()
        //{
        //mainActorSO.SetBaseEndurance(3);
        //mainActorSO.ChangeEndurance(5);
        //mainActorSO.perksList[0].UsePerk();
        //foreach(var perk in mainActorSO.perksList)
        //{
        //    Debug.Log("HAHAHAH");
        //}

        //this was not commented;
        //mainActorSO.AddPerk(mainActorSO.perkManager.Agility_FastRunner());
        //}
    }
}