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

    Vector3 interpolationPosition;
    Vector3 previousPosition;
    Vector3 interpolationRotation;
    Vector3 previousRotation;

    void Awake() {
        ClientBindToMessageReceived(OnClientMessageReceived);

        character = GetComponent<PlayerCharacter>();
        interpolationRotation = interpolationPosition = interpolationRotation = previousPosition = transform.position;
    }

    void OnClientMessageReceived(object sender, MessageReceivedEventArgs eventArgs) {
        ClientReadMessageWithTag(MOVEMENT, eventArgs,
            (reader) => {
                ushort id = reader.ReadUInt16();
                Vector3 position = new Vector3(reader.ReadSingle(), reader.ReadSingle(), 0);
                Vector3 rotation = new Vector3(0, 0, reader.ReadSingle());

                if (GetClientID() != id) {
                    interpolationPosition = position;
                    interpolationRotation = rotation;
                }
            });
    }

    void Update() {
        if (IsLocalyControlled) {
            if (DisplacementExceededDelta() || RotationExceededDelta()) {
                SendClientMessage((writer) => {
                    writer.Write(transform.position.x);
                    writer.Write(transform.position.y);
                    writer.Write(transform.rotation.eulerAngles.z);
                }, MOVEMENT, DarkRift.SendMode.Unreliable);

                previousPosition = transform.position;
                previousRotation = transform.rotation.eulerAngles;
            }
        }
        else {
            transform.position = Vector3.MoveTowards(transform.position, interpolationPosition, interpolationAmount * Time.deltaTime);
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(interpolationRotation), interpolationAmount * Time.deltaTime);
        }
    }

    bool DisplacementExceededDelta() {
        return Vector3.Distance(previousPosition, transform.position) > displacementDelta;
    }

    bool RotationExceededDelta() {
        
        return Mathf.Abs(transform.rotation.eulerAngles.z - previousRotation.z) > displacementDelta;
    }
}