using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDInventory : MonoBehaviour
{
    /* --- Debug --- */
    private string DebugTag = "[DungeonCrawler HUDInventory]: ";
    private bool DEBUG_init = false;


    /* --- Components --- */
    public CharacterInventory playerInventory;
    public Sprite defaultImage;
    public GameObject equippedWeaponSlot;
    public GameObject[] weaponSlots = new GameObject[3] { null, null, null };


    /* --- Internal Variables --- */



    /* --- Unity Methods --- */
    void Start()
    {
        if (DEBUG_init) { print(DebugTag + "Activated"); }
    }

    /* --- Methods --- */
    public void UpdateInventory()
    {
        for (int i = 0; i < weaponSlots.Length; i++)
        {
            if (playerInventory.weapons[i])
            {
                Image weaponPortrait = weaponSlots[i].GetComponent<Image>();
                weaponPortrait.sprite = playerInventory.weapons[i].portrait;
            }
        }

        if (playerInventory.equippedWeapon)
        {
            Image weaponPortrait = equippedWeaponSlot.GetComponent<Image>();
            weaponPortrait.sprite = playerInventory.equippedWeapon.portrait;
        }
    }

}
