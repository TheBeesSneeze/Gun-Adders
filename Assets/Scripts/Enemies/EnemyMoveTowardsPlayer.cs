using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//hey guys sky here
public class EnemyMoveTowardsPlayer : MonoBehaviour
{
    private Rigidbody rb;
    private PlayerBehaviour player;

    [Tooltip("Speed at which enemy moves")]
    [SerializeField] private float Speed = 1;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        player = GameObject.FindObjectOfType<PlayerBehaviour>();
    }
    void Update()
    {
        Vector3 direction = player.transform.position - gameObject.transform.position;
        direction.y = 0;
        direction.Normalize();
        rb.velocity = direction * Speed;
    }

}
