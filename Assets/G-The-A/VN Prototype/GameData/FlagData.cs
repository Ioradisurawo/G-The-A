using System;
using UnityEngine;

[Serializable]
public class FlagData
{
    [SerializeField] public Flag flag;
    [SerializeField] public int value;

    // Конструктор копирования
    public FlagData(FlagData flagData)
    {
        flag = flagData.flag;
        value = flagData.value;
    }

    // Каким способом эта FlagData будет менять значение другой FlagData
    [Serializable]
    public enum ModifyMode
    {
        Replace,
        Add,
        Substract,
        Multiply,
        Divide
    }
    [SerializeField] public ModifyMode modifyMode;
    public int ModifyOtherValue(int otherValue)
    {
        switch (modifyMode)
        {
            case ModifyMode.Replace:
            {
                otherValue = value;
                break;
            }
            case ModifyMode.Add:
            {
                otherValue += value;
                break;
            }
            case ModifyMode.Substract:
            {
                otherValue -= value;
                break;
            }
            case ModifyMode.Multiply:
            {
                otherValue *= value;
                break;
            }
            case ModifyMode.Divide:
            {
                if (otherValue != 0)
                    otherValue /= value;
                else
                    Debug.Log("Divison by zero!");
                break;
            }
        }

        return otherValue;
    }
}