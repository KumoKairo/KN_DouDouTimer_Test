using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class PersistenceLayer
{
    private const string SaveKey = "Timers";
    
    [Serializable]
    private class ArrayWrapper
    {
        public List<Timer> timers;
    }
    
    [CanBeNull]
    public List<Timer> TryLoad()
    {
        if (PlayerPrefs.HasKey(SaveKey))
        {
            var wrapper = JsonUtility.FromJson<ArrayWrapper>(PlayerPrefs.GetString(SaveKey));
            return wrapper.timers;
        }
        else
        {
            return null;
        }
    }

    public void Save(List<Timer> timers)
    {
        var wrapper = new ArrayWrapper
        {
            timers = timers
        };
        
        var serializedTimers = JsonUtility.ToJson(wrapper);
        Debug.Log(serializedTimers);
        PlayerPrefs.SetString(SaveKey, serializedTimers);
    }
}
