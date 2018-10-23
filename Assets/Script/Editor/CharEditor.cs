using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CharacterData))]
public class CharEditor : Editor
{

    private SerializedObject data;
    private SerializedProperty id, image, charName, m_charPos, charDesc, skillDesc, supperSkillDesc, airContorl, jumpForce, moveSpeed, weight, preAttackTime, AfterAttackTime;
    //灵痕模块
    private SerializedProperty BuffModule, SpriteModule, ShooterModule, WarriourModule, GhostModule;
    //守护灵痕
    private SerializedProperty m_EffectTo, BuffTime, BuffObj, StartBorn, EffectRadius, ActiveTime, EyeObj;
    //精密灵痕
    private SerializedProperty AutoAim, AimContorlable, luanchForce, luanchTime, luanchAngle, attackSpeed, bullet;
    //突围灵痕
    private SerializedProperty HugForce, HugAngle, HitForce, m_Effect, EffectTime, Invancible, WeightUp;
    //鬼影灵痕
    private SerializedProperty SelfMoveDistend, SelfMoveTime, SelfMoveAngle, SelfInvisible, InvisibleTime, IgnorColleder, IgnorGravity, SpeedUp, JumpForceUp, MoveAngleContorlable;


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
        AfterAttackTime = data.FindProperty("AfterAttackTime");

        //====================灵痕模块========================
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

        //====================精密灵痕========================
        AutoAim = data.FindProperty("AutoAim");
        AimContorlable = data.FindProperty("AimContorlable");
        luanchForce = data.FindProperty("luanchForce");
        luanchTime = data.FindProperty("luanchTime");
        luanchAngle = data.FindProperty("luanchAngle");
        attackSpeed = data.FindProperty("attackSpeed");
        bullet = data.FindProperty("bullet");

        //====================突围灵痕========================
        HugForce = data.FindProperty("HugForce");
        HugAngle = data.FindProperty("HugAngle");
        HitForce = data.FindProperty("HitForce");
        m_Effect = data.FindProperty("m_Effect");
        EffectTime = data.FindProperty("EffectTime");
        Invancible = data.FindProperty("Invancible");
        WeightUp = data.FindProperty("WeightUp");

        //====================鬼影灵痕========================
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
        EditorGUILayout.PropertyField(AfterAttackTime);

        //====================守护灵痕========================
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
        }
        else
        {
            EffectRadius.floatValue = 0f;
        }

        //====================精密灵痕========================
        EditorGUILayout.PropertyField(ShooterModule);
        if (ShooterModule.boolValue)
        {
            // EditorGUILayout.LabelField("灵痕简介：向外抛射子弹", EditorStyles.label);
            EditorGUILayout.PropertyField(AimContorlable);
            if (!AimContorlable.boolValue)
            {
                EditorGUILayout.PropertyField(AutoAim);
                if (!AutoAim.boolValue)
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
        EditorGUILayout.PropertyField(WarriourModule);
        if (WarriourModule.boolValue)
        {
            // EditorGUILayout.LabelField("灵痕简介：本身就是战斗机器", EditorStyles.label);
            EditorGUILayout.PropertyField(HugForce);
            EditorGUILayout.PropertyField(HugAngle);
            EditorGUILayout.PropertyField(HitForce);
            EditorGUILayout.PropertyField(m_Effect);
            EditorGUILayout.PropertyField(EffectTime);
            EditorGUILayout.PropertyField(Invancible);
            EditorGUILayout.PropertyField(WeightUp);
        }

        //====================鬼影灵痕========================
        EditorGUILayout.PropertyField(GhostModule);
        if (GhostModule.boolValue)
        {
            // EditorGUILayout.LabelField("灵痕简介：拥有神出鬼没的能力", EditorStyles.label);
            EditorGUILayout.PropertyField(SelfMoveDistend);
            EditorGUILayout.PropertyField(SelfMoveTime);
            EditorGUILayout.PropertyField(MoveAngleContorlable);
            //
            if (MoveAngleContorlable.boolValue)
            {
                EditorGUILayout.PropertyField(SelfMoveAngle);
            }
            EditorGUILayout.PropertyField(SelfInvisible);
            if (SelfInvisible.boolValue)
            {
                EditorGUILayout.PropertyField(InvisibleTime);
            }
            EditorGUILayout.PropertyField(IgnorColleder);
            EditorGUILayout.PropertyField(IgnorGravity);
            EditorGUILayout.PropertyField(SpeedUp);
            EditorGUILayout.PropertyField(JumpForceUp);
        }
        data.ApplyModifiedProperties();
    }
}
