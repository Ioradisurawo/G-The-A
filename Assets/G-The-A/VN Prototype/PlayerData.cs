using System;
using System.Collections.Generic;
using System.Text;
[Serializable]
public class PlayerData
{
    [UnityEngine.SerializeField]
    public readonly PlayerDataName dataName;

    [UnityEngine.SerializeField]
    public int value;

    public PlayerData(PlayerDataName name)
    {
        dataName = name;
        value = 0;
    }
}
