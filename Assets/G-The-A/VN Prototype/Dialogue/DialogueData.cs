using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine.Events;

[Serializable]
public class DialogueData
{
    [UnityEngine.SerializeField]
    public string speaker;
    [UnityEngine.SerializeField]
    [UnityEngine.TextArea(1,5)]
    public string text;

    public List<UnityEvent> appearEvents;
    public List<UnityEvent> disappearEvents;
}
