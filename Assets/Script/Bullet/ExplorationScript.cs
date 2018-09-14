using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ExplorationScript : NetworkBehaviour
{

    [SerializeField]
    private  float _effecTime;

    [SerializeField]
    private  float _force;

    [SerializeField]
    private  Color _color;

    [SerializeField]
    private float _forceTransSpeed;

    [SerializeField]
    private bool _teamEffect;

    public int _teamFlag;

    private void Start()
    {
        Destroy(gameObject, 5.0f);
    }

    [ClientCallback]
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<PlatformerCharacter2D>())
        {
            if (collision.gameObject.GetComponent<LHNetworkPlayer>().team == _teamFlag&& !_teamEffect) return;
            collision.gameObject.GetComponent<PlatformerCharacter2D>().
                //传入 作用时间，作用点坐标，作用力，颜色
                PlayerUncontrol(_forceTransSpeed,GetComponent<CircleCollider2D>().radius, _effecTime, transform.position, _force, _color);

        }
    }
}
