using System.Collections.Generic;
using UnityEngine;

public class DiepPlayerManager : SingletonBehavior<DiepPlayerManager> {
    public event System.Action<DiepPlayerController> OnLocalPlayerReceived;
    public event System.Action<DiepCharacter> OnPlayerReceived;

    public Dictionary<ushort, DiepCharacter> PlayerRegistry { get => playersRegistry; }
    public DiepPlayerController LocalPlayerController { get => localPlayer; }

    [SerializeField] GameConfigFile gameConfig;
    [SerializeField] DiepPlayerController localPlayer;
    Dictionary<ushort, DiepCharacter> playersRegistry;

    void Awake() {
        gameConfig = Resources.Load<GameConfigFile>("Config/GameConfig");
        playersRegistry = new Dictionary<ushort, DiepCharacter>();
    }

    public DiepCharacter SpawnPlayer(ushort id, Vector3 position, float radius, Color32 color, bool controllable) {
        GameObject playerObject = null;
        playerObject = Instantiate(gameConfig.character, position, Quaternion.identity);

        DiepCharacter character = playerObject.GetComponent<DiepCharacter>();
        character.SetRadius(radius);
        character.SetColor(color);
        character.PlayerID = id;
        playersRegistry.Add(id, character);

        if (controllable) {
            localPlayer = Instantiate(gameConfig.controller).GetComponent<DiepPlayerController>();
            OnLocalPlayerReceived?.Invoke(localPlayer);

            localPlayer.TakeControl(character);
            localPlayer.PlayerID = id;
        }

        OnPlayerReceived?.Invoke(character);

        return character;
    }

    public void DespawnPlayer(ushort id) {
        Destroy(playersRegistry[id].gameObject);
        playersRegistry.Remove(id);
    }

    public DiepCharacter GetPlayer(ushort playerID) => playersRegistry[playerID];
}