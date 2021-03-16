using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{

    /* --- Debug --- */
    private string DebugTag = "[DungeonCrawler HUD]: ";
    private bool DEBUG_init = false;


    /* --- Components --- */
    public CharacterState playerState;


    /* --- Internal Variables --- */
    [HideInInspector] public CharacterState currSelection = null;


    /* --- Unity Methods --- */
    void Start()
    {
        if (DEBUG_init) { print(DebugTag + "Activated"); }
        SetHealth();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            SelectNext();
        }
        UpdateHealth();
    }


    /* --- Methods --- */
    public void Select(CharacterState characterState)
    {
        if (GameRules.isPaused) { return; }
    }

    void SelectNext()
    {
        if (GameRules.isPaused) { return; }
    }

    void Deselect()
    {
        if (GameRules.isPaused) { return; }
    }

    void SetHealth()
    {
        
    }

    void UpdateHealth()
    {

    }

    public void Action()
    {
        if (GameRules.isPaused) { return; }

    }

    public void Pause()
    {
        GameRules.isPaused = !GameRules.isPaused;
        //hudPauseMenu.SetActive(GameRules.isPaused);
        //Time.timeScale = Convert.ToInt32(GameRules.isPaused);
    }
}