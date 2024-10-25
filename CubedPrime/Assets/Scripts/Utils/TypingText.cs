using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

public class TypingText : MonoBehaviour
{
    public TMP_Text textField;
    public float typingSpeed = 0.1f;
    public float typingDelay = 0.2f;
    public float transitionDelay = 2f;
    public bool autoStart = true;
    
    public string prefix; 

    private string _initText;
    private bool _isTyping = false; 
    private Stack<string> backLogBuffer = new Stack<string>();
    
    // Start is called before the first frame update
    void Start()
    {
        if(textField.text.Length != 0 && autoStart) startType(textField.text);
    }
    
    public void startType(string newText) {
        if(_isTyping) {
            backLogBuffer.Push(newText);
            return;
        }
        if(prefix.Length != 0) _initText = prefix + newText;
        else  _initText = newText;
        textField.text = "";
        StartCoroutine(Type());
    }

    IEnumerator Type()
    {   
        _isTyping = true;
        yield return new WaitForSeconds(typingDelay);
        foreach (char c in _initText)
        {
            textField.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }
        yield return new WaitForSeconds(transitionDelay);
        if (autoStart) StartCoroutine(BackSpace());
    }
    IEnumerator BackSpace() {
        StringBuilder sb = new StringBuilder(textField.text);
        yield return new WaitForSeconds(typingDelay);
        foreach (char c in _initText)
        {
            sb.Length--;
            textField.text = sb.ToString();
            yield return new WaitForSeconds(typingSpeed);
        }
        _isTyping = false;
        if(backLogBuffer.Count != 0) {
            startType(backLogBuffer.Pop());
        }
    }

    public void RestartType(string newText)
    {
        StopAllCoroutines();

        _isTyping = false;
        if(prefix.Length != 0) _initText = prefix + newText;
        else  _initText = newText;
        textField.text = "";
        
        StartCoroutine(Type());
    }
}