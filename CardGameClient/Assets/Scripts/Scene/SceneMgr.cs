using System;
using System.Collections.Generic;
using UnityEngine.SceneManagement;


public class SceneMgr : ManagerBase
{
    public static SceneMgr Instance = null;
    private void Awake()
    {
        Instance = this;

        SceneManager.sceneLoaded += SceneManage_sceneLoaded;

        Add(SceneEvent.LOAD_SCENE, this);
    }



    public override void Execute(int eventCode, object message)
    {
        switch (eventCode)
        {
            case SceneEvent.LOAD_SCENE:
                LoadSceneMsg sceneIndex = (LoadSceneMsg)message;
                loadScene(sceneIndex);
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// 临时变量
    /// </summary>
    public Action OnSceneLoaded = null;

    /// <summary>
    /// 加载场景
    /// </summary>
    /// <param name="sceneBuildIndex"></param>
    private void loadScene(LoadSceneMsg msg)
    {
        if (msg.SceneBuildIndex != -1)
            SceneManager.LoadScene(msg.SceneBuildIndex);
        if (msg.SceneBuildName != null)
            SceneManager.LoadScene(msg.SceneBuildName);
        if (msg.OnSceneLoaded != null)
            OnSceneLoaded = msg.OnSceneLoaded;
    }
    /// <summary>
    /// 当场景加载完成的时候调用
    /// </summary>
    /// <param name="arg0"></param>
    /// <param name="arg1"></param>
    private void SceneManage_sceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        if (OnSceneLoaded != null)
            OnSceneLoaded();
    }
}
