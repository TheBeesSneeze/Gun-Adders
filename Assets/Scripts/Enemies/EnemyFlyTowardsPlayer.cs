using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//hey guys sky here
public class EnemyFlyTowardsPlayer : MonoBehaviour
{
    private Rigidbody rb;
    private PlayerBehaviour player;
    private Slider slider;

    [Tooltip("Speed at which enemy moves")]
    [SerializeField] private float Speed = 1;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        player = GameObject.FindObjectOfType<PlayerBehaviour>();
        slider = GetComponentInChildren<Slider>();
    }
    void Update()
    {
        Vector3 direction = player.transform.position - gameObject.transform.position;
        direction.Normalize();
        rb.velocity = direction * Speed;
        slider.transform.LookAt(player.transform.position);
    }

}
