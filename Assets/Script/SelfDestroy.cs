using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestroy : MonoBehaviour
{
    public float destroy_Time;
    GameObject player;
    private float activeTime;
    // Use this for initialization
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (destroy_Time != 0)
        {
            activeTime += Time.deltaTime;
            if (activeTime > destroy_Time)
            {
                Destroy(gameObject);
            }
        }
        else if (transform.position.y - player.transform.position.y > 20)
        {
            Destroy(this.gameObject);
        }

    }
}
