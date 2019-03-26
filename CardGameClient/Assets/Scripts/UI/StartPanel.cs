using Protocol;
using Protocol.Code;
using Protocol.Dto;
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
    private PromptMsg promptMsg;

    // Use this for initialization
    void Start()
    {
        btnLogin = transform.Find("btnLogin").GetComponent<Button>();
        btnClose = transform.Find("btnClose").GetComponent<Button>();
        inputAccount = transform.Find("inputAccount").GetComponent<InputField>();
        inputPassword = transform.Find("inputPassword").GetComponent<InputField>();

        btnLogin.onClick.AddListener(loginClick);
        btnClose.onClick.AddListener(closeClick);

        promptMsg = new PromptMsg();

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
        {
            promptMsg.Change("登陆的用户名不能为空！", Color.red);
            Dispatch(AreaCode.UI, UIEvent.PROMPT_MSG, promptMsg);  
            return;
        }
        if (string.IsNullOrEmpty(inputPassword.text))
        {
            promptMsg.Change("登陆的密码不能为空！", Color.red);
            Dispatch(AreaCode.UI, UIEvent.PROMPT_MSG, promptMsg);
            return;
        }
            if (inputPassword.text.Length < 4 || inputPassword.text.Length > 16)
        {
            promptMsg.Change("登陆的密码长度不合法，应该在4-16个字符之内！", Color.red);
            Dispatch(AreaCode.UI, UIEvent.PROMPT_MSG, promptMsg);
            return;
        }

        AccountDto dto = new AccountDto(inputAccount.text, inputPassword.text);
        SocketMsg socketMsg = new SocketMsg(OpCode.ACCOUNT, AccountCode.LOGIN, dto);
        Dispatch(AreaCode.NET, 0, socketMsg);
    }

    private void closeClick()
    {
        setPanelActive(false);
    }

}
