using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character3DMovement : MonoBehaviour
{
    /* --- Debug --- */
    private string DebugTag = "[DungeonCrawler Character3DMovement]: ";
    private bool DEBUG_init = false;


    /* --- Components --- */

    // Controller
    public CharacterController characterController;

    /* --- Internal Variables --- */

    // Inputs
    private float horizontalMove = 0f;
    private float verticalMove = 0f;

    // Controls
    private Vector3 velocity = Vector3.zero;
    private float turnSmoothTime = 0.1f;
    private float turnSmoothVelocity;
    private float speed = 5f;


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
        Move();
        Rotate();
    }


    /* --- Methods --- */
    void Move()
    {
        velocity = new Vector3(horizontalMove, 0, verticalMove).normalized * speed;
        characterController.Move(velocity * Time.fixedDeltaTime);
    }

    void Rotate()
    {
        float targetAngle = Mathf.Atan2(velocity.x, velocity.z) * Mathf.Rad2Deg;
        float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
        transform.rotation = Quaternion.Euler(0f, angle, 0f);
    }

    void MoveFlag()
    {
        horizontalMove = Input.GetAxisRaw("Horizontal");
        verticalMove = Input.GetAxisRaw("Vertical");
    }

}
