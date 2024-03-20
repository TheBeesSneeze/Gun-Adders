/*******************************************************************************
* File Name :         ProjectileType.cs
* Author(s) :         Toby Schamberger
* Creation Date :     9/7/2023
*
* Brief Description : Base class for attacks and projectiles.
* By default, attacks things by colliding with them.
* 
* Uses two different player attack types because of different knockback values
 *****************************************************************************/

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ProjectileType : MonoBehaviour
{
    [Header("Settings")]

    public float Damage;

    //[Tooltip("If characters will take damage when colliding with the effect")]
    //public bool DamageOnCollision = true;

    [Tooltip("If true, the effect will be destroyed by collisions that arent player/enemy/projectiles")]
    public bool DestroyedByWalls = true;

    [Tooltip("If true, gameobject will be destroyed after attacking something")]
    public bool DestroyedAfterAttack = true;

    [Tooltip("If true, this will destoy other attacks (that are marked as DestoryedAfterAttack)")]
    public bool DestroyOtherAttacks = false;

    [Tooltip("If true, will be destroyed collides with other attacks")]
    public bool GetDestroyedByOtherAttacks = false;

    //public bool CanBeParried = true;

    [Tooltip("If this # is less than 0, the attack will not be destroyed. Otherwise, destroys this gameobject")]
    public float DestroyAttackAfterSeconds = -1;

    public AudioClip SoundWhenDestroyed;

    public enum _AttackSource { General, Enemy, Player };
    //public enum PlayerAttack { NA, Primary, Secondary };

    public _AttackSource AttackSource;
    //public PlayerAttack PlayerAttackType;

    protected virtual void Start()
    {
        if (DestroyAttackAfterSeconds > 0)
            StartCoroutine(DestroyAfterSeconds());
    }

    protected virtual void OnPlayerCollision(Collider collision)
    {
        Debug.Log("player");
        if (AttackSource.Equals(_AttackSource.Enemy) || AttackSource.Equals(_AttackSource.General))
        {
            PlayerBehaviour player = collision.GetComponent<PlayerBehaviour>();

            player.TakeDamage(Damage);

            if (DestroyedAfterAttack)
                Destroy(this.gameObject);
        }
    }

    protected virtual void OnEnemyCollision(Collider collision)
    {
        if (AttackSource.Equals(_AttackSource.Player) || AttackSource.Equals(_AttackSource.General))
        {
            EnemyType enemy = collision.GetComponent<EnemyType>();

            enemy.TakeDamage(Damage);

            if (DestroyedAfterAttack)
                Destroy(this.gameObject);
        }
    }

    /// <summary>
    /// intended to be used in animation events
    /// </summary>
    public void DestroyAttack()
    {
        Destroy(gameObject);
    }

    protected virtual void OnProjectileCollision(ProjectileType attack)
    {
        

        if (gameObject.tag == "Player Attack" && attack.gameObject.tag == "Player Attack")
            return;

        bool destroyBoth = GetDestroyedByOtherAttacks && attack.GetDestroyedByOtherAttacks;
        bool destroyThis = destroyBoth || (attack.DestroyOtherAttacks && GetDestroyedByOtherAttacks);
        bool destroyThat = destroyBoth || (attack.GetDestroyedByOtherAttacks && DestroyOtherAttacks);

        if (destroyThat)
            Destroy(attack.gameObject);

        if (destroyThis)
            Destroy(this.gameObject);
    }

    public void OnTriggerEnter(Collider collision)
    {
        //string tag = collision.tag;
        Debug.Log("collision");

        if (collision.GetComponent<PlayerBehaviour>() != null)
        {
            OnPlayerCollision(collision);
            return;
        }

        if (collision.GetComponent<EnemyType>() != null)
        {
            OnEnemyCollision(collision);
            return;
        }

        //hit other attack
        if (collision.GetComponent<ProjectileType>() != null)
        {
            ProjectileType attack = collision.GetComponent<ProjectileType>();

            if (attack != null)
                OnProjectileCollision(attack);

            if(GetDestroyedByOtherAttacks)
                return;
        }

        if (DestroyedByWalls)
        {
            StopAllCoroutines();
            Destroy(this.gameObject);
            return;
        }
    }

    public void OnCollisionEnter(Collision collision)
    {
        OnTriggerEnter(collision.collider);
    }

    protected virtual IEnumerator DestroyAfterSeconds()
    {
        yield return new WaitForSeconds(DestroyAttackAfterSeconds);

        Destroy(this.gameObject);
    }

    protected void OnDestroy()
    {
        if (SoundWhenDestroyed != null)
        {
            Debug.Log("playing");
            AudioSource.PlayClipAtPoint(SoundWhenDestroyed, Camera.main.transform.position);
        }
    }

}
