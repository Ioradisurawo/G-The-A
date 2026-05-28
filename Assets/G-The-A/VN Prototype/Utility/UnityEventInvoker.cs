using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class UnityEventInvoker : MonoBehaviour
{
    public UnityEvent unityAction;
    [ContextMenu("Invoke Unity Action")]
    public void InvokeUnityAction()
    {
        unityAction?.Invoke();
    }
}