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
    public Slider healthSlider;
    public Slider manaSlider;
    public Text nameText;
    public Image portraitImage;
    public Text levelText;


    /* --- Internal Variables --- */
    [HideInInspector] public CharacterState currSelection = null;


    /* --- Unity Methods --- */
    void Start()
    {
        if (DEBUG_init) { print(DebugTag + "Activated"); }
        Select(playerState);
        SetHealth();
        SetMana();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            SelectNext();
        }

        UpdateHealth();
        UpdateMana();
    }


    /* --- Methods --- */
    public void Select(CharacterState characterState)
    {
        if (GameRules.isPaused) { return; }

        currSelection = characterState;
        nameText.text = currSelection.name;
        portraitImage.sprite = currSelection.portrait;
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
        healthSlider.maxValue = currSelection.maxHealth;
    }

    void UpdateHealth()
    {
        healthSlider.value = currSelection.health;
    }

    void SetMana()
    {
        manaSlider.maxValue = currSelection.maxMana;
    }

    void UpdateMana()
    {
        manaSlider.value = currSelection.mana;
    }

    void SetLevel()
    {
        levelText.text = currSelection.level.ToString();
    }

    void UpdateLevel()
    {
        levelText.text = currSelection.level.ToString();
    }

    public void Action()
    {
        if (GameRules.isPaused) { return; }
    }

    public void OpenPanel(GameObject _gameObject) 
    {
        _gameObject.SetActive(!_gameObject.activeSelf);
    }

    public void Pause()
    {
        GameRules.isPaused = !GameRules.isPaused;
        //hudPauseMenu.SetActive(GameRules.isPaused);
        //Time.timeScale = Convert.ToInt32(GameRules.isPaused);
    }
}