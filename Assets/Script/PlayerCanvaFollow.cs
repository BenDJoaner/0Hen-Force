using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using UnityStandardAssets._2D;

public class PlayerCanvaFollow : MonoBehaviour
{
    public Text PlayerName;
    public Slider EnegySlider;
    private Transform target;
    public float damping = 1;

    private float m_OffsetZ;
    private Vector3 m_LastTargetPosition;
    private Vector3 m_CurrentVelocity;
    private Vector3 m_LookAheadPos;

    // Use this for initialization
    private void Start()
    {
        m_LastTargetPosition = target.position;
        m_OffsetZ = (transform.position - target.position).z;
        transform.parent = null;
    }


    // Update is called once per frame
    private void Update()
    {
        // only update lookahead pos if accelerating or changed direction
        //float xMoveDelta = (target.position - m_LastTargetPosition).x;
        if (target)
        {
            Vector3 aheadTargetPos = target.position + m_LookAheadPos + Vector3.forward * m_OffsetZ;
            Vector3 newPos = Vector3.SmoothDamp(transform.position, aheadTargetPos, ref m_CurrentVelocity, damping);
            transform.position = newPos;
            m_LastTargetPosition = target.position;
        }
    }

    /// <summary>
    /// 设置名字
    /// </summary>
    /// <param name="name">Name.</param>
    public void OnSetPlayerName(string name)
    {
        PlayerName.text = name;
    }

    public void OnSetTargetTransform(Transform trans)
    {
        target = trans;
    }

    public void OnSetColor(Color color)
    {
        PlayerName.color = color;
    }
}
