using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class BulletData : MonoBehaviour {

	[Header("是否爆炸")]
    public bool isExplor;

	[Header("发射时特效")]
    public GameObject LaunchEffect;

	[Header("击中特效")]
    public GameObject HitEffect;

	[Header("爆炸预设体")]
    public GameObject ExplorEffect;

	[Header("击中作用力")]
    public float HitForce;

	[Header("击中硬直")]
    public float HitFreeze;

	[Header("硬直变色")]
    public Color Hitcolor;

	[Header("是否穿透")]
    public bool CrossOver;
}
