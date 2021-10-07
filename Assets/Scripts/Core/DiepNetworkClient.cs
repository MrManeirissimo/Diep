using DarkRift.Client.Unity;
using UnityEngine;

public class DiepNetworkClient : UnityClient {
    public NetworkConfigFile NetConfig => netConfig;

    [SerializeField] NetworkConfigFile netConfig;
    protected override void Awake() {
        base.Awake();

        netConfig = Resources.Load<NetworkConfigFile>("Config/NetConfig");

        host = netConfig.Host;
        port = netConfig.Port;
        connectOnStart = false;
    }

    public void Connect() => Connect(host, port, noDelay);
}