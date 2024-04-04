using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClippingPlane : MonoBehaviour
{
    [Tooltip ("Where the player respawns if they clip through the map")]
    [SerializeField] private GameObject SpawnPoint;
    private void OnTriggerEnter(Collider collision)
    {
        PlayerBehaviour player = collision.gameObject.GetComponent<PlayerBehaviour>();
        EnemyType enemy = collision.gameObject.GetComponent<EnemyType>();
        if (player != null)
        {
            player.transform.position = SpawnPoint.transform.position;
            return;
        }
        if (enemy != null)
        {
            enemy.TakeDamage(10000000);
        }
    }
}
