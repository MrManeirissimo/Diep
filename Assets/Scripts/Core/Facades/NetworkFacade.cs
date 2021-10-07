using static RuntimeFacade;

using DarkRift.Client;
using DarkRift;

using UnityEngine.Events;
using UnityEngine;

public static class NetworkFacade {
    public static DiepNetworkManager GetNetManager() => GetGameInstance<DiepGameInstance>().NetManager;

    public static ushort GetClientID() => GetNetManager().Client.ID;

    public static void Disconnect() => GetNetManager().Disconnect();
    public static void Connect() => GetNetManager().Connect();

    public static void ClientReadMessageWithTag(ushort tag, MessageReceivedEventArgs eventArgs, DiepNetworkManager.MessageRead readDelegate)
        => GetNetManager().ClientReadMessageWithTag(tag, eventArgs, readDelegate);

    public static void SendClientMessage(DiepNetworkManager.MessageWrite messageWrite, ushort messageTag, SendMode sendMode)
        => GetNetManager().ClientSendMessage(messageWrite, messageTag, sendMode);

    public static void ClientBindToMessageReceived(UnityAction<object, MessageReceivedEventArgs> onReceive)
        => GetNetManager().ClientBindToMessageReceived(onReceive);

    public static void ClientUnbindFromMessageReceived(UnityAction<object, MessageReceivedEventArgs> onReceive)
        => GetNetManager().ClientUnbindFromMessageReceived(onReceive);    
}