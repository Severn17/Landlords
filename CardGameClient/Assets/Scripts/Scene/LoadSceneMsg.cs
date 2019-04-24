using System;
using System.Collections.Generic;


public class LoadSceneMsg
{
    public int SceneBuildIndex;
    public string SceneBuildName;
    public Action OnSceneLoaded;

    public LoadSceneMsg()
    {
        this.SceneBuildIndex = -1;
        this.SceneBuildName = null;
        this.OnSceneLoaded = null;
    }
    public LoadSceneMsg(int index, Action loaded)
    {
        this.SceneBuildIndex = index;
        this.SceneBuildName = null;
        this.OnSceneLoaded = loaded;
    }
    public void Change(string name, Action loaded)
    {
        this.SceneBuildIndex = -1;
        this.SceneBuildName = name;
        this.OnSceneLoaded = loaded;
    }
}
