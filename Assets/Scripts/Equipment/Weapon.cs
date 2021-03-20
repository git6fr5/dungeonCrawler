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
    public Transform handle;


    /* --- Internal Variables ---*/
    [HideInInspector] public int maxDurability = 1;
    [HideInInspector] public int durability;

    [HideInInspector] protected float backSwingTime = 0.5f;
    [HideInInspector] protected float swingTime = 0.5f;
    [HideInInspector] protected float resetTime = 0.5f;

    [HideInInspector] protected float backSwingAngle = 180f;
    [HideInInspector] protected float swingAngle = -180f;

    [HideInInspector] protected float backSwingAngleRate;
    [HideInInspector] protected float swingAngleRate;
    [HideInInspector] protected float resetAngleRate;

    [HideInInspector] public bool isCollectible = true;
    [HideInInspector] public bool isAttacking = false;
    [HideInInspector] public bool isBackSwinging = false;
    [HideInInspector] public bool isSwinging = false;
    [HideInInspector] public bool isResetting = false;

    [HideInInspector] public Vector3 originalPosition;
    [HideInInspector] public Quaternion originalRotation;

    /* --- Unity Methods --- */
    public virtual void Start()
    {
        if (DEBUG_init) { print(DebugTag + "Activated"); }
        SetStats();
        CalculateRotationRate();
    }

    public virtual void Update()
    {
        Attack();
    }

    public virtual void FixedUpdate()
    {
    }

    public virtual void OnTriggerEnter2D(Collider2D collider2D)
    {
    }

    public virtual void OnTriggerStay2D(Collider2D hitInfo)
    {
    }

    public virtual void OnTriggerExit2D(Collider2D hitInfo)
    {
    }

    /* --- Methods --- */
    public virtual void StartAttack()
    {
        isAttacking = true;
        isBackSwinging = true;
        originalPosition = transform.localPosition;
        originalRotation = transform.localRotation;
        StartCoroutine(BackSwing(backSwingTime));
    }

    public virtual void Attack()
    {
    }

    public virtual void SetStats()
    {
        durability = maxDurability;
    }

    protected IEnumerator BackSwing(float delay)
    {
        yield return new WaitForSeconds(delay);

        isBackSwinging = false;
        isSwinging = true;
        StartCoroutine(Swing(swingTime));

        yield return null;
    }

    protected IEnumerator Swing(float delay)
    {
        yield return new WaitForSeconds(delay);

        isSwinging = false;
        isResetting = true;
        StartCoroutine(Reset(resetTime));

        yield return null;
    }

    protected IEnumerator Reset(float delay)
    {
        yield return new WaitForSeconds(delay);

        isBackSwinging = false;
        isAttacking = false;

        transform.localPosition = originalPosition;
        transform.localRotation = originalRotation;

        yield return null;
    }

    protected void CalculateRotationRate()
    {
        backSwingAngleRate = backSwingAngle / backSwingTime;
        swingAngleRate = (swingAngle - backSwingAngle) / swingTime;
        resetAngleRate = -swingAngle / resetTime;
    }
}
