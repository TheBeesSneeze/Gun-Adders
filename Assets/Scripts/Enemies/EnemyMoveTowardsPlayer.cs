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
    [SerializeField] private float groundFriction = 0.3f;
    protected override void Move(float speed)
    {
        Vector3 direction = target.position - transform.position;
        //direction.y = 0;
        direction.Normalize();
        direction *= speed;
        direction.y = 0f;
        rb.AddForce(direction);
        var friction = -rb.velocity * groundFriction;
        friction.y = 0f;
        rb.AddForce(friction);
        //rb.AddForce(direction * speed);
    }

}
