using DarkRift.Client.Unity;
using DarkRift.Client;
using DarkRift;

using UnityEngine.Events;
using UnityEngine;

[RequireComponent(typeof(DiepNetworkClient))]
public class DiepNetworkManager : SingletonBehavior<DiepNetworkManager> {
    public delegate void MessageWrite(DarkRiftWriter writer);
    public delegate void MessageRead(DarkRiftReader reader);
    public DiepNetworkClient Client { get; set; }
    UnityEvent<object, MessageReceivedEventArgs> messageReceivedObserver = new UnityEvent<object, MessageReceivedEventArgs>();

    private void Awake() {
        Client = GetComponent<DiepNetworkClient>();
        Client.MessageReceived += OnClientMessageReceived;
    }

    public void Disconnect() => Client.Disconnect();
    public void Connect() => Client.Connect();

    private void OnClientMessageReceived(object sender, MessageReceivedEventArgs e) {
        messageReceivedObserver.Invoke(sender, e);
    }
    public void ClientBindToMessageReceived(UnityAction<object, MessageReceivedEventArgs> onReceive) {
        messageReceivedObserver.AddListener(onReceive);
    }
    public void ClientUnbindFromMessageReceived(UnityAction<object, MessageReceivedEventArgs> onReceive) {
        messageReceivedObserver.RemoveListener(onReceive);
    }
    public void ClientSendMessage(MessageWrite lambda, ushort messageTag, SendMode sendMode = SendMode.Unreliable) {
        using(DarkRiftWriter writer = DarkRiftWriter.Create()) {
            lambda(writer);

            using(Message message = Message.Create(messageTag, writer)) {
                Client.SendMessage(message, sendMode);
            }
        }
    }
    public void ClientReadMessageWithTag(ushort tag, MessageReceivedEventArgs eventArgs, MessageRead readDelegate) {
        using(Message message = eventArgs.GetMessage()) {
            if(message.Tag == tag) {
                using(DarkRiftReader reader = message.GetReader()) {
                    readDelegate(reader);
                }
            }
        }
    }
    public void ClientReadMessage(Message message, MessageRead readDelegate) {
        using (DarkRiftReader reader = message.GetReader()) {
            readDelegate(reader);
        }
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.C)) {
            Connect();
        }

        else if (Input.GetKeyDown(KeyCode.P)) {
            Disconnect();
        }
    }
}