using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class CharacterConfig : MonoBehaviour
{
    [SerializeField]
    private List<CharacterData> CharList;

    public List<CharacterData> GetAllList()
    {
        return CharList;
    }

    public List<CharacterData> GetCharListByPos(CharacterData.PosEnum pos)
    {
        List<CharacterData> arr = new List<CharacterData> { };
        foreach (CharacterData item in CharList)
        {
            if (item.m_charPos == pos)
            {
                arr.Add(item);
            }
        }
        return arr;
    }

    public CharacterData GetCharByID(int id)
    {
        CharacterData temp = null;
        foreach (CharacterData item in CharList)
        {
            if (item.id == id)
            {
                temp = item;
                break;
            }
        }
        return temp;
    }
}
