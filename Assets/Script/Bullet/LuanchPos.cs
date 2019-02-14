using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LuanchPos : MonoBehaviour
{
    public Vector3 pos;
    bool flag;
    BulletData bullet;
    float force;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        pos = transform.position;
        if (flag)
        {
            GameObject tempBullet = Instantiate(bullet.gameObject, pos, transform.rotation);
            tempBullet.GetComponent<BulletData>().OnCreated(force);
            flag = false;
        }
    }

    public void OnFire(BulletData _bullet, float _force)
    {
        print(_force);
        
        flag = true;
        force = _force;
        bullet = _bullet;
    }
}
