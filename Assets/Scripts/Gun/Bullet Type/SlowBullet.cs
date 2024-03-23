using UnityEngine;

namespace DefaultNamespace
{
    [CreateAssetMenu(fileName = "SlowBullet", menuName = "BulletEffects/SlowBullet")]
    public class SlowBullet : BulletEffect
    {
        public override void OnEnemyHit(EnemyType type)
        {
            //slow down the enemy some how
        }
    }
}