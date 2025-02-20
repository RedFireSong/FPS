using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 给每个敌人分不同路线
/// </summary>
public class WayPointMager : MonoBehaviour
{
    private static WayPointMager _instance;
    public static WayPointMager Instance {
        get 
        { 
            return _instance; 
        } 
    }

    public List<int> usingIndex = new List<int>();
    public List<int> rawIndex = new List<int>();


    private void Awake()
    {
        _instance = this;
        var tempCount = rawIndex.Count;
        for (int i = 0; i < tempCount; i++)
        {
            var index = Random.Range(0, rawIndex.Count);
            usingIndex.Add(rawIndex[index]);
            rawIndex.RemoveAt(index);
        }
    }
}
