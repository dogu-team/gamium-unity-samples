using System;
using Data.Static;
using UnityEngine;


[Serializable]
public class PlayerCharacterData
{
    public string uuid = System.Guid.NewGuid().ToString();
    public int characterId = 0;
    public string nickname = string.Empty;

    public long Experience
    {
        get { return experience; }
        set
        {
            experience = value;
            var befLevel = level;
            GetLevelInfo(out LevelInfo currentLevelInfo, out LevelInfo nextLevelInfo);
            level = currentLevelInfo.level;
            if (level != befLevel)
            {
                OnLevelChanged?.Invoke(currentLevelInfo);
            }
        }
    }

    private long experience = 0;
    private int level = 0;
    [NonSerialized] public Action<LevelInfo> OnLevelChanged;

    public void GetLevelInfo(out LevelInfo currentLevelInfo, out LevelInfo nextLevelInfo)
    {
        LevelInfoList.Instance.GetLevelInfoFromExp(Experience, out currentLevelInfo, out nextLevelInfo);
    }

    public float GetExpPercent()
    {
        GetLevelInfo(out LevelInfo currentLevelInfo, out LevelInfo nextLevelInfo);
        return LevelInfoList.Instance.GetExpPercent(Experience, currentLevelInfo, nextLevelInfo);
    }
}