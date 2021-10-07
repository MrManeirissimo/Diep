using System.Collections.Generic;

using DarkRift.Client.Unity;
using DarkRift;

using UnityEngine;
using DiepPlugin;
using System;

public class PlayerNetworkManager : MonoBehaviour {
    [SerializeField]
    [Tooltip("The DarkRift client to communicate on.")]
    UnityClient client;

    Dictionary<ushort, Player> networkPlayers = new Dictionary<ushort, Player>();

    public void Add(ushort id, Player player) {
        networkPlayers.Add(id, player);
    }

    private void Awake() {
        client.MessageReceived += OnMessageReceived;
    }

    private void OnMessageReceived(object sender, DarkRift.Client.MessageReceivedEventArgs e) {
        using (Message message = e.GetMessage()) {
            if(message.Tag == GameNetworkTag.MOVEMENT) {
                using(DarkRiftReader reader = message.GetReader()) {
                    ushort id = reader.ReadUInt16();
                    Vector3 position = new Vector3(reader.ReadSingle(), reader.ReadSingle(), 0);

                    if (networkPlayers.ContainsKey(id)) {
                        networkPlayers[id].SetMovePosition(position);
                    }
                }
            }
        }
    }

    internal void DestroyPlayer(ushort id) {
        Player player = networkPlayers[id];

        Destroy(player.gameObject);

        networkPlayers.Remove(id);
    }
}