using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class DialogueReader : MonoBehaviour
{
    [SerializeField]
    public Dialogue currentDialogue;
    
    public int currentIndex = 0;

    public enum ReaderState : int
    {
        // Нет диалога
        Initial,
        // Диалог установлен
        Ready,
        // Диалог в процессе
        DialogueInProgress
    }
    public ReaderState state = ReaderState.Initial;

    public UIDocument uiDocument;
    public Label textLabel;

    void Start()
    {
        currentIndex = 0;
        textLabel = uiDocument.rootVisualElement.Q<Label>("text-label");
        state = ReaderState.Ready;
    }
    public void Advance(InputAction.CallbackContext value)
    {
        if (value.canceled)
        {
            NextDialogueData();
        }
    }
    private void DialogueDataAppear()
    {
        // События появления диалогдаты
        foreach (var appearEvent in currentDialogue.dialogueDatas[currentIndex].appearEvents)
            appearEvent?.Invoke();

        textLabel.text = currentDialogue.dialogueDatas[currentIndex].text;
    }
    private void DialogueDataDisappear(bool forPrevious)
    {
        // События исчезновения диалогдаты
        foreach (var disappearEvent in currentDialogue.dialogueDatas[currentIndex - (forPrevious ? 1 : 0)].disappearEvents)
            disappearEvent?.Invoke();
    }
    private void NextDialogueData()
    {
        switch (state)
        {
            // Если готов, диалог не начался
            case ReaderState.Ready:
            {
               // Вызываем события начала диалога
               foreach (var beginEvent in currentDialogue.beginEvents)
                   beginEvent?.Invoke();

               // Начинаем диалог
               state = ReaderState.DialogueInProgress;

               currentIndex = 0;

               DialogueDataAppear();
               
               break;
            }
            case ReaderState.DialogueInProgress:
            {
                if((currentIndex < currentDialogue.dialogueDatas.Count) == false)
                {
                    DialogueDataDisappear(false);
                    
                    currentIndex = -1;
                    state = ReaderState.Initial;
                    
                    // Вызываем события завершения диалога
                    foreach (var endEvent in currentDialogue.endEvents)
                        endEvent?.Invoke();
                    
                    break;
                }

                ++currentIndex;
                
                DialogueDataDisappear(true);

                DialogueDataAppear();

                break;
            }
            case ReaderState.Initial:
            {
                Debug.LogError("Reader dialogue not set!");
                break;
            }
        }
    }
    public void SetDialogue(Dialogue dialogue)
    {
        state = ReaderState.Ready;
        currentDialogue = dialogue;
    }
}
