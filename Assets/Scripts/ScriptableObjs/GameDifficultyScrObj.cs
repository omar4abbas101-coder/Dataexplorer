using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New difficulty", menuName = "Difficulty")]
public class GameDifficultyScrObj : ScriptableObject
{
    public Color color;

    public List<WaveScrObj> waves;
}
