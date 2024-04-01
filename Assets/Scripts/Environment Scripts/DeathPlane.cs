using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathPlane : MonoBehaviour
{
    private void OnTriggerEnter(Collider collision)
    {
        PlayerBehaviour player = collision.gameObject.GetComponent<PlayerBehaviour>();
        EnemyType enemy = collision.gameObject.GetComponent<EnemyType>();
        if (player != null)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            return;
        }
        if (enemy != null)
        {
            enemy.TakeDamage(10000000);
            Debug.Log("enemy should be dying now");
        }
    }
}
