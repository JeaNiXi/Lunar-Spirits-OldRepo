using Character;
using Inventory;
using Inventory.SO;
using Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

public class B11B12AwaitEquip : MonoBehaviour
{
    [NonReorderable] public List<Slimes> slimesList = new List<Slimes>();

    public ItemSO weaponNameString;
    public GameObject EndingRef;
    private bool weaponFound;
    private bool slimesActive;

    public WeaponSO weaponToAdd;
    public PotionsSO potionToAdd;

    public void OnEnable()
    {
        InventoryController.Instance.AddItem(weaponToAdd, 1);
    }


    //public void Update()
    //{
    //    if (!weaponFound && !GameManager.Instance.MainCharacter.MainWeapon.IsEmpty)
    //        if (GameManager.Instance.MainCharacter.MainWeapon.item == weaponNameString)
    //        {
    //            weaponFound = true;
    //            Debug.Log("SLIMES INCOMING");
    //            ActivateSlimes();
    //        }
    //    if(slimesActive)
    //    {
    //        AwaitDefeated();
    //    }
    //}
    public void ActivateSlimes()
    {
        slimesActive = true;
        foreach(var item in slimesList)
        {
            item.gameObject.SetActive(true);
        }
    }
    public void AwaitDefeated()
    {

    }
}
