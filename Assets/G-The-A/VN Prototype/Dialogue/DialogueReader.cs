using System;
using System.Collections.Generic;
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
    public Dialogue currentDialogue;
    public DialogueChoices currentChoices;
    
    private int currentIndex = 0;
    public bool isShowText = false; // Показано ли окно
    public bool isAdvanceAllowed = true;
    public bool isContinueDialog = true; // Продолжить ли диалог если загружен следующий?
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
    
    public static Action<string> OnTextUpdate;
    private static Action OnAdvance;
    public static Action OnChoicesAdded;
    public static Action OnChoicesRemoved;
    #endregion

    #region Init
    private void Awake()
    {
        if (Instance == null && Instance != this)
            Instance = this;
    }
    void Start()
    {
        state = currentDialogue ? ReaderState.Ready : ReaderState.Initial;
    }
    #endregion

    #region Public
    public void Advance(InputAction.CallbackContext value)
    {
        if (isAdvanceAllowed && isShowText && value.canceled)
        {
            Instance.NextDialogueData();
        }
        OnAdvance?.Invoke();
    }

    public static void ChoiceSelect(DialogueChoiceData choiceData)
    {
        Debug.Log($"Selected choice {choiceData.text}!");
        foreach(var unityEvent in choiceData.eventList)
        {
            unityEvent?.Invoke();
        }

        ChoicesRemove();
    }

    public static void ChoicesAdd(DialogueChoices dialogueChoices)
    {
        Instance.currentChoices = dialogueChoices;
        OnChoicesAdded?.Invoke();
    }
    // Hides and removes choices
    public static void ChoicesRemove()
    {
        Instance.currentChoices = null;
        OnChoicesRemoved?.Invoke();
    }

    public static void SetDialogue(Dialogue dialogue)
    {
        Instance.state = ReaderState.Ready;
        Instance.currentIndex = 0;
        Instance.currentDialogue = dialogue;

        if (Instance.isContinueDialog)
        {
            Instance.NextDialogueData();
        }
    }
    public static void SetContinue(bool continueDialog)
    {
        Instance.isContinueDialog = continueDialog;
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

        OnTextUpdate(currentDialogue.dialogueDatas[currentIndex].text);
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
                    OnTextUpdate?.Invoke("");

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
