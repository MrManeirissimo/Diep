using System.Collections.Generic;
using System.Collections;

using DarkRift.Client.Unity;
using DarkRift;

using UnityEngine;
using DiepPlugin;
using System.Net;
using System.Net.Sockets;

public class PlayerSpawner : MonoBehaviour {
    [SerializeField]
    [Tooltip("The DarkRift client to communicate on.")]
    UnityClient client;

    [SerializeField]
    [Tooltip("The controllable player prefab.")]
    GameObject controllablePrefab;

    [SerializeField]
    [Tooltip("The network controllable player prefab.")]
    GameObject networkPrefab;

    private void Awake() {
        if (client == null) {
            Debug.LogError("Client unassigned in PlayerSpawner.");
            Application.Quit();
        }

        if (controllablePrefab == null) {
            Debug.LogError("Controllable Prefab unassigned in PlayerSpawner.");
            Application.Quit();
        }

        if (networkPrefab == null) {
            Debug.LogError("Network Prefab unassigned in PlayerSpawner.");
            Application.Quit();
        }

        client.MessageReceived += SpawnPlayer;
    }

    private void SpawnPlayer(object sender, DarkRift.Client.MessageReceivedEventArgs e) {
        using (Message message = e.GetMessage())
        using (DarkRiftReader reader = message.GetReader()) {
            if (message.Tag == InGameTags.SpawnPlayer) {
                if (reader.Length % 17 != 0) {
                    Debug.LogWarning("Received malformed spawn packet.");
                    return;
                }

                while(reader.Position < reader.Length) {
                    ushort id = reader.ReadUInt16();
                    Vector3 position = new Vector3(reader.ReadSingle(), reader.ReadSingle());
                    float radius = reader.ReadSingle();
                    Color32 color = new Color32(reader.ReadByte(), reader.ReadByte(), reader.ReadByte(), 255);

                    GameObject obj = Instantiate(id == client.ID ? controllablePrefab : networkPrefab, position, Quaternion.identity) as GameObject;

                    Player playerObj = obj.GetComponent<Player>();
                    playerObj.SetRadius(radius);
                    playerObj.SetColor(color);
                }
            }
        }
    }
}
