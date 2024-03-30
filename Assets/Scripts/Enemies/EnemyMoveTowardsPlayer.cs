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
public class EnemyMoveTowardsPlayer : EnemyMovementType
{
    protected override void Move(float speed)
    {
        Debug.Log("hey guys its me moving");
        Vector3 direction = target.position - transform.position;
        //direction.y = 0;
        direction.Normalize();
        direction *= speed;
        direction.y = rb.velocity.y;
        rb.velocity = direction;

        //rb.AddForce(direction * speed);
    }

}
