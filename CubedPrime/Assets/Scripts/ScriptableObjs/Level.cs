using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New level", menuName = "level")]
public class Level : ScriptableObject
{
    public string sceneName;
    public string levelName;
    public string levelDescription;
    public string gameMode;
    public string levelTheme;
    public string levelHighScoreTag;
}
