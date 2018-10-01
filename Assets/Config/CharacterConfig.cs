using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class CharacterConfig : MonoBehaviour
{
    [SerializeField]
    private List<CharacterData> CharList;

    public List<CharacterData> GetAllList(){
        return CharList;
    }

    public List<CharacterData> GetSlipyList(){
        List<CharacterData> arr = null;
        foreach (CharacterData  item in CharList)
        {
            if(item.m_charPos == CharacterData.PosEnum.SLIPPY){
                arr.Add(item);
            }
        }
        return arr;
    }

    public List<CharacterData> GetAttackList(){
        List<CharacterData> arr = null;
        foreach (CharacterData  item in CharList)
        {
            if(item.m_charPos == CharacterData.PosEnum.ATTACK){
                arr.Add(item);
            }
        }
        return arr;
    }
    public List<CharacterData> GetSupportList(){
        List<CharacterData> arr = null;
        foreach (CharacterData  item in CharList)
        {
            if(item.m_charPos == CharacterData.PosEnum.SUPPORT){
                arr.Add(item);
            }
        }
        return arr;
    }

    public CharacterData GetCharByID(int id){
        CharacterData temp = null;
        foreach (CharacterData  item in CharList)
        {
            if(item.m_charPos == CharacterData.PosEnum.SUPPORT){
                temp = item;
                break;
            }
        }
        return temp;
    }

}
