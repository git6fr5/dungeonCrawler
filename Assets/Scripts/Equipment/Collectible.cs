using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    /* --- Debug --- */
    private string DebugTag = "[DungeonCrawler Gem]: ";
    private bool DEBUG_init = false;


    /* --- Components --- */
    public Collider2D hitBox;
    public Sprite portrait;


    /* --- Internal Variables ---*/
    [HideInInspector] public bool isCollectible = true;

    /* --- Unity Methods --- */
    void Start()
    {
        if (DEBUG_init) { print(DebugTag + "Activated"); }
    }


    /* --- Methods --- */
}
