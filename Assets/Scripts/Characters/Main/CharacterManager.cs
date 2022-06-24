using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Inventory.SO;
using Actor.SO;

namespace Character
{
    public class CharacterManager : MonoBehaviour
    {
        [Header("Main SO")]
        [SerializeField] InventorySO mainInventorySO;
        [SerializeField] ActorSO mainActorSO;


        Rigidbody2D mcRigidBody2D;


        public void Awake()
        {
            mcRigidBody2D = GetComponent<Rigidbody2D>();
        }
        public InventorySO GetInventorySO() => mainInventorySO;
        public void Move(float direction)
        {
            mcRigidBody2D.velocity = new Vector2(direction * 6f, mcRigidBody2D.velocity.y);
        }
        public void Test()
        {
           
        }
    }
}