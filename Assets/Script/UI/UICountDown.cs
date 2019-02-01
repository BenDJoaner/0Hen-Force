using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICountDown : MonoBehaviour
{
    public Text UIText;
    public float countdown;
    UISelectChar ui_select;
    // Update is called once per frame
    void Update()
    {
        if (countdown > 0)
        {
            countdown -= Time.deltaTime;
            UIText.text = (int)countdown + "<size=100>." + (int)((countdown % 1) * 100) + "</size>";
        }
        else
        {
            ui_select.gameObject.SetActive(true);
            this.gameObject.SetActive(false);
        }
    }

    public void SetCountDownDown(int second, UISelectChar ui)
    {
        countdown = second;
        ui_select = ui;
    }
}

