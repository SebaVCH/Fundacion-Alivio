using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class AudioTimedUnityEvent
{
    public float time;
    public UnityEvent onTrigger;
    [HideInInspector] public bool triggered;
}
