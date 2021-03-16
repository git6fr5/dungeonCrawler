using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{

    /* --- Debug --- */
    private string DebugTag = "[DungeonCrawler CharacterMovement]: ";
    private bool DEBUG_init = false;


    /* --- Components --- */
    public Rigidbody2D body;


    /* --- Internal Variables ---*/

    // Values
    [HideInInspector] public float baseSpeed = 5f;
    [HideInInspector] public float horizontalMove = 0f;
    [HideInInspector] public float verticalMove = 0f;

    // Controls
    private Vector3 velocity = Vector3.zero;
    private float movementSmoothing = 0.05f;
    private bool facingRight = true;
    private float speed = 0f;


    /* --- Unity Methods --- */
    void Start()
    {
        if (DEBUG_init) { print(DebugTag + "Activated for " + gameObject.name); }
        SetSpeed(baseSpeed);
    }

    public virtual void FixedUpdate()
    {
        Move();
    }


    /* --- Methods --- */
    void Move()
    {
        if (horizontalMove < 0 && facingRight) { Flip(); }
        else if (horizontalMove > 0 && !facingRight) { Flip(); }
        Vector3 targetVelocity = new Vector2(horizontalMove, verticalMove).normalized * speed;
        body.velocity = Vector3.SmoothDamp(body.velocity, targetVelocity, ref velocity, movementSmoothing);
    }

    void Flip()
    {
        facingRight = !facingRight;
        transform.Rotate(0f, 180f, 0f);
    }

    public void SetSpeed(float _speed)
    {
        speed = _speed;
    }
}