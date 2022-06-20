using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Inventory.SO;

public class MainCharacter : MonoBehaviour
{
    [SerializeField] InventorySO mainInventory;

    Rigidbody2D mcRigidBody2D;


    public void Awake()
    {
        mcRigidBody2D = GetComponent<Rigidbody2D>();
    }
    public InventorySO GetInventorySO() => mainInventory;
    public void Move(float direction)
    {
        mcRigidBody2D.velocity = new Vector2(direction * 6f, mcRigidBody2D.velocity.y);
    }
}
