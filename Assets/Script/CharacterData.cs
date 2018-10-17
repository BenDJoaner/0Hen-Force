using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(NetworkIdentity))]
[RequireComponent(typeof(NetworkAnimator))]

public class CharacterData : MonoBehaviour
{
    [Header("ID")]
    public int charID = 10001;

    [Header("头像")]
    public Sprite image;

    [Header("角色名")]
    public string charName = "无";

    [Header("角色定位")]
    public PosEnum m_charPos;

    [Header("角色介绍")]
    public string charDesc = "无";

    [Header("技能介绍")]
    public string skillDesc = "无";

    [Header("置空")]
    public bool airContorl;

    [Header("弹跳力")]
    public int jumpForce = 1500;

    [Header("移动速度")]
    public int moveSpeed = 8;

    [Header("重量")]
    public int weight = 6;

    [Header("攻击前摇")]
    public float preAttackTime;

    [Header("攻击后摇")]
    public float AfterAttackTime;
    //====================光环灵痕========================
    [Header("光环灵痕**********************")]
    public bool BuffModule;

    [Header("作用于：")]
    public EffectTo m_EffectTo;

    [Header("作用时间(0为无限)")]
    public float BuffTime;

    [Header("飞行效果")]
    public GameObject FlyMode;

    [Header("飞行速度")]
    public GameObject FlySpeed;

    [Header("光环预设体")]
    public GameObject BuffObj;
    //====================守护灵痕========================
    [Header("守护灵痕**********************")]
    public bool SpriteModule;

    [Header("开始时生成")]
    public bool StartBorn;

    [Header("无视重力")]
    public bool IgnorGravity;

    [Header("持续时间(0为无限)")]
    public float ActiveTime;

    [Header("守护预设体")]
    public GameObject EyeObj;
    //====================精密灵痕========================
    [Header("精密灵痕**********************")]
    public bool ShooterModule;

    [Header("自动瞄准")]
    public bool AutoAim;

    [Header("可控射击方向")]
    public bool AimContorlable;

    [Header("最大抛射力度(0为原地放置)")]
    public float luanchForce;

    [Header("最大蓄力时间(0为瞬间抛出)")]
    public float luanchTime;

    [Header("抛射角度(方向不可控时)")]
    public float luanchAngle;

    [Header("攻击冷却时间")]
    public float attackSpeed;

    [Header("子弹预设体")]
    public BulletData bullet;
    //====================突围灵痕========================
    [Header("突围灵痕**********************")]
    public bool WarriourModule;

    [Header("前扑力度")]
    public float HugForce;

    [Header("前扑角度")]
    public float HugAngle;

    [Header("攻击力度")]
    public float HitForce;

    [Header("攻击效果")]
    public AttackEffect m_Effect;

    [Header("攻击作用时间")]
    public float EffectTime;

    [Header("无视强位移")]
    public bool Invancible;

    [Header("重力倍数")]
    public float WeightUp;
    //====================鬼影灵痕========================
    [Header("鬼影灵痕**********************")]
    public bool GhostModule;

    [Header("位移距离")]
    public float SelfMoveDistend;

    [Header("位移时间(0为瞬间到达)")]
    public float SelfMoveTime;

    [Header("可控位移方向")]
    public bool MoveAngleContorlable;

    [Header("位移的角度(方向不可控时)")]
    public float SelfMoveAngle;

    [Header("隐身")]
    public bool SelfInvisible;

    [Header("无视碰撞")]
    public bool IgnorColleder;

    [Header("移动速度倍数")]
    public float SpeedUp;

    [Header("弹跳力倍数")]
    public float JumpForceUp;

    public enum PosEnum
    {
        SLIPPY = 0,
        ATTACK = 1,
        SUPPORT = 2
    }

    public enum EffectTo
    {
        自己 = 0,
        敌方 = 1,
        我方 = 2,
        双方 = 3
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

    //=============Override方法=================

    public virtual void OnFire()
    {
        GetComponent<Animator>().SetTrigger("attack");
    }

    public virtual void OnHit()
    {
        GetComponent<Animator>().SetTrigger("hit");
    }

    public virtual void OnMove(float speed)
    {
        GetComponent<Animator>().SetFloat("Speed", Mathf.Abs(speed));
    }


    public virtual void OnDead()
    {
        //GetComponent<Animator>().SetTrigger("dead");
        Destroy(gameObject);
    }

    public List<Transform> GetBornPosList()
    {
        List<Transform> posList = new List<Transform> { };
        foreach (Transform pos in gameObject.transform)
        {
            if (pos.tag == "bornPos")
                posList.Add(pos);
        }
        return posList;
    }
}
