using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BronTransConfig : MonoBehaviour
{
    private List<Transform> itemList = new List<Transform>();
    void Start()
    {
        foreach (Transform trans in transform)
        {
            itemList.Add(trans);
        }
    }

    public List<Transform> GetAllTrans()
    {
        return itemList;
    }


    public Transform GetRandomTrans()
    {
        int randomNum = Random.Range(0, itemList.Count);
        return itemList[randomNum].transform;
    }
}
