using DarkRift.Client.Unity;
using DarkRift.Client;
using DarkRift;

using UnityEngine;
using DiepPlugin;
//using static AAA;


//public delegate void WriteMessage(DarkRiftWriter writer);

//public static class AAA {
//    public static UnityClient Client { get; set; }

//    public static void SendNetworkMessage(WriteMessage lambda, ushort messageTag, SendMode sendMode = SendMode.Unreliable) {
//        using (DarkRiftWriter writer = DarkRiftWriter.Create()) {
//            lambda(writer);

//            using (Message message = Message.Create(messageTag, writer)) {
//                Client.SendMessage(message, sendMode);
//            }
//        }
//    }
//}


public class MovementReplicationStuff : MonoBehaviour {
    const byte MOVEMENT_TAG = 1;
    public UnityClient Client { get; set; }

    [SerializeField]
    [Tooltip("The distance we can move before we send a position update.")]
    float moveDistance = 0.05f;

    Vector3 lastPosition;
    void Awake() {
        lastPosition = transform.position;
    }

    void Update() {
        //if (true) {
        //    SendNetworkMessage((w) => {
        //        w.Write(transform.position.x);
        //        w.Write(transform.position.y);
        //    },MOVEMENT_TAG, SendMode.Unreliable);
        //}

        if (Vector3.Distance(lastPosition, transform.position) > moveDistance) {
            using (DarkRiftWriter writer = DarkRiftWriter.Create()) {
                writer.Write(transform.position.x);
                writer.Write(transform.position.y);

                using (Message message = Message.Create(GameNetworkTag.MOVEMENT, writer)) {
                    Client.SendMessage(message, SendMode.Unreliable);
                }
            }

            lastPosition = transform.position;
        }
    }
}