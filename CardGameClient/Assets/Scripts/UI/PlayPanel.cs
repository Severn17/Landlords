using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayPanel : UIBase
{
    private Button btnStart;
    private Button btnRegist;

    // Use this for initialization
    void Start()
    {
        btnStart = transform.Find("btnStart").GetComponent<Button>();
        btnRegist = transform.Find("btnRegist").GetComponent<Button>();

        btnStart.onClick.AddListener(startClick);
        btnRegist.onClick.AddListener(registClick);
    }

    public override void OnDestroy()
    {
        base.OnDestroy();

        btnStart.onClick.RemoveAllListeners();
        btnRegist.onClick.RemoveAllListeners();
    }

    private void startClick()
    {
        Dispatch(AreaCode.UI, UIEvent.START_PANEL_ACTIVE, true);
        //GameObject.Find("").gameObject.SetActive(true);
    }


    private void registClick()
    {
        Dispatch(AreaCode.UI, UIEvent.REGIST_PANEL_ACTIVE, true);
    }

}
