using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//created by alec

[CreateAssetMenu(menuName = "BulletEffects/WindBullet", fileName = "WindBullet")]
public class WindBullet : BulletEffect
{
    public float KnockBackForce = 10f;
    public float PlayerKnockBackForce = 10f;
    public override void OnEnemyHit(EnemyType type, float damage)
    {
        Vector3 normal = type.transform.position - PlayerControler.Instance.transform.position;
        if (type.RB != null)
        {
            type.RB.AddForce(normal.normalized * KnockBackForce, ForceMode.Impulse);
        }
        //PlayerControler.Instance.RB.AddForce(-normal.normalized * PlayerKnockBackForce, ForceMode.Impulse);
    }

    public override void OnHitOther(Vector3 point, float damage)
    {
        
    }

    public override float PlayerRecoilMultiplier => PlayerKnockBackForce;
}
