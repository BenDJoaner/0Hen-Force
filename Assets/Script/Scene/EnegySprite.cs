using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnegySprite : MonoBehaviour
{
    Transform CharObj;
    public Transform imageTrans;
    public float EnenyNum = 1;
    // Start is called before the first frame update
    void Start()
    {

    }
    // Update is called once per frame
    void Update()
    {
        if (CharObj)
        {
            GetComponent<AutoMove>().enabled = false;
            transform.position = CharObj.position;
            imageTrans.RotateAround(CharObj.position, Vector3.forward, 5);
        }
        else
        {
            GetComponent<AutoMove>().enabled = true;
        }

    }

    public void AttachPlayer(Transform trans)
    {
        CharObj = trans;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (!collision.GetComponent<LHNetworkPlayer>().CarrySprite(this))
            {
                AttachPlayer(collision.transform);
                imageTrans.position = new Vector2(transform.position.x + 1, transform.position.y);
            }
        }
    }
}
