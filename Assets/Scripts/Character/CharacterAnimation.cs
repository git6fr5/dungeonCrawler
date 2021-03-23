using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimation : MonoBehaviour
{
    /* --- Debug --- */
    private string DebugTag = "[DungeonCrawler CharacterAnimation]: ";
    private bool DEBUG_init = false;


    /* --- Components --- */

    // HUD
    public Transform head;
    public Transform body;
    public Transform legs;

    /* --- Internal Variables --- */
    protected float bobTurn = 0.1f;
    protected float bobAcceleration = 0.001f;

    // temp
    private Vector3 headOriginalPos;
    private Vector3 force;

    /* --- Unity Methods --- */
    void Start()
    {
        if (DEBUG_init) { print(DebugTag + "Activated for " + gameObject.name); }
        headOriginalPos = head.transform.localPosition;
        force = new Vector3(0, bobAcceleration, 0);
    }

    void Update()
    {
        HeadBob();
    }

    /* --- Methods --- */
    void HeadBob()
    {
        if (Mathf.Abs(head.transform.localPosition.y - headOriginalPos.y) > bobTurn)
        {
            bobAcceleration = -bobAcceleration;
            force = new Vector3(0, bobAcceleration, 0);
        }
        head.position = head.position + force;
    }

    void HeadSpasm()
    {
        if (Mathf.Abs(head.transform.localPosition.y - headOriginalPos.y) > bobTurn)
        {
            force = -force;
            bobAcceleration = -bobAcceleration;
        }
        force = force + new Vector3(0, bobAcceleration, 0);
        head.position = head.position + force;
    }
}
