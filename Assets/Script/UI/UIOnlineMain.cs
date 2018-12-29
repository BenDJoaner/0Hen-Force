using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIOnlineMain : MonoBehaviour
{
    public Slider team_1_Slider;
    public Slider team_2_Slider;
    public Text countDownText;
    public GameObject[] pice_1_Arr;
    public GameObject[] pice_2_Arr;

    float team_1_Num;
    float team_2_Num;

    public float countDown = 0;

    Common.GameMode currentStep = (Common.GameMode)1;

    // float MaxValue;
    // Start is called before the first frame update
    void Start()
    {
        // MaxValue = team_1_Slider.maxValue;
        OnValueChange();
    }

    // Update is called once per frame
    void Update()
    {
        if (countDown > 0)
        {
            countDown -= Time.deltaTime;
            countDownText.text = Common.ScontToTime(countDown);
        }

    }

    public void OnModeChange(int index)
    {
        currentStep = (Common.GameMode)index;
        countDown = Common.timeList[index];

        if (index != 0)
            for (int i = 1; i < pice_1_Arr.Length; i++)
            {
                pice_1_Arr[i - 1].gameObject.SetActive(false);
                pice_2_Arr[i - 1].gameObject.SetActive(false);
            }
    }

    public void OnAddEnegy(int team, float num)
    {
        switch (team)
        {
            case 1:
                team_1_Num = num;
                break;
            case 2:
                team_2_Num = num;
                break;
        }
        OnValueChange();
    }

    void OnValueChange()
    {
        team_1_Slider.value = team_1_Num;
        team_2_Slider.value = team_2_Num;
        // print("OnValueChange:" + team_1_Num + "/" + team_2_Num);
        for (int i = 1; i < pice_1_Arr.Length; i++)
        {
            pice_1_Arr[i - 1].transform.Find("point").gameObject.SetActive(team_1_Num >= i);
            pice_2_Arr[i - 1].transform.Find("point").gameObject.SetActive(team_2_Num >= i);
        }
    }
}
