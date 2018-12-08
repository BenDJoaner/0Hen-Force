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


    // Use this for initialization
    void Start()
    {
        _player = GetComponent<LHNetworkPlayer>();
        m_PointerCheck = transform.Find("PointerCheck").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (!initDone)
        {
            DataInit(_player._data);
        }
    }

    public void DataInit(CharacterData data)
    {
        if (initDone || data == null) return;
        _data = data;
        initDone = true;
    }

    void FixedUpdate()
    {

        if (initDone)
        {
            _joystick.x = CnInputManager.GetAxis("Horizontal");
            _joystick.y = CnInputManager.GetAxis("Vertical");
            m_attack = CnInputManager.GetButton("Fire1");

            AimFunc();

            if (m_attack)
            {

            }
            else
            {

            }
        }
    }

    void AimFunc()
    {
        if (_joystick == new Vector2(0, 0)) return;
        float angle = Mathf.Atan2(_joystick.y, _joystick.x) * 180f / Mathf.PI;
        _data.SetAimAngle(angle);
        m_PointerCheck.transform.eulerAngles = new Vector3(0, 0, angle);
    }
}
