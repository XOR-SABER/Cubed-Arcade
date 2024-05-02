using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
using Utils;
public class ChangeLog : MonoBehaviour
{
    public TypingText titleText;
    public TypingText changeLogText;

    public Button previewsButton;
    public Button nextButton;

    public string fileName;
    public string folderName;
    private AudioManager _audioMan;

    public List<string> titleIdentifier = new List<string>();
    public List<string> pageIdentifier = new List<string>();

    private List<(string, string)> _page;
    private int _index = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        _audioMan = AudioManager.instance;
        Parse();
        LoadPage();
    }

    public void LoadNextPage()
    {
        _audioMan.Play("ButtonConfirm");
        _index++;
        LoadPage();
    }

    public void LoadPreviewsPage()
    {
        _audioMan.Play("ButtonDeConfirm");
        _index--;
        LoadPage();
    }

    private void LoadPage()
    {
        previewsButton.interactable = _index != 0;
        nextButton.interactable = _index != _page.Count - 1;
        
        titleText.RestartType(_page[_index].Item1);
        changeLogText.RestartType(_page[_index].Item2);
    }

    private void Parse()
    {
        ParseText parse = new ParseText(fileName, folderName);
        string text = parse.GetText();
        GetPage(text);
    }

    private void GetPage(string text)
    {
        _page = new List<(string, string)>();
        string[] lines = text.Split("\n");
        string titleField;
        string textField;
        
        int index = 0;
        while (index < lines.Length)
        {
            textField = "";
            titleField = "";
            
            while (index < lines.Length && !ContainIdentifier(lines[index].ToLower(), pageIdentifier))
            {
                if (ContainIdentifier(lines[index].ToLower(), titleIdentifier))
                {
                    titleField = lines[index].TrimStart().TrimEnd().Replace("\n", String.Empty);
                    titleField = RemoveIndentifierTitle(titleField);
                }
                else
                {
                    textField += lines[index];
                }

                index++;
            }

            textField = textField.Replace("\r", "\n");
            _page.Insert(_page.Count, (titleField, textField));
            index++;
        }
    }

    private string RemoveIndentifierTitle(String title)
    {
        foreach (string i in titleIdentifier)
        {
            if (title.ToLower().Contains(title.ToLower()))
            {
                return title.Remove(0, i.Length).TrimStart();
            }
        }

        return title;
    }

    private bool ContainIdentifier(string line, List<string> indentifier)
    {
        foreach (string i in indentifier)
        {
            if (line.Contains(i.ToLower()))
            {
                return true;
            }
        }

        return false;
    }
}
