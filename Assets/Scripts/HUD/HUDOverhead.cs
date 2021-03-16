using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDOverhead : MonoBehaviour
{
    /* --- Debug --- */
    private string DebugTag = "[DungeonCrawler HUDOverhead]: ";
    private bool DEBUG_init = false;


    /* --- Components --- */
    public CharacterState playerState;
    public Slider healthSlider;
    public Slider manaSlider;
    public Text levelText;


    /* --- Internal Variables --- */


    /* --- Unity Methods --- */
    void Start()
    {
        if (DEBUG_init) { print(DebugTag + "Activated"); }
        SetHealth();
        SetMana();
        SetHealth();
    }

    void Update()
    {
        UpdateHealth();
        UpdateMana();
        UpdateLevel();
    }


    /* --- Methods --- */
    void SetHealth()
    {
        healthSlider.maxValue = playerState.maxHealth;
    }

    void UpdateHealth()
    {
        healthSlider.value = playerState.health;
    }

    void SetMana()
    {
        manaSlider.maxValue = playerState.maxHealth;
    }

    void UpdateMana()
    {
        manaSlider.value = playerState.health;
    }

    void SetLevel()
    {
        levelText.text = playerState.level.ToString();
    }

    void UpdateLevel()
    {
        levelText.text = playerState.level.ToString();
    }
}
