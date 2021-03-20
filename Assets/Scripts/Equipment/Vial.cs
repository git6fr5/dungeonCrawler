using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vial : MonoBehaviour
{
    /* --- Debug --- */
    private string DebugTag = "[DungeonCrawler Vial]: ";
    private bool DEBUG_init = false;


    /* --- Components --- */


    /* --- Internal Variables ---*/
    [HideInInspector] public int charges;

    /* --- Unity Methods --- */
    void Start()
    {
        if (DEBUG_init) { print(DebugTag + "Activated"); }
    }


    /* --- Methods --- */

}
