using Protocol;
using Protocol.Code;
using Protocol.Dto;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RegistPanel : UIBase
{
    private void Awake()
    {
        Bind(UIEvent.REGIST_PANEL_ACTIVE);
    }

    public override void Execute(int eventCode, object message)
    {
        switch (eventCode)
        {
            case UIEvent.REGIST_PANEL_ACTIVE:
                setPanelActive((bool)message);
                break;
            default:
                break;
        }
    }

    private Button btnReigist;
    private Button btnClose;
    private InputField inputAccount;
    private InputField inputPassword;
    private InputField inputRepeat;

    // Use this for initialization
    void Start()
    {
        btnReigist = transform.Find("btnReigist").GetComponent<Button>();
        btnClose = transform.Find("btnClose").GetComponent<Button>();
        inputAccount = transform.Find("inputAccount").GetComponent<InputField>();
        inputPassword = transform.Find("inputPassword").GetComponent<InputField>();
        inputRepeat = transform.Find("inputRepeat").GetComponent<InputField>();

        btnClose.onClick.AddListener(closeClick);
        btnReigist.onClick.AddListener(registClick);

        setPanelActive(false);
    }

    public override void OnDestroy()
    {
        base.OnDestroy();

        btnClose.onClick.RemoveListener(closeClick);
        btnReigist.onClick.RemoveListener(registClick);
    }

    AccountDto dto = new AccountDto();
    SocketMsg socketMsg = new SocketMsg();

    /// <summary>
    /// 注册按钮的点击事件处理
    /// </summary>
    private void registClick()
    {
        if (string.IsNullOrEmpty(inputAccount.text))
            return;
        if (string.IsNullOrEmpty(inputPassword.text)
            || inputPassword.text.Length < 4
            || inputPassword.text.Length > 16)
            return;
        if (string.IsNullOrEmpty(inputRepeat.text)
            || inputRepeat.text != inputPassword.text)
            return;

        dto.account = inputAccount.text;
        dto.password = inputPassword.text;
        socketMsg.OpCode = OpCode.ACCOUNT;
        socketMsg.SubCode = AccountCode.REGIST_CREQ;
        socketMsg.Value = dto;
        //AccountDto dto = new AccountDto(inputAccount.text, inputPassword.text);
        //SocketMsg socketMsg = new SocketMsg(OpCode.ACCOUNT, AccountCode.REGIST_CREQ, dto);
        Dispatch(AreaCode.NET, 0, socketMsg);
    }

    private void closeClick()
    {
        setPanelActive(false);
    }
}
