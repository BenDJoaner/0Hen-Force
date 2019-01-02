using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CnControls;
using System;

public class LHAttackController : MonoBehaviour
{

    private Vector2 _joystick;//操作杆方位
    private bool initDone;
    LHNetworkPlayer _player;
    CharacterData _data;
    bool m_attack;
    private GameObject m_PointerCheck;
    Parabola parabola;

    public bool InitDone { get => initDone; set => initDone = value; }

    // Use this for initialization
    void Start()
    {
        _player = GetComponent<LHNetworkPlayer>();
        m_PointerCheck = transform.Find("PointerCheck").gameObject;
        parabola = m_PointerCheck.GetComponent<Parabola>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!InitDone)
        {
            DataInit(_player._data);
        }
    }

    public void DataInit(CharacterData data)
    {
        if (InitDone || data == null) return;
        _data = data;
        if (data.shooterModule && !data.autoAim)
        {
            m_PointerCheck.SetActive(true);
            parabola.shootForce = data.luanchForce;
            if (!data.aimContorlable)
            {
                m_PointerCheck.transform.eulerAngles = new Vector3(0, 0, data.luanchAngle);
            }
        }
        else
        {
            m_PointerCheck.SetActive(false);
        }
        InitDone = true;
    }

    void FixedUpdate()
    {

        if (InitDone)
        {
            _joystick.x = CnInputManager.GetAxis("Horizontal");
            _joystick.y = CnInputManager.GetAxis("Vertical");
            m_attack = CnInputManager.GetButtonDown("attack");

            AimFunc();

            if (m_attack)
            {
                _data.OnFire();
            }
            else
            {

            }
        }
    }

    void AimFunc()
    {
        if (!_data.aimContorlable) return;
        if (_joystick == new Vector2(0, 0)) return;
        float angle = Mathf.Atan2(_joystick.y, _joystick.x) * 180f / Mathf.PI;
        m_PointerCheck.transform.eulerAngles = new Vector3(0, 0, angle);
    }
}
