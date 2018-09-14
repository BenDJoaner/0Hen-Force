using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHostItem : MonoBehaviour {
    public Text nameText;
    public Text ipText;
    public Button selfBtn;

    public void Init(string name,string ip,LobbyJoinPanel panel)
    {
        nameText.text = name;
        ipText.text = ip;

        selfBtn.onClick.AddListener(delegate ()
        {
            panel.OnJoinHostServer(ip);
        });
        transform.localScale = new Vector3(1, 1, 1);
    }
}
