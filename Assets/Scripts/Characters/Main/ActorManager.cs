using Actor.SO;
using Inventory.SO;
using Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Character
{
    public class ActorManager : MonoBehaviour
    {
        [SerializeField] protected CharacterManager mainCharacter;
        [SerializeField] protected ActorSO actorSO;
        [SerializeField] protected Animator animator;
        [SerializeField] protected Rigidbody2D rigidBody;
        [SerializeField] protected CapsuleCollider2D colliderMain;

        private const string ANIM_JUMP_START = "StartJump";
        private const string ANIM_JUMP_TO_FALL = "JumpToFall";
        private const string ANIM_JUMP_END = "JumpLand";
        private const string ANIM_IDLE = "Idle";
        private const string ANIM_GET_HIT = "GetHit";
        private const string ANIM_DEATH = "Death";
        
        public enum STATE
        {
            ALIVE,
            DEAD,
        }
        public STATE ActorState = STATE.ALIVE;

        private const float KNOCKBACK_DELAY = .3f;

        private bool isPlayerInRange = false;
        private bool isKnockedBack = false;

        protected Vector2 JumpDirection;
        protected bool JumpDirectionFound;
        protected bool IsJumping { get; set; }
        protected bool CanJump { get; set; }
        protected void FindPlayer()
        {
            mainCharacter = GameManager.Instance.MainCharacter;
        }
        protected void InitActor()
        {
            actorSO.InitializeSO();
        }
        protected virtual void UpdateActorHP()
        {
            if(actorSO.CurrentHealth <= 0)
            {
                ActorState = STATE.DEAD;
                colliderMain.enabled = false;
                StopAllCoroutines();
                StopMovement();
                animator.Play(ANIM_DEATH);
            }
        }
        public void GetHit(Vector2 hitPosition)
        {
            Debug.Log("ENEMY HIT");
            actorSO.GetHit(GetMainWeapon());
            DoKnockback(hitPosition);
            PlayHitAnimation();
        }
        protected virtual void ChasePlayer()
        {
            if (CanJump || IsJumping)
            {
                if (!isKnockedBack)
                {
                    if (!IsJumping && CanJump)
                    {
                        PlayJumpAnimation();
                    }
                    if (IsJumping)
                    {
                        rigidBody.velocity = JumpDirection * actorSO.JumpStrength;
                    }
                }
            }
        }
        protected virtual void DoKnockback(Vector2 actorPosition)
        {
            StopAllCoroutines();
            isKnockedBack = true;
            Vector2 direction = ((Vector2)gameObject.transform.localPosition - actorPosition).normalized;
            rigidBody.velocity = direction * GetMainWeapon().item.KnockbackStrength;
            StartCoroutine(StopKnockback(KNOCKBACK_DELAY));
        }
        protected virtual void StopMovement()
        {
            StopAllCoroutines();
            StartCoroutine(WaitForJump(actorSO.JumpDelay));
            rigidBody.velocity = Vector2.zero;
            JumpDirectionFound = false;
        }
        private IEnumerator StopKnockback(float delay)
        {
            yield return new WaitForSeconds(delay);
            StopJumpAnimation(); // Also stops movement.
            //StartCoroutine(WaitForJump(actorSO.JumpDelay));
        }
        private IEnumerator WaitForJump(float delay)
        {
            yield return new WaitForSeconds(delay);
            CanJump = true;
        }


        private EquipmentItem GetMainWeapon()
        {
            return mainCharacter.MainWeapon;
        }
        protected void GetJumpDirection()
        {
            if(!JumpDirectionFound)
            {
                if(isPlayerInRange)
                {
                    Vector2 targetPos = mainCharacter.transform.position;
                    JumpDirection = (targetPos - (Vector2)gameObject.transform.localPosition).normalized;
                }
                else
                {
                    JumpDirection = new Vector2(Random.Range(-3f, 3f), Random.Range(-3f, 3f)).normalized;
                }
                JumpDirectionFound = true;
            }
        }
        protected virtual void PlayIdleAnimation()
        {
            animator.Play(ANIM_IDLE);
        }
        protected virtual void PlayHitAnimation()
        {
            animator.Play(ANIM_GET_HIT);
        }
        protected virtual void PlayJumpAnimation()
        {
            animator.Play(ANIM_JUMP_START);
        }
        protected virtual void StopJumpAnimation()
        {
            IsJumping = false;
            StopMovement();
        }
        protected virtual void SetTrueJumpState()
        {
            GetJumpDirection();
            IsJumping = true;
            CanJump = false;
        }
        protected virtual void StopKnockbackAnimation()
        {
            isKnockedBack = false;
        }
        protected void UpdateDirection()
        {
            if(!isKnockedBack)
            {
                //if (isPlayerInRange)
                //{
                //    float position = mainCharacter.gameObject.transform.position.x - gameObject.transform.position.x;
                //    if (position >= 2)
                //        gameObject.transform.localScale = new Vector3(1, 1, 1);
                //    if (position <= -2)
                //        gameObject.transform.localScale = new Vector3(-1, 1, 1);
                //}
                //else
                //{
                    float position = JumpDirection.x;
                    if (position > 0)
                        gameObject.transform.localScale = new Vector3(1, 1, 1);
                    if (position < 0)
                        gameObject.transform.localScale = new Vector3(-1, 1, 1);
                //}
            }
        }
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                DoKnockback(collision.gameObject.transform.position);
                PlayHitAnimation();
            }
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if(collision.CompareTag("Player"))
            {
                isPlayerInRange = true;
            }
        }
        private void OnTriggerStay2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                isPlayerInRange = true;
            }
        }
        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                isPlayerInRange = false;
            }
        }
        public void DestroyGameObject()
        {
            Destroy(gameObject);
        }
    }
}