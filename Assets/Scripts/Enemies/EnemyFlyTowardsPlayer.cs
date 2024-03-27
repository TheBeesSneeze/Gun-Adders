/*******************************************************************************
 * File Name :         EnemyFlyTowardsPlayer.cs
 * Author(s) :         Sky, Toby
 * Creation Date :     
 *
 * Brief Description : 
 *****************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//hey guys sky here
public class EnemyFlyTowardsPlayer : EnemyMovementType
{
    protected override void Move(float speed)
    {
        Vector3 direction = target.transform.position - gameObject.transform.position;
        direction.Normalize();
        direction *= speed;
        //rb.velocity = direction * Speed;
        rb.AddForce(direction);
    }

    private void LateUpdate()
    {
        if (MoveEnemy) return;

        rb.velocity = Vector3.Lerp(rb.velocity, Vector3.zero, Time.deltaTime);
    }

}
