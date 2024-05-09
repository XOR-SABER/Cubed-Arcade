using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VideoIntroScript : MonoBehaviour
{
    public TypingText bottomTextField;
    public TypingText topTextField;

    void Start() {
        topTextField.startType("Cubed prime");
        bottomTextField.startType("Coming to Steam when we feel like it!");
    }
}
