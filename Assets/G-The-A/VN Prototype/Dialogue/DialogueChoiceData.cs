using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class DialogueChoiceData
{
    [SerializeField]
    public string text;

    public List<UnityEvent> eventList;
}