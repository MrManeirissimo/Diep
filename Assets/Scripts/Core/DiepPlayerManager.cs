using System.Collections.Generic;
using UnityEngine;

public class DiepPlayerManager : SingletonBehavior<DiepPlayerManager> {
    public Dictionary<ushort, DiepCharacter> PlayerRegistry { get => playersRegistry; }

    [SerializeField] GameConfigFile gameConfig;
    [SerializeField] PlayerController localPlayer;
    Dictionary<ushort, DiepCharacter> playersRegistry;

    void Awake() {
        gameConfig = Resources.Load<GameConfigFile>("Config/GameConfig");
        playersRegistry = new Dictionary<ushort, DiepCharacter>();
    }

    public void SpawnPlayer(ushort id, Vector3 position, float radius, Color32 color, bool controllable) {
        GameObject playerObject = null;
        playerObject = Instantiate(gameConfig.character, position, Quaternion.identity);

        DiepCharacter character = playerObject.GetComponent<DiepCharacter>();
        character.SetRadius(radius);
        character.SetColor(color);
        playersRegistry.Add(id, character);

        if (controllable) {
            DiepPlayerController controller = Instantiate(gameConfig.controller).GetComponent<DiepPlayerController>();
            controller.TakeControl(character);
        }
    }

    public void DespawnPlayer(ushort id) {
        Destroy(playersRegistry[id].gameObject);
        playersRegistry.Remove(id);
    }
}