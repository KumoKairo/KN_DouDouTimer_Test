using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class PersistenceLayer
{
    private const string SaveKey = "Timers";
    
    // Unity doesn't support first-class Array or List serialization
    // This is the only reason we need to wrap this class around
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
        PlayerPrefs.SetString(SaveKey, serializedTimers);
    }
}
