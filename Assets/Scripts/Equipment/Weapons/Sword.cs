using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : Weapon
{

    /* --- Overridden Variables ---*/
    public Sword()
    {
        backSwingTime = 0.4f;
        swingTime = 0.3f;
        resetTime = 0.4f;

        backSwingAngle = 270f;
        swingAngle = -200f;
    }

    /* --- Additional Variables --- */

    /* --- Overridden Unity Methods --- */

    /* --- Overridden Methods --- */
    public override void Attack()
    {
        if (isAttacking)
        {
            Slash();
        }
    }

    /* --- Additional Methods --- */

    void Slash()
    {
        float rotationFactor = Time.deltaTime * transform.right.x;
        if (isBackSwinging)
        {
            transform.RotateAround(handle.position, Vector3.forward, rotationFactor * backSwingAngleRate);
        }
        else if (isSwinging)
        {
            transform.RotateAround(handle.position, Vector3.forward, rotationFactor * swingAngleRate);
        }
        else if (isResetting)
        {
            transform.RotateAround(handle.position, Vector3.forward, rotationFactor * resetAngleRate);
        }
    }
}
