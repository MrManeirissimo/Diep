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

    public DiepPlayerManager PlayerManager { get; private set; }
    public DiepNetworkManager NetManager { get; private set; }
    public DiepEventManager EventManager { get; private set; }

    private DiepGameInstance() {
        EventManager = DiepEventManager.Instance;

        NetManager = DiepNetworkManager.Instance;
        NetManager.ClientBindToMessageReceived(OnCliedReceivedMessage);
        GameObject.DontDestroyOnLoad(NetManager);

        PlayerManager = DiepPlayerManager.Instance;
        GameObject.DontDestroyOnLoad(PlayerManager);

        PlayerManager.OnPlayerReceived += delegate (DiepCharacter character) {
            EventManager.Dispatch("OnPlayerReceived", PlayerManager, () => {
                DiepEventArgs args = new DiepEventArgs();
                args.AddProp("player", character);
                return args;
            });
        };

        PlayerManager.OnLocalPlayerReceived += delegate (DiepPlayerController playerController) {
            EventManager.Dispatch("OnLocalPlayerReceived", PlayerManager, () => {
                DiepEventArgs args = new DiepEventArgs();
                args.AddProp("controller", playerController);
                return args;
            });
        };
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

            else if (message.Tag == SPAWN_BULLET) {
                ClientReadMessage(message,
                    (reader) => {
                        ushort ownerID = reader.ReadUInt16();

                        Vector3 position = new Vector3 {
                            x = reader.ReadSingle(),
                            y = reader.ReadSingle(),
                            z = 0
                        };

                        Vector3 velocity = new Vector3 {
                            x = reader.ReadSingle(),
                            y = reader.ReadSingle(),
                            z = 0
                        };

                        Color32 color = new Color32 {
                            r = reader.ReadByte(),
                            g = reader.ReadByte(),
                            b = reader.ReadByte(),
                            a = 255
                        };

                        GameObject.FindObjectOfType<WeaponComponent>().FireRemote(ownerID, color, position, velocity);
                    }
                );
            }
        }
    }
}