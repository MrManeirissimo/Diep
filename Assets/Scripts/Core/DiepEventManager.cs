using System.Collections.Generic;

using UnityEngine.Events;
using UnityEngine;

public class DiepEventManager : SingletonBehavior<DiepEventManager> {
    public delegate DiepEventArgs EventArgsBuilder();

    Dictionary<string, UnityEvent<object, DiepEventArgs>> eventDictionary;

    public void ListenToEvent(string eventTag, UnityAction<object, DiepEventArgs> callback) {
        if (!eventDictionary.ContainsKey(eventTag)) {
            eventDictionary.Add(eventTag, new UnityEvent<object, DiepEventArgs>());
        }

        eventDictionary[eventTag].AddListener(callback);
    }

    public void UnlistenToEvent(string eventTag, UnityAction<object, DiepEventArgs> callback) {
        if (eventDictionary.ContainsKey(eventTag)) {
            eventDictionary[eventTag].RemoveListener(callback);
        }
    }

    public void Dispatch(string eventTag, object sender, DiepEventArgs eArgs) {
        if (!eventDictionary.ContainsKey(eventTag)) {
            eventDictionary.Add(eventTag, new UnityEvent<object, DiepEventArgs>());
        }

        eventDictionary[eventTag].Invoke(sender, eArgs);
    }
    public void Dispatch(string eventTag, object sender, EventArgsBuilder builderCallback) {
        Dispatch(eventTag, sender, builderCallback());
    }

    private void Awake() {
        eventDictionary = new Dictionary<string, UnityEvent<object, DiepEventArgs>>();
    }
}