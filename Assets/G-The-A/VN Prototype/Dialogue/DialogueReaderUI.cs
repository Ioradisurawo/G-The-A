using UnityEngine;
using UnityEngine.UIElements;

public class DialogueReaderUI : MonoBehaviour
{
    [SerializeField]
    private DialogueReader reader;

    // UI
    [SerializeField]
    private UIDocument uiDocument;

    private VisualElement textWindow;
    public string textWindowName = "text-block";

    [SerializeField]
    private VisualTreeAsset selectionButton;

    private VisualElement selectionWindow;
    public string selectionWindowName = "dialogue-select";

    private Label textLabel;
    public string textLabelName = "text-label";
    private void Awake()
    {
        DialogueReader.OnTextUpdate += TextUpdate;
        DialogueReader.OnChoicesAdded += AddChoiceButtons;
        DialogueReader.OnChoicesRemoved += ChoicesRemove;

        SceneControllerSingleton.OnSceneLoadingStarted += TextHide;
        SceneControllerSingleton.OnSceneLoadingFinished += TextShow;
    }
    private void OnDestroy()
    {
        SceneControllerSingleton.OnSceneLoadingStarted -= TextHide;
        SceneControllerSingleton.OnSceneLoadingFinished -= TextShow;
    }

    private void Start()
    {
        reader = DialogueReader.Instance;

        var root = uiDocument.rootVisualElement;

        textLabel = uiDocument.rootVisualElement.Q<Label>(textLabelName);

        selectionWindow = uiDocument.rootVisualElement.Q<VisualElement>(selectionWindowName);
        selectionWindow.SetEnabled(false);

        textWindow = root.Q<VisualElement>(textWindowName);
        textWindow.SetEnabled(reader.isShowText);
    }

    public void TextShow()
    {
        reader.isShowText = true;

        if (textWindow != null)
            textWindow.SetEnabled(reader.isShowText);
    }
    public void TextHide()
    {
        reader.isShowText = false;

        if (textWindow != null)
            textWindow.SetEnabled(reader.isShowText);
    }

    public void TextUpdate(string newText)
    {
        textLabel.text = newText;
    }

    public void AddChoiceButtons()
    {
        foreach (var choiceData in reader.currentChoices.dialogueChoiceDatas)
        {
            Button newButton = selectionButton.CloneTree().Q<Button>("select-button");
            newButton.text = choiceData.text;
            newButton.clicked += () => { DialogueReader.ChoiceSelect(choiceData); };
            selectionWindow.Add(newButton);
        }

        ChoicesShow();
    }
    public void ChoicesShow()
    {
        reader.isAdvanceAllowed = false;

        selectionWindow.SetEnabled(true);
        
        Debug.Log("Choice window shown!");
    }
    public void ChoicesRemove()
    {
        selectionWindow.SetEnabled(false);
        selectionWindow.Clear();

        reader.isAdvanceAllowed = true;
        
        Debug.Log("Choice window hidden!");
    }
}
