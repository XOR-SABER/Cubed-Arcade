using UnityEngine;

[CreateAssetMenu(fileName = "New Sound Collection", menuName = "Sound Collection")]
public class SoundCollection : ScriptableObject{
    public SoundObject[] sounds; 
}