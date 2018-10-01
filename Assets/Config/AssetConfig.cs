using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AssetConfig
{

    public static GameObject GetPrefabByName(string name)
    {
        return Resources.Load<GameObject>("Prefab/" + name);
    }

    public static GameObject GetCharByName(string name)
    {
        return Resources.Load<GameObject>("Prefab/Charector/" + name);
    }

    public static Sprite GetCharImgByWay(string Way)
    {
        return Resources.LoadAll<Sprite>("Image/Charector/" + Way + "/stand")[0];
    }

    public static RuntimeAnimatorController GetCharConrollerByIndex(int index)
    {
        return Resources.Load<RuntimeAnimatorController>("Animator/CharAnim" + index);
    }

}
