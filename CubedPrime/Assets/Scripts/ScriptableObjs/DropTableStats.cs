using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Droptable", menuName = "Droptable")]
public class DropTableStats : ScriptableObject {
    [Tooltip("All Objects in the a Spawntable / Droptable")]
    public Spawnable [] objs;
}
