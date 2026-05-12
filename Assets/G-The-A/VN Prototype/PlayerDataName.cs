using System;
using UnityEngine;

// Расширяемый Enum
[Serializable]
public class PlayerDataName
{
    [UnityEngine.SerializeField]
    public int value;

    public PlayerDataName(int value)
    {
        this.value = value;
    }

    // Перегрузки операторов

    public static implicit operator int(PlayerDataName name)
    {
        return name.value;
    }

    public static implicit operator PlayerDataName(int value)
    {
        return new PlayerDataName(value);
    }
};