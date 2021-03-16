using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterState : MonoBehaviour
{
    /* --- Debug --- */
    private string DebugTag = "[DungeonCrawler CharacterState]: ";
    private bool DEBUG_init = false;


    /* --- Components --- */

    // Colliders
    public Collider2D hitbox;
    public Collider2D hull;

    // Overhead UI
    public Slider overheadHealthBar;
    public Slider overheadMana;
    public int overheadLevel;

    // Mouse UI
    public SpriteRenderer highlight;
    public SpriteRenderer selected;

    // Sprites
    public SpriteRenderer spriteRenderer;
    public Sprite portrait;

    // Character Controls
    public bool isControllable;
    public CharacterMovement characterMovement;


    /* --- Internal Variables --- */

    // Health
    [HideInInspector] public float maxHealth = 1f;
    [HideInInspector] public float health = 1f;

    // States
    [HideInInspector] public bool isDead = false;

    // Depth
    [HideInInspector] public float depth = 0;


    /* --- Unity Methods --- */
    void Start()
    {
        if (DEBUG_init) { print(DebugTag + "Activated for " + gameObject.name); }
    }

    void Update()
    {
        MoveFlag();
    }

    void FixedUpdate()
    {
    }

    public virtual void OnMouseDown()
    {
        Select();
    }

    public virtual void OnMouseOver()
    {
        Highlight(true);
    }

    public virtual void OnMouseExit()
    {
        Highlight(false);
    }


    /* --- Methods --- */
    public virtual void Select()
    {
    }

    public void Highlight(bool isHover)
    {
    }

    void MoveFlag()
    {
        characterMovement.horizontalMove = Input.GetAxisRaw("Horizontal");
        characterMovement.verticalMove = Input.GetAxisRaw("Vertical");
    }
}
