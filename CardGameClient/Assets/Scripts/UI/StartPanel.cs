using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartPanel : UIBase
{

    private void Awake()
    {
        Bind(UIEvent.START_PANEL_ACTIVE);
    }

    public override void Execute(int eventCode, object message)
    {
        switch (eventCode)
        {
            case UIEvent.START_PANEL_ACTIVE:
                setPanelActive((bool)message);
                break;
            default:
                break;
        }
    }

    private Button btnLogin;
    private Button btnClose;
    private InputField inputAccount;
    private InputField inputPassword;

    // Use this for initialization
    void Start()
    {
        btnLogin = transform.Find("btnLogin").GetComponent<Button>();
        btnClose = transform.Find("btnClose").GetComponent<Button>();
        inputAccount = transform.Find("inputAccount").GetComponent<InputField>();
        inputPassword = transform.Find("inputPassword").GetComponent<InputField>();

        btnLogin.onClick.AddListener(loginClick);
        btnClose.onClick.AddListener(closeClick);

        //面板需要默认隐藏
        setPanelActive(false);
    }

    public override void OnDestroy()
    {
        base.OnDestroy();

        btnLogin.onClick.RemoveListener(loginClick);
        btnClose.onClick.RemoveListener(closeClick);
    }

    /// <summary>
    /// 登录按钮的点击事件处理
    /// </summary>
    private void loginClick()
    {
        if (string.IsNullOrEmpty(inputAccount.text))
            return;
        if (string.IsNullOrEmpty(inputPassword.text)
            || inputPassword.text.Length < 4
            || inputPassword.text.Length > 16)
            return;
        
        //需要和服务器交互了
        //TODO
    }

    private void closeClick()
    {
        setPanelActive(false);
    }

}
