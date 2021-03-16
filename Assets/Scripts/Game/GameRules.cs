using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameRules : MonoBehaviour
{
    /* --- Debug --- */
    private string DebugTag = "[DungeonCrawler GameRules]: ";
    private bool DEBUG_init = false;

    /* --- Components --- */

    /* --- Internal Variables --- */
    [HideInInspector] public static bool isPaused = false;

    /* --- Methods --- */
    void Start()
    {
        if (DEBUG_init) { print(DebugTag + "Activated"); }
    }

}
