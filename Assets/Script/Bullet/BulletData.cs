using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(Rigidbody2D))]
public class BulletData : NetworkBehaviour
{
    [FieldLabel("作用效果")]
    public AttackEffect m_Effect;

    [FieldLabel("生命周期")]
    public float LifeTime;

    [FieldLabel("是载体")]
    public bool IsCarryer;

    [FieldLabel("生成物")]
    public BulletData BornBullet;

    [FieldLabel("对队友有效")]
    public bool TeamEffect;

    [FieldLabel("无视重力")]
    public bool IgnorGravity;

    [FieldLabel("作用力")]
    public float HitForce;

    [FieldLabel("附着目标")]
    public bool AttachTarget;

    [FieldLabel("目标硬直")]
    public bool Freeze;

    [FieldLabel("硬直时间")]
    public float FreezeTime;

    [FieldLabel("硬直变色")]
    public Color Freezecolor;

    [FieldLabel("是否穿透")]
    public bool CrossOver;

    [HideInInspector]
    public LHNetworkPlayer _borner;//发射者

    void Start()
    {
        Destroy(gameObject, LifeTime);
    }

    public enum AttackEffect
    {
        击退 = 0,
        击落 = 1,
        魅惑 = 2,
        恐惧 = 3,
        晕眩 = 4,
        禁锢 = 5,
        减速 = 6,
        虚空 = 7,
        致盲 = 8
    }

    [ClientCallback]
    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject other = collision.gameObject;
        if (IsCarryer)
        {
            GameObject bullet = Instantiate(BornBullet.gameObject, transform.position, Quaternion.identity);
            NetworkServer.Spawn(bullet);
            Destroy(gameObject);
        }
        else
        {
            PlatformerCharacter2D charecter = other.GetComponent<PlatformerCharacter2D>();
            if (charecter)
            {
                if (other.GetComponent<LHNetworkPlayer>().team == _borner.team && !TeamEffect) return;
                //传入 作用时间，作用点坐标，作用力，颜色
                // charecter.PlayerUncontrol(_forceTransSpeed, GetComponent<CircleCollider2D>().radius, _effecTime, transform.position, _force, _color);
                Destroy(gameObject);
            }
        }
    }
}
