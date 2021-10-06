using UnityEngine;
using System;

public class PlayerController : MonoBehaviour, IPlayerController {
    public event Action<IPlayerCharacer> OnControlTaken;

    public IPlayerCharacer GetPlayerCharacer() => character;
    public GameObject GetGameObject() => gameObject;

    public void TakeControl(IPlayerCharacer character) {
        this.character = (PlayerCharacter)character;

        if (this.character) {
            OnControlTaken?.Invoke(character);
            this.character.SetController(this);
        }
    }

    [Tooltip("The controlled character reference for this controller")]
    [SerializeField] protected PlayerCharacter character;
}