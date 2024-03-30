/*******************************************************************************
* File Name :         ElectricityBullet.cs
* Author(s) :         Toby Schamberger
* Creation Date :     3/30/2024
*
* Brief Description : zaps nearby enemies
* 
* TODO visual
 *****************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace
{
    [CreateAssetMenu(fileName = "ElectricityBullet", menuName = "BulletEffects/Electricity")]
    public class ElectricityBullet : BulletEffect
    {
        public float ElectrocutionRange;
        public int MaxEnemiesToZap;

        public LayerMask OnlyEnemiesMask;

        public override void OnEnemyHit(EnemyType type, float damage)
        {
            Debug.Log("electricity hit");
            EnemyType[] closeEnemies = GetEnemiesInRange(type);

            if (closeEnemies.Length == 0) return;

            damage = damage * DamageMultiplier;

            for(int i=0; i< MaxEnemiesToZap && i<closeEnemies.Length; i++)
            {
                Debug.Log("electrocuting");
                Electrocute(closeEnemies[i], damage, type.transform.position);
            }
        }

        public override void OnHitOther(Vector3 point, float damage) { }

        private void Electrocute(EnemyType enemy, float damage, Vector3 origin)
        {
            enemy.TakeDamage(damage);
            Visualize(origin, enemy.transform.position);
        }

        private void Visualize(Vector3 origin, Vector3 target)
        {
            Debug.LogWarning("electricity visualization not done yet");
            Debug.DrawLine(origin, target, Color.yellow, 0.5f);
        }

        /// <summary>
        /// does NOT return enemy that got hit
        /// </summary>
        /// <param name="enemy"></param>
        /// <returns></returns>
        private EnemyType[] GetEnemiesInRange(EnemyType enemy)
        {
            //weird logic im sorry
            //i wanted to challenge myself to not use a list but i really should have tbh
            //wouldve been easier to read
            //sorry if this breaks and u gotta do shit to it later

            RaycastHit[] hit;
            hit = Physics.SphereCastAll(enemy.transform.position, MaxEnemiesToZap, Vector3.up, 0, OnlyEnemiesMask);

            int l = Mathf.Max(0, hit.Length - 1);
            EnemyType[] enemies = new EnemyType[l];

            int offset = 0;
            for (int i = 0; i < hit.Length; i++)
            {
                EnemyType e = hit[i].transform.GetComponent<EnemyType>();

                if (e == enemy)
                {
                    offset = -1;
                    continue;
                }

                enemies[i + offset] = e;
            }

            return enemies;

        }
    }
}
