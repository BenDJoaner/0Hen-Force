using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlakHole : MonoBehaviour
{
    LHNetworkGameManager manager;
    float coldTime;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!manager && GameObject.FindGameObjectWithTag("GameManager"))
        {
            manager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<LHNetworkGameManager>();
        }
        if (coldTime < 1)
        {
            coldTime += Time.deltaTime;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (coldTime >= 1 && collision.gameObject.tag == "Player")
        {
            LHNetworkPlayer player = collision.GetComponent<LHNetworkPlayer>();
            EnegySprite _eSprit = player.GetEnegySprite();
            if (_eSprit)
            {
                _eSprit.OnSelfDestory();
                manager.OnUseEnegy(player);
                coldTime = 0;
            }
        }
    }
}
