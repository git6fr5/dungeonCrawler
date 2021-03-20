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

    // Inventories
    public GameObject weaponInventory;
    public GameObject vialInventory;
    public GameObject gemInventory;
    public GameObject oreInventory;

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

    // Gems
    [HideInInspector] public List<Gem> gems = new List<Gem>();
    [HideInInspector] public List<Ore> ores = new List<Ore>();


    // Tags
    private string weaponTag = "Weapon";
    private string vialTag = "Vial";
    private string gemTag = "Gem";
    private string oreTag = "Ore";

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
        //print("entering trigger");
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
        else if (_object.tag == gemTag && _object.GetComponent<Gem>())
        {
            CollectGem(_object.GetComponent<Gem>());
        }
        else if (_object.tag == oreTag && _object.GetComponent<Ore>())
        {
            CollectOre(_object.GetComponent<Ore>());
        }
    }

    void CollectWeapon(Weapon weapon)
    {
        //print("collecting weapon");

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
        weapon.transform.parent = weaponInventory.transform;
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
        //print("equipping");
        equippedWeapon = weapon;
        weapon.gameObject.SetActive(true);
        AdjustHandle(weapon);
    }

    void AdjustHandle(Weapon weapon)
    {
        weapon.transform.localPosition = -weapon.handle.localPosition;
    }

    void CollectGem(Gem gem)
    {
        //print("collecting gem");

        if (!gem.isCollectible) { return; }

        gems.Add(gem);

        gem.isCollectible = false;
        gem.hitBox.isTrigger = true;
        gem.transform.parent = gemInventory.transform;
        gem.gameObject.SetActive(false);

        hudInventory.UpdateInventory();
    }

    void CollectOre(Ore ore)
    {
        //print("collecting ore");

        if (!ore.isCollectible) { return; }

        ores.Add(ore);

        ore.isCollectible = false;
        ore.hitBox.isTrigger = true;
        ore.transform.parent = gemInventory.transform;
        ore.gameObject.SetActive(false);

        hudInventory.UpdateInventory();
    }
}
