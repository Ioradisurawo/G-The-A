using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class UI : MonoBehaviour
{
    [Header("Famous gameScript")]
    public GameScript gameScript; // TODO Разбить скрипт на части

    [Header("UXML Templates")]
    public VisualTreeAsset rowAsset;
    public VisualTreeAsset dataAsset;
    public VisualTreeAsset buttonAsset;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip clipConfirm;
    public AudioClip clipProgress;
    public AudioClip clipFinish;
    public AudioClip clipHover;

    private VisualElement root;
    private Dictionary<string, Button> buttonsDict = new();
    private Label status_text;

    void Start()
    {
        root = GetComponent<UIDocument>().rootVisualElement;

        root.SetEnabled(false);

        AddRows();

        // status text
        status_text = root.Q<Label>("status-text");
        status_text.text = "> начало.";

        MapButtons();
    }

    void UpdateStored()
    {
        var data_instance = dataAsset.CloneTree();
        data_instance.SetEnabled(false);

        int last_idx = gameScript.stored.Count - 1;
        
        StringBuilder sb = new();

        data_instance.Q<Label>("name").text = gameScript.stored[last_idx].name;

        foreach (var stat in gameScript.stored[last_idx].affectedStats)
        {
            sb.Append($"{stat.stat_name} = {stat.value_affect}//\n");
        }

        data_instance.Q<Label>("value").text = sb.ToString();

        var data_row_instance = root.Q("data-row-stored");
        
        data_row_instance.Q("list").Add(data_instance);
        
        data_instance.SetEnabled(true);

        // sound
        data_instance.RegisterCallback<MouseEnterEvent>((evt) => {
            if (clipHover)
                audioSource.PlayOneShot(clipHover);
        });
    }
    void ButtonAdvance()
    {
        gameScript.Shuffle();
        if(gameScript.shuffle_amount == gameScript.max_shuffle_amount)
        {
            buttonsDict["advance"].text = "Завершить";
            buttonsDict["advance"].clicked -= ButtonAdvance;
            buttonsDict["advance"].clicked += ButtonFinish;
        }
    }
    IEnumerator WaitThenDo(float waitAmount, Action action) // Вынести в отдельное
    {
        yield return new WaitForSeconds(waitAmount);
        action();
    }
    void ButtonFinish()
    {
        gameScript.CountStored();
        buttonsDict["advance"].text = "Перезапуск";
        buttonsDict["advance"].clicked -= ButtonFinish;
        buttonsDict["advance"].clicked += ButtonRestart;
        StartCoroutine(WaitThenDo(2, () => {
            buttonsDict["advance"].SetEnabled(true);
        }));
    }
    void ButtonRestart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    void ButtonSelect(int index)
    {
        gameScript.Select(index);
    }
    void SetEnabledSelectButtons(bool value)
    {
        for (int i = 0; i < 3; i++)
            buttonsDict[$"select-{i}"].SetEnabled(value);
    }
    void SetTextSelectButtons()
    {
        for (int i = 0; i < 3; i++)
            buttonsDict[$"select-{i}"].text = gameScript.pool[gameScript.index_selection_pool[i]].name;
    }
    void MapButtons()
    {
        // Buttons
        var button_row = root.Q("button-row");

        // main button
        var instance_button = buttonAsset.CloneTree().Q<Button>("action-button");

        instance_button.text = "начать";
        instance_button.clicked += ()=> {
            if (clipHover)
                audioSource.PlayOneShot(clipConfirm); 
            ButtonAdvance(); 
        };

        button_row.Add(instance_button);

        buttonsDict["advance"] = instance_button;

        // select buttons
        for (int i = 0; i < 3; i++)
        {
            instance_button = buttonAsset.CloneTree().Q<Button>("action-button");
            
            instance_button.text = $"-{i}-";
            instance_button.name = $"{i}";
            Debug.Log($"Added sel button idx ${i}");

            int captured_idx = i; // т.к. i из цикла фор по ссылке передаётся
            instance_button.clicked += () => {
                if (clipHover)
                    audioSource.PlayOneShot(clipConfirm);

                ButtonSelect(captured_idx); 
            }; 
            // Might not work? Stuff is weird but ok......
            // It didn't work when I used RegisterCallback
            // now it works with cached i

            button_row.Add(instance_button);

            buttonsDict[$"select-{i}"] = instance_button;
        }
        SetEnabledSelectButtons(false);

        GameScript.OnShuffled += ()=> {
            status_text.text = $"> раздача {gameScript.shuffle_amount} из {gameScript.max_shuffle_amount}.";
            buttonsDict["advance"].SetEnabled(false);
            SetTextSelectButtons();
            SetEnabledSelectButtons(true);

            if (clipProgress)
                audioSource.PlayOneShot(clipProgress);

        }; // Bad? But Ok?

        GameScript.OnSelect += (value) => {
            UpdateStored();
            status_text.text = $"> выбран : [{gameScript.pool[gameScript.index_selection_pool[value]].name}]. ожидание раздачи.";
            buttonsDict["advance"].text = "продолжить";
            buttonsDict["advance"].SetEnabled(true);

            if (clipFinish)
                audioSource.PlayOneShot(clipFinish);

            SetEnabledSelectButtons(false);
        };

        GameScript.OnCountStored += () => {
            status_text.text = $"> конец";
            buttonsDict["advance"].SetEnabled(false);
            SetEnabledSelectButtons(false);
        };

    }
    void AddRows()
    {
        var data_rows = root.Q("block-body-rows");

        // stats
        var data_row_instance = rowAsset.CloneTree();
        data_row_instance.Q<Label>("block-name").text = "_Участники";
        data_rows.Add(data_row_instance);
        for (int i = 0; i < gameScript.stats.Count; i++)
        {
            var data_instance = dataAsset.CloneTree();
            data_instance.SetEnabled(false);

            data_instance.dataSource = gameScript.stats[i];

            data_row_instance.Q("list").Add(data_instance);
            data_instance.SetEnabled(true);

            // sound
            data_instance.RegisterCallback<MouseEnterEvent>((evt) => { 
                if(clipHover)
                    audioSource.PlayOneShot(clipHover); 
            });
        }

        // pool
        data_row_instance = rowAsset.CloneTree();
        data_row_instance.Q<Label>("block-name").text = "_Доступные-предметы";
        data_rows.Add(data_row_instance);
        for (int i = 0; i < gameScript.pool.Count; i++)
        {
            var data_instance = dataAsset.CloneTree();
            data_instance.SetEnabled(false);

            data_instance.Q<Label>("name").text = gameScript.pool[i].name;
            StringBuilder sb = new();
            foreach(var stat in gameScript.pool[i].affectedStats)
            {
                sb.Append($"{stat.stat_name} = {stat.value_affect}//");
            }
            data_instance.Q<Label>("value").text = sb.ToString();

            data_row_instance.Q("list").Add(data_instance);
            data_instance.SetEnabled(true);

            // sound
            data_instance.RegisterCallback<MouseEnterEvent>((evt) => {
                if (clipHover)
                    audioSource.PlayOneShot(clipHover);
            });
        }

        // stored
        data_row_instance = rowAsset.CloneTree();
        data_row_instance.name = "data-row-stored";
        data_row_instance.Q<Label>("block-name").text = $"_Выбранные-предметы (максимум {gameScript.max_shuffle_amount})";
        data_rows.Add(data_row_instance);
    }
}
