using System;
using UnityEngine;

public class PlayerCharacter : MonoBehaviour, IPlayerCharacer {
    public event Action<IPlayerController> OnControllerReceived;
    public IPlayerController GetController() => controller;
    public GameObject GetGameObject() => gameObject;


    [Tooltip("The controller entity reference for this character")]
    [SerializeField] PlayerController controller;

    public void SetController(IPlayerController controller) {
        this.controller = (PlayerController)controller;
        if (this.controller) {
            OnControllerReceived?.Invoke(this.controller);
        }
    }
}