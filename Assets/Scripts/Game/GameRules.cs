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
    [HideInInspector] public static int maxLevel = 5;
    [HideInInspector] public static int[] xpTable = new int[] { 1, 2, 3 };


    /* --- Methods --- */
    void Start()
    {
        if (DEBUG_init) { print(DebugTag + "Activated"); }
    }

}
