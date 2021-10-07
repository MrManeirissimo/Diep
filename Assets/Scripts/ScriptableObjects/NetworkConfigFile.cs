using UnityEngine;

[CreateAssetMenu(fileName = "NetConfig", menuName = "Network/NetConfigFile")]
public class NetworkConfigFile : ScriptableObject {
    public event System.Action OnConfigChanged;
    public string Host {
        get => host; 
        set {
            host = value;
            OnConfigChanged?.Invoke();
        }
    }
    public ushort Port {
        get => port;
        set {
            port = value;
            OnConfigChanged?.Invoke();
        }
    }

    [SerializeField] private string host;
    [SerializeField] private ushort port;
}