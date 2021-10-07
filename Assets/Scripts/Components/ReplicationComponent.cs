using static DiepPlugin.GameNetworkTag;
using static NetworkFacade;

using DarkRift.Client;
using UnityEngine;
using DarkRift;

public class ReplicationComponent : MonoBehaviour {
    bool IsLocalyControlled => character.GetController() != null;

    [Tooltip("The distance we can move before we send a position update.")]
    [SerializeField] float displacementDelta = 0.05f;
    [SerializeField] float interpolationAmount = 5;
    PlayerCharacter character;

    Vector3 interpolateTowards;
    Vector3 previousPosition;

    void Awake() {
        ClientBindToMessageReceived(OnClientMessageReceived);

        character = GetComponent<PlayerCharacter>();
        interpolateTowards = previousPosition = transform.position;
    }

    void OnClientMessageReceived(object sender, MessageReceivedEventArgs eventArgs) {
        ClientReadMessageWithTag(MOVEMENT, eventArgs,
            (reader) => {
                ushort id = reader.ReadUInt16();
                Vector3 position = new Vector3(reader.ReadSingle(), reader.ReadSingle(), 0);

                if (GetClientID() != id) {
                    interpolateTowards = position;
                }
            });
    }

    void Update() {
        if (IsLocalyControlled) {
            if (DisplacementExceededDelta()) {
                SendClientMessage((writer) => {
                    writer.Write(transform.position.x);
                    writer.Write(transform.position.y);
                }, MOVEMENT, DarkRift.SendMode.Unreliable);

                previousPosition = transform.position;
            }
        }
        else {
            transform.position = Vector3.MoveTowards(transform.position, interpolateTowards, interpolationAmount * Time.deltaTime);
        }
    }

    bool DisplacementExceededDelta() {
        return Vector3.Distance(previousPosition, transform.position) > displacementDelta;
    }
}