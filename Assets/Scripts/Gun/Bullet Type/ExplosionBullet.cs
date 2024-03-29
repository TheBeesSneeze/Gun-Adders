/*******************************************************************************
* File Name :         ExplosionBullet.cs
* Author(s) :         Toby Schamberger
* Creation Date :     3/25/2024
*
* Brief Description : Spawns a funny lil explosion guy when it blows up
 *****************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace
{
    [CreateAssetMenu(fileName = "ExplosionBullet", menuName = "BulletEffects/ExplosionBullet")]
    public class ExplosionBullet : BulletEffect
    {
        
        public GameObject ExplosionPrefab;
        public override void OnEnemyHit(EnemyType type)
        {
            GunController gun = GameObject.FindObjectOfType<GunController>();

            GameObject explosion = Instantiate(ExplosionPrefab, type.transform.position, ExplosionPrefab.transform.rotation);

            explosion.GetComponent<AttackType>().Damage = gun.defaultShootingMode.BulletDamage * DamageMultiplier;
        }

        public override void OnHitOther(Vector3 point)
        {
            GunController gun = GameObject.FindObjectOfType<GunController>();

            GameObject explosion = Instantiate(ExplosionPrefab, point, ExplosionPrefab.transform.rotation);

            explosion.GetComponent<AttackType>().Damage = gun.defaultShootingMode.BulletDamage * DamageMultiplier;
        }
    }
}
