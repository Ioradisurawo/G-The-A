using System.Collections.Generic;
using System.Text;
using UnityEngine;
public class FlagDataHolder : MonoBehaviour
{
    [SerializeField]
    public List<FlagData> flagDatas = new();

    public enum FlagUpdateMode
    {
        Assign,
        Modify
    }

    #region CRUD
    public void AddFlagData(FlagData flagData)
    {
        var found = flagDatas.Find((FlagData other) => { return flagData.flag == other.flag; });
        if (found != null)
        //if (flagDataDict.ContainsKey(flagData.flag))
        {
            Debug.Log($"AddFlagData - flag {flagData.flag.name} already exists");
        }
        else
        {
            flagDatas.Add(new FlagData(flagData));
            //flagDataDict[flagData.flag] = flagData.value;
        }
    }
    public void UpdateFlagData(FlagData flagData, FlagUpdateMode mode)
    {
        var found = flagDatas.Find((FlagData other) => { return flagData.flag == other.flag; });
        if(found == null)
        //if (!flagDataDict.ContainsKey(flagData.flag))
        {
            Debug.Log($"UpdateFlagData - flag {flagData.flag.name} doesn't exist, adding it to list");
            AddFlagData(flagData);
            return;
        }
        else
        {
            switch(mode)
            {
                case FlagUpdateMode.Assign:
                {
                    found.value = flagData.value;
                    break;
                }
                case FlagUpdateMode.Modify:
                {
                    found.value = flagData.ModifyOtherValue(found.value);
                    break;
                }
            }
            //flagDataDict[flagData.flag] 
            //    = mode == FlagUpdateMode.Assign ? flagData.value 
            //    : flagData.ModifyOtherValue(flagDataDict[flagData.flag]);
        }
    }
    public void UpdateFlagData(FlagDataHolder flagDataHolder)
    {
        foreach(var fd in flagDataHolder.flagDatas)
        {
            // FlagData должна модифицировать дату из словаря, поэтому Modify
            UpdateFlagData(fd, FlagUpdateMode.Modify);
        }
    }
    #endregion
}
