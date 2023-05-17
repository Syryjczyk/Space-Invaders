using UnityEngine;

[CreateAssetMenu(fileName = "SceneData", menuName = "Scriptable Objects/SceneData")]
public class SceneDataSO : ScriptableObject
{
    public int Level;
    public bool IsGameInitialize;
    public bool NextLevelAchived;
}
