using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    GameObject localPlayer;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public GameObject GetLocalPlayer()
    {
        return localPlayer;
    }

    public void SetLocalPlayer(GameObject obj)
    {
        localPlayer = obj;
    }
}
