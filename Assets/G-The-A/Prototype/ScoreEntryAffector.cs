using System;
using System.Collections.Generic;
using Unity.Properties;
using UnityEngine;


[CreateAssetMenu(fileName = "ScoreEntryAffector", menuName = "Scriptable Objects/ScoreEntryAffector")]
public class ScoreEntryAffector : ScriptableObject
{
    [Serializable]
    [GeneratePropertyBag]
    public struct AffectedStat
    {
        public Stat.NAME stat_name;
        public int value_affect;
    }

    public List<AffectedStat> affectedStats;
}
