using UnityEngine;

public static class RuntimeFacade {
    public static GameInstance GetGameInstance() => GameInstance.Instance;
    public static T GetGameInstance<T>() where T : GameInstance => (T)GameInstance.Instance;
}