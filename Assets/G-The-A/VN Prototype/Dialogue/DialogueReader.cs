using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class DialogueReader : MonoBehaviour
{
    #region Fields
    public static DialogueReader Instance;

    // Dialogue
    [SerializeField]
    private Dialogue currentDialogue;
    
    private int currentIndex = 0;
    public bool isEnabled = false; // Показано ли окно
    public bool continueDialog = true; // Продолжить ли диалог если загружен следующий?
    public enum ReaderState : int
    {
        // Нет диалога
        Initial,
        // Диалог установлен
        Ready,
        // Диалог в процессе
        DialogueInProgress
    }
    private ReaderState state = ReaderState.Initial;

    // UI
    [SerializeField]
    private UIDocument uiDocument;

    private VisualElement textWindow;
    private Label textLabel;
    #endregion

    #region Init
    private void Awake()
    {
        if (Instance == null && Instance != this)
            Instance = this;

        SceneControllerSingleton.OnSceneLoadingStarted += Disable;
        SceneControllerSingleton.OnSceneLoadingFinished += Enable;
    }
    private void OnDestroy()
    {
        SceneControllerSingleton.OnSceneLoadingStarted -= Disable;
        SceneControllerSingleton.OnSceneLoadingFinished -= Enable;
    }
    void Start()
    {
        var root = uiDocument.rootVisualElement;
        
        textLabel = uiDocument.rootVisualElement.Q<Label>("text-label");

        textWindow = root.Q<VisualElement>("text-block");
        textWindow.SetEnabled(isEnabled);
        
        state = currentDialogue ? ReaderState.Ready : ReaderState.Initial;
    }
    #endregion

    #region Public
    public void Advance(InputAction.CallbackContext value)
    {
        if (isEnabled && value.canceled)
        {
            Instance.NextDialogueData();
        }
    }
    public void Enable()
    {
        isEnabled = true;
        
        if(textWindow != null) 
            textWindow.SetEnabled(isEnabled);
    }
    public void Disable()
    {
        isEnabled = false;

        if (textWindow != null)
            textWindow.SetEnabled(isEnabled);
    }
    public static void SetDialogue(Dialogue dialogue)
    {
        Instance.state = ReaderState.Ready;
        Instance.currentIndex = 0;
        Instance.currentDialogue = dialogue;

        if (Instance.continueDialog)
        {
            Instance.NextDialogueData();
        }
    }
    public static void SetContinue(bool continueDialog)
    {
        Instance.continueDialog = continueDialog;
    }
    #endregion

    #region Private
    private void DialogueDataAppear()
    {
        Debug.Log($"DataAppear - idx{currentIndex}");
        
        if(currentIndex == -1)
        {
            Debug.LogError("Index is -1 for some reason");
            return;
        }

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
                Debug.Log($"Curr index {currentIndex} of {currentDialogue.dialogueDatas.Count}");
                ++currentIndex;

                // Если последний
                if (currentIndex >= currentDialogue.dialogueDatas.Count)
                {
                    Debug.Log("Reached dialogue end");
                    DialogueDataDisappear(true);
                    
                    currentIndex = -1;
                    state = ReaderState.Initial;

                    var currentDialogueTemp = currentDialogue;
                    currentDialogue = null;
                    textLabel.text = "";

                    // Вызываем события завершения диалога
                    foreach (var endEvent in currentDialogueTemp.endEvents)
                        endEvent?.Invoke();
                    
                    break;
                }

                
                DialogueDataDisappear(true);

                DialogueDataAppear();
                Debug.Log($"Dialogue progressed, curr index {currentIndex} of {currentDialogue.dialogueDatas.Count}");
                break;
            }
            case ReaderState.Initial:
            {
                Debug.LogError("Reader dialogue not set!");
                break;
            }
        }
    }
    #endregion
}
