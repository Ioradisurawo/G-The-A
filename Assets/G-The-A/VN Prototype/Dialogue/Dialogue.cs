using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Events;

//[CreateAssetMenu(fileName = "Dialogue", menuName = "Scriptable Objects/Dialogue")]
public class Dialogue : MonoBehaviour
{
    [Header("Реплики")]
    public List<DialogueData> dialogueDatas;
    [Header("События начала диалога")]
    public List<UnityEvent> beginEvents;
    [Header("События конца диалога")]
    public List<UnityEvent> endEvents;
}
