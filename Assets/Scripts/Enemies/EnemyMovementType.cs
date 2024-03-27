/*******************************************************************************
 * File Name :         EnemyMovementType.cs
 * Author(s) :         Toby
 * Creation Date :     
 *
 * Brief Description : doesnt actually move. but does the raycast stuff
 *****************************************************************************/

using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class EnemyMovementType : MonoBehaviour
{
    [Tooltip("Speed at which enemy moves")]
    [SerializeField] protected float Speed = 1.0f;
    [SerializeField] protected float SlowedSpeed = 0.5f;
    [Tooltip("How far enemy can see")]
    [SerializeField] private float PlayerSightDistance=15; 
    [SerializeField] private bool RequirePlayerSightToMove=true;

    [ReadOnly] public bool MoveEnemy;

    protected Transform target;
    protected Rigidbody rb;
    protected Transform healthBar;
    protected EnemyType enemyType;

    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody>();
        target = GameObject.FindObjectOfType<PlayerBehaviour>().transform;
        healthBar = GetComponentInChildren<Slider>().transform;
        enemyType = GetComponent<EnemyType>();
    }

    private void Update()
    {
        RotateHealthBar();

        MoveEnemy = PlayerVisible(); //PlayerVisible returns true if RequirePlayerSightToMove = false

        if (!MoveEnemy) return;

        float speed = enemyType.slowed ? SlowedSpeed : Speed;

        Move(speed);
    }

    /// <summary>
    /// called in update
    /// </summary>
    protected virtual void Move(float speed)
    {
        throw new NotImplementedException();
    }

    private void RotateHealthBar()
    {
        if(healthBar != null)
            healthBar.transform.LookAt(target.position);
    }

    /// <summary>
    /// shoots raycast to look for player.
    /// returns true if RequirePlayerSightToMove = true
    /// </summary>
    public bool PlayerVisible()
    {
        if (!RequirePlayerSightToMove) return true;

        Vector3 direction = target.position - transform.position;
        Ray ray = new Ray(transform.position, direction);

        Debug.DrawLine(transform.position, transform.position + (direction * PlayerSightDistance), Color.white);

        RaycastHit hit;
        if(!Physics.Raycast(ray, out hit, PlayerSightDistance)) return false; //sees nothing

        //ok it saw something
        PlayerBehaviour pb = hit.transform.GetComponent<PlayerBehaviour>();

        return (pb != null);
    }
}
