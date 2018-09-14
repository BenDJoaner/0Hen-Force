using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GaneralLuachScript : NetworkBehaviour
{
    [Header("是否自生触发攻击效果")]
    public bool selfEffect;
    [HideInInspector]
    public int _teamFlag;
    [SerializeField]
    [Header("对队友有效")]
    private bool _teamEffect;
    [Header("强制位移目标")]
    [SerializeField]
    private float _forceTransSpeed;
    [Header("当SelfEffect为false时必须有对象")]
    public GameObject effect;
    [Header("当SelfEffect为ture时填写")]
    [SerializeField]
    [Header("作用时间")]
    private float _effecTime;

    [SerializeField]
    private float _force;

    [SerializeField]
    private Color _color;

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
