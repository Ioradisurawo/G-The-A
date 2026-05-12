using System;
using System.Collections.Generic;
using UnityEngine;
public class PlayerDataHolder : MonoBehaviour
{
    //public Dictionary<PlayerDataName, PlayerData> playerDataDict = new();
    public List<PlayerData> playerDataDict = new();

    public void AddData(PlayerDataName dataName)
    {
        PlayerData found = playerDataDict.Find((PlayerData other) => { return other.dataName == dataName; });
        if (found != null)
        //if (playerDataDict.ContainsKey(dataName))
        {
            Debug.LogWarning("AddData fail - Data name exists : " + dataName);
            return;
        }

        //playerDataDict.Add(dataName, new PlayerData(dataName));
        playerDataDict.Add(new PlayerData(dataName));
    }
    public void RemoveData(PlayerDataName dataName)
    {
        PlayerData found = playerDataDict.Find((PlayerData other) => { return other.dataName == dataName; });
        if (found == null)
        //if (!playerDataDict.ContainsKey(dataName))
        {
            Debug.LogWarning("RemoveData fail - Data name doesn't exist : " + dataName);
            return;
        }
        playerDataDict.Remove(found);
    }
    public void UpdateData(PlayerDataName dataName, int value, Func
        <int, // our value
        int, // data value
        int> // func return type
        func)
    {
        PlayerData found = playerDataDict.Find((PlayerData other) => { return other.dataName == dataName; });
        if(found == null)
        //if (!playerDataDict.ContainsKey(dataName))
        {
            Debug.LogWarning("RemoveData fail - Data name doesn't exist : " + dataName);
            return;
        }
        //playerDataDict[dataName].value = func(value, playerDataDict[dataName].value);
        found.value = func(value, playerDataDict[dataName].value);
    }
    public int GetDataValue(PlayerDataName dataName)
    {
        PlayerData found = playerDataDict.Find((PlayerData other) => { return other.dataName == dataName; });
        if(found == null)
        //if (!playerDataDict.ContainsKey(dataName))
        {
            Debug.LogWarning("RemoveData fail - Data name doesn't exist : " + dataName);
            return -1;
        }

        //return playerDataDict[dataName].value;
        return found.value;
    }
}
