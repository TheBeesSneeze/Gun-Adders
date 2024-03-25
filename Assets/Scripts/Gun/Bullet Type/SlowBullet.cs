using UnityEngine;

namespace DefaultNamespace
{
    [CreateAssetMenu(fileName = "SlowBullet", menuName = "BulletEffects/SlowBullet")]
    public class SlowBullet : BulletEffect
    {
        [field:SerializeField] public float SlowTime { get; private set; }= 2f;
        public override void OnEnemyHit(EnemyType type)
        {
            //slow down the enemy some how
            type.ApplySlow(SlowTime);
        }
    }
}