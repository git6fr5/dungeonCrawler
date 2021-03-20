using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{

    /* --- Debug --- */
    private string DebugTag = "[DungeonCrawler Weapon]: ";
    private bool DEBUG_init = false;


    /* --- Components --- */
    public Collider2D hitBox;
    public Sprite portrait;


    /* --- Internal Variables ---*/
    [HideInInspector] public int maxDurability = 1;
    [HideInInspector] public int durability;

    [HideInInspector] public bool isCollectible = true;
    [HideInInspector] public bool isAttacking = false;

    /* --- Unity Methods --- */
    void Start()
    {
        if (DEBUG_init) { print(DebugTag + "Activated"); }
        durability = maxDurability;
    }

    void OnTriggerEnter2D(Collider2D collider2D)
    {
    }

    void OnTriggerStay2D(Collider2D hitInfo)
    {
    }

    void OnTriggerExit2D(Collider2D hitInfo)
    {
    }


    /* --- Methods --- */

}
