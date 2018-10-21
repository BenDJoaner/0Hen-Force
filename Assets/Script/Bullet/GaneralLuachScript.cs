using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GaneralLuachScript : NetworkBehaviour
{
    [FieldLabel("是否自生触发攻击效果")]
    public bool selfEffect;
    [HideInInspector]
    public int _teamFlag;
    [FieldLabel("对队友有效")]
    public bool _teamEffect;
    [FieldLabel("强制位移目标")]
    public float _forceTransSpeed;
    [FieldLabel("生产对象")]
    public GameObject effect;
    [FieldLabel("作用时间")]
    public float _effecTime;
    [FieldLabel("力度")]
    public float _force;
    [FieldLabel("变色")]
    public Color _color;

    //public float speed=1;
    // Use this for initialization
    void Start()
    {
        Destroy(gameObject, 5.0f);
    }


    [ClientCallback]
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (selfEffect)
        {
            if (collision.gameObject.GetComponent<PlatformerCharacter2D>())
            {
                if (collision.gameObject.GetComponent<LHNetworkPlayer>().team == _teamFlag && !_teamEffect) return;
                collision.gameObject.GetComponent<PlatformerCharacter2D>().
                    //传入 作用时间，作用点坐标，作用力，颜色
                    PlayerUncontrol(_forceTransSpeed, GetComponent<CircleCollider2D>().radius, _effecTime, transform.position, _force, _color);
                Destroy(gameObject);
            }
        }
        else
        {
            GameObject temp = Instantiate(effect, transform.position, Quaternion.identity);
            temp.GetComponentInChildren<ExplorationScript>()._teamFlag = _teamFlag;
            NetworkServer.Spawn(temp);
            Destroy(gameObject);
        }
    }
}
