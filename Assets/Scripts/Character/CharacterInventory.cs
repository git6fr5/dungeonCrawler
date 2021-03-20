using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterInventory : MonoBehaviour
{
    /* --- Debug --- */
    private string DebugTag = "[DungeonCrawler CharacterState]: ";
    private bool DEBUG_init = false;


    /* --- Components --- */
    public GameObject inventory;
    public HUDInventory hudInventory;


    /* --- Internal Variables --- */

    // Weapons
    [HideInInspector] public Weapon equippedWeapon;
    [HideInInspector] public Weapon[] weapons = new Weapon[] { null, null, null };

    // Vials
    [HideInInspector] public Vial equippedVial;
    [HideInInspector] public Vial[] redVials = new Vial[] { null, null, null };
    [HideInInspector] public Vial[] blueVials = new Vial[] { null, null, null };
    [HideInInspector] public Vial[] greenVials = new Vial[] { null, null, null };

    // Tags
    private string weaponTag = "Weapon";
    private string vialTag = "Vial";

    /* --- Unity Methods --- */
    void Start()
    {
        if (DEBUG_init) { print(DebugTag + "Activated for " + gameObject.name); }
    }

    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D collider2D)
    {
        print("entering trigger");
        CheckCollect(collider2D);
    }

    /* --- Methods --- */
    void CheckCollect(Collider2D collider2D)
    {
        GameObject _object = collider2D.gameObject;
        if (_object.tag == weaponTag && _object.GetComponent<Weapon>())
        {
            CollectWeapon(_object.GetComponent<Weapon>());
        }
        else if (_object.tag == vialTag && _object.GetComponent<Vial>())
        {
            CollectVial(_object.GetComponent<Vial>());
        }
    }

    void CollectWeapon(Weapon weapon)
    {
        print("collecting");

        if (!weapon.isCollectible) { return; }

        bool addedToInventory = false;
        for (int i = 0; i < weapons.Length; i++)
        {
            if (!weapons[i])
            {
                weapons[i] = weapon;
                addedToInventory = true;
                break;
            }
        }
        if (!addedToInventory) { print("Inventory full"); return; }

        weapon.isCollectible = false;
        weapon.hitBox.isTrigger = true;
        weapon.transform.parent = inventory.transform;
        weapon.gameObject.SetActive(false);
        if (!equippedWeapon)
        {
            Equip(weapon);
        }
        hudInventory.UpdateInventory();
    }

    void CollectVial(Vial vial)
    {

    }

    void Equip(Weapon weapon)
    {
        print("equipping");
        equippedWeapon = weapon;
        weapon.gameObject.SetActive(true);
    }
}
