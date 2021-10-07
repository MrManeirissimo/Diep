using System.Collections.Generic;
using System.Collections;

using DarkRift.Client.Unity;
using DarkRift;

using UnityEngine;
using DiepPlugin;
using System.Net;
using System.Net.Sockets;
using DarkRift.Client;
using System;

public class PlayerSpawner : MonoBehaviour {
    [SerializeField]
    [Tooltip("The DarkRift client to communicate on.")]
    UnityClient client;

    [SerializeField]
    [Tooltip("The network player manager.")]
    PlayerNetworkManager networkPlayerManager;

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

        client.MessageReceived += OnMessageReceived;
    }

    private void OnMessageReceived(object sender, DarkRift.Client.MessageReceivedEventArgs e) {
        using (Message message = e.GetMessage()) {
            if(message.Tag == GameNetworkTag.SPAWN_PLAYER) {
                SpawnPlayer(message);
            }
            else if(message.Tag == GameNetworkTag.DESPAWN_PLAYER) {
                DespawnPlayer(message);
            }
        }
    }


    private void SpawnPlayer(Message message) {
        using (DarkRiftReader reader = message.GetReader()) {
            if (reader.Length % 17 != 0) {
                Debug.LogWarning("Received malformed spawn packet.");
                return;
            }

            while (reader.Position < reader.Length) {
                ushort id = reader.ReadUInt16();
                Vector3 position = new Vector3(reader.ReadSingle(), reader.ReadSingle());
                float radius = reader.ReadSingle();
                Color32 color = new Color32(reader.ReadByte(), reader.ReadByte(), reader.ReadByte(), 255);

                GameObject obj;
                if (id == client.ID) {
                    obj = Instantiate(controllablePrefab, position, Quaternion.identity) as GameObject;


                    MovementReplicationStuff replication;
                    if (replication = obj.GetComponent<MovementReplicationStuff>()) {
                        replication.Client = client;
                    }
                }
                else {
                    obj = Instantiate(networkPrefab, position, Quaternion.identity) as GameObject;
                }

                Player player = obj.GetComponent<Player>();
                player.SetRadius(radius);
                player.SetColor(color);

                networkPlayerManager.Add(id, player);
            }
        }
    }

    private void DespawnPlayer(Message message) {
        using (DarkRiftReader reader = message.GetReader())
            networkPlayerManager.DestroyPlayer(reader.ReadUInt16());
    }
}
