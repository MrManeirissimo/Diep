using static NetworkFacade;
using static DiepPlugin.GameNetworkTag;
using UnityEngine;

public class CharacterReplicationComponent : MonoBehaviour {
    [SerializeField]
    [Tooltip("The distance we can move before we send a position update.")]
    PlayerController playerController;

    [SerializeField] float displacementDelta = 0.05f;
    Vector3 lastPosition;

    void Awake() {
        playerController = GetComponent<PlayerController>();

        if (GetControlledCharacterTransform(out Transform t))
            lastPosition = t.position;
    }

    void Update() {
        if (GetControlledCharacterTransform(out Transform t)) {
            if (DisplacementExceededDelta(t.position)) {
                SendClientMessage((writer) => {
                    writer.Write(t.position.x);
                    writer.Write(t.position.y);
                }, MOVEMENT, DarkRift.SendMode.Unreliable);

                lastPosition = t.position;
            }
        }
    }

    bool GetControlledCharacterTransform(out Transform transform) {
        try {
            transform = playerController.GetPlayerCharacer().GetGameObject().transform;
            return true;
        }
        catch(System.NullReferenceException) {
            transform = null;
            return false;
        }
    }
    bool DisplacementExceededDelta(Vector3 position) {
        return Vector3.Distance(lastPosition, position) > displacementDelta;
    }
}