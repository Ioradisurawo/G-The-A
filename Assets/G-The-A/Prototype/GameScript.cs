using System;
using System.Collections.Generic;
using System.Text;
using Unity.Properties;
using UnityEngine;

[Serializable]
[GeneratePropertyBag]
public class Stat
{
    [Serializable]
    public enum NAME : int
    {
        Maid = 0,
        Harman_Smith = 1,
        Dan_Smith = 2,
        KAEDE_Smith = 3,
        Garcian_Smith = 4
    }
    [CreateProperty] public NAME name;
    [CreateProperty] public int value;
    [CreateProperty] public string Combine => $"{name} : {value}";
}

[GeneratePropertyBag]
public class GameScript : MonoBehaviour
{
    public List<Stat> stats = new();
    public List<ScoreEntryAffector> pool = new();
    public List<ScoreEntryAffector> stored = new();
    public int[] index_selection_pool = new int[3];

    public static event Action OnShuffled;
    public static event Action<int> OnSelect;
    public static event Action OnCountStored;

    public int shuffle_amount = 0;
    public int max_shuffle_amount = 5;

    public bool ready;

    public void DebugLogAll()
    {
        StringBuilder ss = new();
        ss.AppendLine("-STATS-");
        foreach(Stat stat in stats)
        {
            ss.AppendLine(stat.name.ToString() + " : " + stat.value);
        }

        ss.AppendLine("-SELECTION POOL AVAILABLE-");
        foreach(ScoreEntryAffector affector in pool)
        {
            ss.AppendLine(affector.name);
        }

        ss.AppendLine("-STORED-");
        if (stored.Count == 0)
            ss.AppendLine("none");
        foreach(ScoreEntryAffector affector in stored)
        {
            ss.AppendLine(affector.name);
        }

        Debug.Log(ss.ToString());
    }
    public void Shuffle()
    {
        if(ready)
        {
            Debug.LogWarning("Already shuffled");
            return;
        }

        if(shuffle_amount == max_shuffle_amount)
        {
            Debug.LogWarning($"Reached maximum shuffles: {max_shuffle_amount}");
            return;
        }

        Debug.Log("-SHUFFLE TIME-");

        int[] pool_indicies = new int[pool.Count];
        // fill
        for (int i = 0; i < pool_indicies.Length; i++)
            pool_indicies[i] = i;

        // swap
        for(int i = 0; i < pool.Count; i++)
        {
            int swap_i = UnityEngine.Random.Range(0, pool_indicies.Length - 1);
            int temp = pool_indicies[swap_i];
            pool_indicies[swap_i] = pool_indicies[i];
            pool_indicies[i] = temp;
        }

        // set in sel pool
        for (int i = 0; i < 3; i++)
        {
            index_selection_pool[i] = pool_indicies[i];
            Debug.Log("[idx_sel_pool] idx " + i + " = name: " + pool[index_selection_pool[i]].name);
        }

        shuffle_amount++;

        ready = true;

        OnShuffled?.Invoke();
    }
    public void Select(int index)
    {
        Debug.Log("-SELECTION-");
        if (!ready)
        {
            Debug.LogWarning("Not ready");
            return;
        }
        Debug.Log("index: " + index);

        stored.Add(pool[index_selection_pool[index]]);

        OnSelect?.Invoke(index);

        pool.RemoveAt(index_selection_pool[index]);

        ready = false;
    }
    public void CountStored()
    {
        Debug.Log("-COUNTING-");
        foreach(ScoreEntryAffector affector in stored)
        {
            foreach(ScoreEntryAffector.AffectedStat affectedStat in affector.affectedStats)
            {
                for(int i = 0; i < stats.Count; i++)
                {
                    if (affectedStat.stat_name == stats[i].name)
                        stats[i].value = affectedStat.value_affect;
                }
            }
        }

        OnCountStored?.Invoke();
    }
    [Obsolete] // : )
    public void MockPlay()
    {
        Debug.Log("-MOCK PLAY START-");
        DebugLogAll();
        for(int i = 0; i < 5; i++)
        {
            Shuffle();
            Select(UnityEngine.Random.Range(0, index_selection_pool.Length - 1));
        }
        CountStored();
        Debug.Log("-MOCK PLAY END-");
        DebugLogAll();
        Debug.Log("Another One?");
        DebugLogAll();

    }
}
