using System;
using UnityEngine;

[Serializable]
[CreateAssetMenu(menuName = "Game Data/Flag")]
public class Flag : ScriptableObject
{
    [TextArea(0, 10)]
    public string description = "";
}