using UnityEngine;

public class DialogueMapper : MonoBehaviour
{
    [SerializeField] 
    private Dialogue firstDialogue;

    private void Start()
    {
        if(DialogueReader.Instance == null)
        {
            Debug.LogError("Dialogue reader doesn't exist!");
            return;
        }

        DialogueReader.SetDialogue(firstDialogue);
        Debug.Log("First scene dialogue set!");
    }
}
