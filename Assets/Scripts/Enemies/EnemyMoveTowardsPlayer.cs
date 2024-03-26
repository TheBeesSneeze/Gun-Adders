using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//hey guys sky here
public class EnemyMoveTowardsPlayer : MonoBehaviour
{
    private Rigidbody rb;
    private PlayerBehaviour player;
    private Slider slider;

    [Tooltip("Speed at which enemy moves")]
    [SerializeField] private float Speed = 1;
    [SerializeField] private float SlowedSpeed = 0.5f;
    private EnemyType _enemyType;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        player = GameObject.FindObjectOfType<PlayerBehaviour>();
        slider = GetComponentInChildren<Slider>();
        _enemyType = GetComponentInChildren<EnemyType>();
    }
    void FixedUpdate()
    {
        Vector3 direction = player.transform.position - gameObject.transform.position;
        direction.y = 0;
        direction.Normalize();
        direction *= _enemyType.slowed ? SlowedSpeed : Speed;
        direction.y = rb.velocity.y;
        rb.velocity = direction;
        slider.transform.LookAt(player.transform.position);
    }

}
