﻿using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CharacterData))]
public class CharEditor : Editor
{

    private SerializedObject data;
    private SerializedProperty id, image, charName, m_charPos, charDesc, skillDesc, supperSkillDesc, airContorl, jumpForce, moveSpeed, weight, preAttackTime, endAttackTime;
    //灵痕模块
<<<<<<< HEAD
    private SerializedProperty BuffModule, SpriteModule, ShooterModule, WarriourModule, GhostModule;
    //守护灵痕
    private SerializedProperty m_EffectTo, BuffTime, BuffObj, StartBorn, EffectRadius, ActiveTime, EyeObj;
=======
    private SerializedProperty BuffModule, guardeModule, shooterModule, warriourModule, ghostModule;
    //守护灵痕
    private SerializedProperty m_EffectTo, buffTime, startBorn, effectRadius, activeTime, guardeObj;
>>>>>>> 2733a25c4383e00bf7a2e7e43898f36e4861e985
    //精密灵痕
    private SerializedProperty autoAim, aimContorlable, luanchForce, luanchTime, luanchAngle, attackSpeed, bullet;
    //突围灵痕
    private SerializedProperty hitForce, m_Effect, effectTime, invancible, selfMoveDistend, selfMoveTime, selfMoveAngle;
    //鬼影灵痕
<<<<<<< HEAD
    private SerializedProperty SelfMoveDistend, SelfMoveTime, SelfMoveAngle, SelfInvisible, InvisibleTime, IgnorColleder, IgnorGravity, SpeedUp, JumpForceUp, MoveAngleContorlable;
=======
    private SerializedProperty selfInvisible, invisibleTime, ignorColleder, ignorGravity, speedTimes, massTimes;
>>>>>>> 2733a25c4383e00bf7a2e7e43898f36e4861e985


    private void OnEnable()
    {
        data = new SerializedObject(target);
        id = data.FindProperty("id");
        image = data.FindProperty("image");
        charName = data.FindProperty("charName");
        m_charPos = data.FindProperty("m_charPos");
        charDesc = data.FindProperty("charDesc");
        skillDesc = data.FindProperty("skillDesc");
        supperSkillDesc = data.FindProperty("supperSkillDesc");
        airContorl = data.FindProperty("airContorl");
        jumpForce = data.FindProperty("jumpForce");
        moveSpeed = data.FindProperty("moveSpeed");
        weight = data.FindProperty("weight");
        preAttackTime = data.FindProperty("preAttackTime");
        endAttackTime = data.FindProperty("endAttackTime");

        //====================灵痕模块========================
<<<<<<< HEAD
        SpriteModule = data.FindProperty("SpriteModule");
        ShooterModule = data.FindProperty("ShooterModule");
        WarriourModule = data.FindProperty("WarriourModule");
        GhostModule = data.FindProperty("GhostModule");

        //====================守护灵痕========================
        m_EffectTo = data.FindProperty("m_EffectTo");
        BuffTime = data.FindProperty("BuffTime");
        BuffObj = data.FindProperty("BuffObj");
        StartBorn = data.FindProperty("StartBorn");
        EffectRadius = data.FindProperty("EffectRadius");
        ActiveTime = data.FindProperty("ActiveTime");
        EyeObj = data.FindProperty("EyeObj");
=======
        guardeModule = data.FindProperty("guardeModule");
        shooterModule = data.FindProperty("shooterModule");
        warriourModule = data.FindProperty("warriourModule");
        ghostModule = data.FindProperty("ghostModule");

        //====================守护灵痕========================
        m_EffectTo = data.FindProperty("m_EffectTo");
        buffTime = data.FindProperty("buffTime");
        startBorn = data.FindProperty("startBorn");
        effectRadius = data.FindProperty("effectRadius");
        activeTime = data.FindProperty("activeTime");
        guardeObj = data.FindProperty("guardeObj");
>>>>>>> 2733a25c4383e00bf7a2e7e43898f36e4861e985

        //====================精密灵痕========================
        autoAim = data.FindProperty("autoAim");
        aimContorlable = data.FindProperty("aimContorlable");
        luanchForce = data.FindProperty("luanchForce");
        luanchTime = data.FindProperty("luanchTime");
        luanchAngle = data.FindProperty("luanchAngle");
        attackSpeed = data.FindProperty("attackSpeed");
        bullet = data.FindProperty("bullet");

        //====================突围灵痕========================
        hitForce = data.FindProperty("hitForce");
        m_Effect = data.FindProperty("m_Effect");
        effectTime = data.FindProperty("effectTime");
        invancible = data.FindProperty("invancible");
        selfMoveDistend = data.FindProperty("selfMoveDistend");
        selfMoveTime = data.FindProperty("selfMoveTime");
        selfMoveAngle = data.FindProperty("selfMoveAngle");

        //====================鬼影灵痕========================
<<<<<<< HEAD
        SelfMoveDistend = data.FindProperty("SelfMoveDistend");
        SelfMoveTime = data.FindProperty("SelfMoveTime");
        MoveAngleContorlable = data.FindProperty("MoveAngleContorlable");
        SelfMoveAngle = data.FindProperty("SelfMoveAngle");
        SelfInvisible = data.FindProperty("SelfInvisible");
        InvisibleTime = data.FindProperty("InvisibleTime");
        IgnorColleder = data.FindProperty("IgnorColleder");
        IgnorGravity = data.FindProperty("IgnorGravity");
        SpeedUp = data.FindProperty("SpeedUp");
        JumpForceUp = data.FindProperty("JumpForceUp");
=======

        selfInvisible = data.FindProperty("selfInvisible");
        invisibleTime = data.FindProperty("invisibleTime");
        ignorColleder = data.FindProperty("ignorColleder");
        ignorGravity = data.FindProperty("ignorGravity");
        massTimes = data.FindProperty("massTimes");
        speedTimes = data.FindProperty("speedTimes");
>>>>>>> 2733a25c4383e00bf7a2e7e43898f36e4861e985
    }

    public override void OnInspectorGUI()
    {
        data.Update();

        EditorGUILayout.LabelField("====通用属性====", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(id);
        EditorGUILayout.PropertyField(image);
        EditorGUILayout.PropertyField(charName);
        EditorGUILayout.PropertyField(m_charPos);
        EditorGUILayout.PropertyField(charDesc);
        EditorGUILayout.PropertyField(skillDesc);
        EditorGUILayout.PropertyField(supperSkillDesc);
        EditorGUILayout.PropertyField(airContorl);
        EditorGUILayout.PropertyField(jumpForce);
        EditorGUILayout.PropertyField(moveSpeed);
        EditorGUILayout.PropertyField(weight);
        EditorGUILayout.Space();
        //显示攻击属性========================
        EditorGUILayout.LabelField("====攻击属性====", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(preAttackTime);
        EditorGUILayout.PropertyField(endAttackTime);

        //====================守护灵痕========================
<<<<<<< HEAD
        EditorGUILayout.PropertyField(SpriteModule);
        if (SpriteModule.boolValue)
        {
            // EditorGUILayout.LabelField("灵痕简介：产生范围作用", EditorStyles.label);
            EditorGUILayout.PropertyField(m_EffectTo);
            EditorGUILayout.PropertyField(BuffTime);
            EditorGUILayout.PropertyField(BuffObj);
            EditorGUILayout.PropertyField(StartBorn);
            EditorGUILayout.PropertyField(EffectRadius);
            EditorGUILayout.PropertyField(ActiveTime);
            EditorGUILayout.PropertyField(EyeObj);
=======
        EditorGUILayout.PropertyField(guardeModule);
        if (guardeModule.boolValue)
        {
            // EditorGUILayout.LabelField("灵痕简介：产生范围作用", EditorStyles.label);
            EditorGUILayout.PropertyField(m_EffectTo);
            EditorGUILayout.PropertyField(buffTime);
            EditorGUILayout.PropertyField(startBorn);
            EditorGUILayout.PropertyField(effectRadius);
            EditorGUILayout.PropertyField(activeTime);
            EditorGUILayout.PropertyField(guardeObj);
        }
        else
        {
            effectRadius.floatValue = 0f;
>>>>>>> 2733a25c4383e00bf7a2e7e43898f36e4861e985
        }
        else
        {
            EffectRadius.floatValue = 0f;
        }

        //====================精密灵痕========================
        EditorGUILayout.PropertyField(shooterModule);
        if (shooterModule.boolValue)
        {
            // EditorGUILayout.LabelField("灵痕简介：向外抛射子弹", EditorStyles.label);
<<<<<<< HEAD
            EditorGUILayout.PropertyField(AimContorlable);
            if (!AimContorlable.boolValue)
=======
            EditorGUILayout.PropertyField(aimContorlable);
            if (!aimContorlable.boolValue)
>>>>>>> 2733a25c4383e00bf7a2e7e43898f36e4861e985
            {
                EditorGUILayout.PropertyField(autoAim);
                if (!autoAim.boolValue)
                {
                    EditorGUILayout.PropertyField(luanchAngle);
                }
            }
            EditorGUILayout.PropertyField(luanchForce);
            EditorGUILayout.PropertyField(luanchTime);
            EditorGUILayout.PropertyField(attackSpeed);
            EditorGUILayout.PropertyField(bullet);
        }

        //====================突围灵痕========================
        EditorGUILayout.PropertyField(warriourModule);
        if (warriourModule.boolValue)
        {
            // EditorGUILayout.LabelField("灵痕简介：本身就是战斗机器", EditorStyles.label);
<<<<<<< HEAD
            EditorGUILayout.PropertyField(HugForce);
            EditorGUILayout.PropertyField(HugAngle);
            EditorGUILayout.PropertyField(HitForce);
=======
>>>>>>> 2733a25c4383e00bf7a2e7e43898f36e4861e985
            EditorGUILayout.PropertyField(m_Effect);
            EditorGUILayout.PropertyField(hitForce);
            EditorGUILayout.PropertyField(effectTime);
            EditorGUILayout.PropertyField(invancible);
            EditorGUILayout.PropertyField(selfMoveDistend);
            EditorGUILayout.PropertyField(selfMoveTime);
            EditorGUILayout.PropertyField(selfMoveAngle);
        }

        //====================鬼影灵痕========================
        EditorGUILayout.PropertyField(ghostModule);
        if (ghostModule.boolValue)
        {
            // EditorGUILayout.LabelField("灵痕简介：拥有神出鬼没的能力", EditorStyles.label);
<<<<<<< HEAD
            EditorGUILayout.PropertyField(SelfMoveDistend);
            EditorGUILayout.PropertyField(SelfMoveTime);
            EditorGUILayout.PropertyField(MoveAngleContorlable);
            //
            if (MoveAngleContorlable.boolValue)
=======
            EditorGUILayout.PropertyField(selfInvisible);
            if (selfInvisible.boolValue)
>>>>>>> 2733a25c4383e00bf7a2e7e43898f36e4861e985
            {
                EditorGUILayout.PropertyField(invisibleTime);
            }
            EditorGUILayout.PropertyField(ignorColleder);
            EditorGUILayout.PropertyField(ignorGravity);
            if (!ignorGravity.boolValue)
            {
                EditorGUILayout.PropertyField(massTimes);
            }
<<<<<<< HEAD
            EditorGUILayout.PropertyField(IgnorColleder);
            EditorGUILayout.PropertyField(IgnorGravity);
            EditorGUILayout.PropertyField(SpeedUp);
            EditorGUILayout.PropertyField(JumpForceUp);
=======
            EditorGUILayout.PropertyField(speedTimes);
>>>>>>> 2733a25c4383e00bf7a2e7e43898f36e4861e985
        }
        data.ApplyModifiedProperties();
    }
}
