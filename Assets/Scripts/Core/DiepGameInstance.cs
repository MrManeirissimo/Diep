using static DiepPlugin.GameNetworkTag;
using static NetworkFacade;

using DarkRift.Client;
using DarkRift;

using UnityEngine;


public class DiepGameInstance : GameInstance {
    public static DiepGameInstance Create() {
        if (instance == null) {
            Instance = new DiepGameInstance();
            return (DiepGameInstance)Instance;
        }

        return null;
    }

    public DiepNetworkManager NetManager { get; private set; }
    public DiepPlayerManager PlayerManager { get; private set; }

    private DiepGameInstance() {
        NetManager = DiepNetworkManager.Instance;
        NetManager.ClientBindToMessageReceived(OnCliedReceivedMessage);
        GameObject.DontDestroyOnLoad(NetManager);

        PlayerManager = DiepPlayerManager.Instance;
        GameObject.DontDestroyOnLoad(PlayerManager);
    }

    private void OnCliedReceivedMessage(object sender, MessageReceivedEventArgs eArgs) {
        using (Message message = eArgs.GetMessage()) {
            if (message.Tag == SPAWN_PLAYER) {
                using (DarkRiftReader reader = message.GetReader()) {
                    while (reader.Position < reader.Length) {
                        // messaging unpacking
                        ushort id = reader.ReadUInt16();
                        Vector3 position = new Vector3(reader.ReadSingle(), reader.ReadSingle());
                        float radius = reader.ReadSingle();
                        Color32 color = new Color32(reader.ReadByte(), reader.ReadByte(), reader.ReadByte(), 255);

                        PlayerManager.SpawnPlayer(id, position, radius, color, id == GetClientID());
                    }
                }
            }

            else if (message.Tag == DESPAWN_PLAYER) {
                using (DarkRiftReader reader = message.GetReader())
                    PlayerManager.DespawnPlayer(reader.ReadUInt16());
            }
        }
    }
}