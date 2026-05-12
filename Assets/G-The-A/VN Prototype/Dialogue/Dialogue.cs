using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "Dialogue", menuName = "Scriptable Objects/Dialogue")]
public class Dialogue : ScriptableObject
{
    public List<DialogueData> dialogueDatas;
    public List<UnityEvent> beginEvents;
    public List<UnityEvent> endEvents;
}
