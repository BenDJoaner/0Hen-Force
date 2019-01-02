using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Events;

[RequireComponent(typeof(SpriteRenderer))]
// [RequireComponent(typeof(NetworkIdentity))]
[ExecuteInEditMode]
[AddComponentMenu("0HenTool/CharacterData")]
public class CharacterData : MonoBehaviour
{
    public int id;

    [FieldLabel("头像")]
    public Sprite image;

    [FieldLabel("角色名")]
    public string charName;

    [Tooltip("SLIPPY:迅捷\n\nATTACK:强攻\n\nSUPPORT:支援")]
    [FieldLabel("角色定位")]
    public Common.PosEnum m_charPos;

    [FieldLabel("角色介绍")]
    public string charDesc;

    [FieldLabel("普通技能介绍")]
    public string skillDesc;

    [FieldLabel("终极技能介绍")]
    public string supperSkillDesc;

    [FieldLabel("空中控制")]
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
    public float endAttackTime;
    //====================守护灵痕========================
    [Space]
    [Tooltip("灵痕简介：产生范围作用")]
    [FieldLabel("守护灵痕>>>>>>>>>>>>>>>>>")]
    public bool guardeModule;

    [Tooltip("self:自己\n\nENEMY:敌方\n\nTEAM:友方\n\nBOTH:双方")]
    [FieldLabel("-作用于")]
    public Common.EffectTo m_EffectTo;

    [FieldLabel("-生效时间")]
    public float buffTime;

    [FieldLabel("-开局生成")]
    public bool startBorn;

    [FieldLabel("-作用范围")]
    public float effectRadius;

    [Tooltip("0为无限")]
    [FieldLabel("-持续时间")]
    public float activeTime;

    [FieldLabel("-守护预设体")]
    public GameObject guardeObj;
    //====================精密灵痕========================
    [Space]
    [Tooltip("灵痕简介：精准地向外抛射子弹")]
    [FieldLabel("精密灵痕>>>>>>>>>>>>>>>>>")]
    public bool shooterModule;

    [FieldLabel("--自动瞄准")]
    public bool autoAim;

    [FieldLabel("-可控射击方向")]
    public bool aimContorlable;

    [FieldLabel("-最大抛射力度")]
    public float luanchForce;

    [Tooltip("0为无需蓄力")]
    [FieldLabel("-最大蓄力时间")]
    public float luanchTime;

    [FieldLabel("---抛射角度")]
    public float luanchAngle;

    [FieldLabel("-攻击冷却时间")]
    public float attackSpeed;

    [FieldLabel("-子弹预设体")]
    public BulletData bullet;
    //====================突围灵痕========================
    [Space]
    [Tooltip("灵痕简介：本身就是战斗机器")]
    [FieldLabel("突围灵痕>>>>>>>>>>>>>>>>>")]
    public bool warriourModule;

    [Tooltip("1,REPEL-击退：对目标产生强位移\n\n2,CHARM-魅惑：目标向攻击者移动(受损)\n\n3,FEAR-恐惧：目标远离攻击者移动(受损)\n\n4,CONFINE-禁锢：目标不能移动\n\n5,DECELERATE-减速：目标移动速度受损\n\n6,CONGEAL-凝滞：目标无法操作和选中")]
    [FieldLabel("-攻击效果")]
    public Common.AttackEffect m_Effect;

    [FieldLabel("-攻击力度")]
    public float hitForce;

    [FieldLabel("-攻击作用时间")]
    public float effectTime;

    [FieldLabel("-无视强位移")]
    public bool invancible;

    [FieldLabel("-位移距离")]
    public float selfMoveDistend;

    [FieldLabel("-位移时间")]
    public float selfMoveTime;
    //====================鬼影灵痕========================
    [Space]
    [Tooltip("灵痕简介：拥有神出鬼没的能力")]
    [FieldLabel("鬼影灵痕>>>>>>>>>>>>>>>>>")]
    public bool ghostModule;

    [FieldLabel("-隐身")]
    public bool selfInvisible;

    [FieldLabel("--隐身持续时间")]
    public float invisibleTime;

    [FieldLabel("-无视碰撞")]
    public bool ignorColleder;

    [FieldLabel("-无视重力")]
    public bool ignorGravity;

    [FieldLabel("--质量倍数")]
    public float massTimes;

    [FieldLabel("-移动速度倍数")]
    public float speedTimes;

    //=============获取各种东西的方法=================
    public Animator OnGetAnimator()
    {
        return GetComponent<Animator>();
    }

    //=============Override方法=================

    public virtual void OnFire()
    {
        GetComponent<Animator>().SetTrigger("attack");
    }

    public virtual void OnHit(Common.AttackEffect effect)
    {
        switch (effect)
        {
            case (Common.AttackEffect)1:
                break;
            case (Common.AttackEffect)2:
                break;
            case (Common.AttackEffect)3:
                break;
            case (Common.AttackEffect)4:
                break;
            case (Common.AttackEffect)5:
                break;
            case (Common.AttackEffect)6:
                break;
        }
        GetComponent<Animator>().SetTrigger("hit");
    }

    public virtual void OnMove(float speed)
    {
        if (GetComponent<Animator>())
        {
            GetComponent<Animator>().SetFloat("Speed", Mathf.Abs(speed));
        }
    }


    public virtual void OnDead()
    {
        GetComponent<Animator>().SetTrigger("dead");
        Destroy(gameObject);
    }
}