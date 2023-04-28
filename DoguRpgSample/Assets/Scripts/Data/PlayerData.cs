using System;

[Serializable]
public class PlayerData
{
    public string uuid = System.Guid.NewGuid().ToString();
    public string nickName = string.Empty;
    public bool isLogin = false;
}