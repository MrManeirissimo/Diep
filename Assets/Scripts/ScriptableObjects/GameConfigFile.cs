using UnityEngine;

[CreateAssetMenu(menuName = "Config/GameConfigFile")]
public class GameConfigFile : ScriptableObject {
    public GameObject character;
    public GameObject controller;
}