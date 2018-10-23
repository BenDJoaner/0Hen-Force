using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Events;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(NetworkIdentity))]
[RequireComponent(typeof(NetworkAnimator))]
[ExecuteInEditMode]
// [AddComponentMenu("Character/CharacterData")]
public class CharacterData : MonoBehaviour
{
    public int id = 10001;

    public Sprite image;

    [FieldLabel("角色名")]
    public string charName;

    [FieldLabel("角色定位")]
    public PosEnum m_charPos;

    [FieldLabel("角色介绍")]
    public string charDesc;

    [FieldLabel("普通技能介绍")]
    public string skillDesc;

    [FieldLabel("终极技能介绍")]
    public string supperSkillDesc;

    [FieldLabel("置空")]
    public bool airContorl;

    [FieldLabel("弹跳力")]
    public int jumpForce;

    [FieldLabel("移动速度")]
    public int moveSpeed;

    [FieldLabel("重量")]
    public int weight;

    [FieldLabel("攻击前摇")]
    public float preAttackTime;

    [FieldLabel("攻击后摇")]
    public float AfterAttackTime;
    //====================守护灵痕========================
    [Space]
    [Tooltip("灵痕简介：产生范围作用")]
    [FieldLabel("守护灵痕>>>>>>>>>>>>>>>>>")]
    public bool SpriteModule;

    [FieldLabel("作用于：")]
    public EffectTo m_EffectTo;

    [FieldLabel("作用时间")]
    public float BuffTime;

    [FieldLabel("飞行效果")]
    public GameObject FlyMode;

    [FieldLabel("飞行速度")]
    public GameObject FlySpeed;

    [FieldLabel("光环预设体")]
    public GameObject BuffObj;

    [FieldLabel("开始时生成")]
    public bool StartBorn;

    [FieldLabel("作用范围")]
    public float EffectRadius;

    [Tooltip("0为无限")]
    [FieldLabel("持续时间")]
    public float ActiveTime;

    [FieldLabel("守护预设体")]
    public GameObject EyeObj;
    //====================精密灵痕========================
    [Space]
    [Tooltip("灵痕简介：精准地向外抛射子弹")]
    [FieldLabel("精密灵痕>>>>>>>>>>>>>>>>>")]
    public bool ShooterModule;

    [FieldLabel("自动瞄准")]
    public bool AutoAim;

    [FieldLabel("可控射击方向")]
    public bool AimContorlable;

    [FieldLabel("最大抛射力度")]
    public float luanchForce;

    [Tooltip("0为无需蓄力")]
    [FieldLabel("最大蓄力时间")]
    public float luanchTime;

    [FieldLabel("抛射角度")]
    public float luanchAngle;

    [FieldLabel("攻击冷却时间")]
    public float attackSpeed;

    [FieldLabel("子弹预设体")]
    public BulletData bullet;
    //====================突围灵痕========================
    [Space]
    [Tooltip("灵痕简介：本身就是战斗机器")]
    [FieldLabel("突围灵痕>>>>>>>>>>>>>>>>>")]
    public bool WarriourModule;

    [FieldLabel("前扑力度")]
    public float HugForce;

    [FieldLabel("前扑角度")]
    public float HugAngle;

    [FieldLabel("攻击力度")]
    public float HitForce;

    [FieldLabel("攻击效果")]
    public AttackEffect m_Effect;

    [FieldLabel("攻击作用时间")]
    public float EffectTime;

    [FieldLabel("无视强位移")]
    public bool Invancible;

    [FieldLabel("重力倍数")]
    public float WeightUp;
    //====================鬼影灵痕========================
    [Space]
    [Tooltip("灵痕简介：拥有神出鬼没的能力")]
    [FieldLabel("鬼影灵痕>>>>>>>>>>>>>>>>>")]
    public bool GhostModule;

    [FieldLabel("位移距离")]
    public float SelfMoveDistend;

    [FieldLabel("位移时间")]
    public float SelfMoveTime;

    [FieldLabel("可控位移方向")]
    public bool MoveAngleContorlable;

    [FieldLabel("位移的角度")]
    public float SelfMoveAngle;

    [FieldLabel("隐身")]
    public bool SelfInvisible;

    [FieldLabel("隐身持续时间")]
    public bool InvisibleTime;

    [FieldLabel("无视碰撞")]
    public bool IgnorColleder;

    [FieldLabel("无视重力")]
    public bool IgnorGravity;

    [FieldLabel("移动速度倍数")]
    public float SpeedUp;

    [FieldLabel("弹跳力倍数")]
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

    public List<Parabola> GetBornPosList()
    {
        List<Parabola> posList = new List<Parabola> { };
        foreach (Parabola pos in gameObject.transform)
        {
            if (pos.tag == "bornPos")
                posList.Add(pos);
        }
        return posList;
    }

    public void OnRenderObject()
    {
        DrawCircle(transform, transform.position, EffectRadius);
    }

    public static void DrawCircle(Transform t, Vector3 center, float radius)
    {
        LineRenderer lr = GetLineRenderer(t);
        int pointAmount = 100;//点的数目，值越大曲线越平滑  
        float eachAngle = 360f / pointAmount;
        Vector3 right = t.right;
        lr.SetVertexCount(pointAmount + 1);
        for (int i = 0; i <= pointAmount; i++)
        {
            Vector3 pos = Quaternion.Euler(0f, 0f, eachAngle * i) * right * radius + center;
            lr.SetPosition(i, pos);
        }
    }

    private static LineRenderer GetLineRenderer(Transform t)
    {
        LineRenderer lr = t.GetComponent<LineRenderer>();
        if (lr == null)
        {
            lr = t.gameObject.AddComponent<LineRenderer>();
        }
        lr.SetWidth(0.03f, 0.03f);
        return lr;
    }
}
