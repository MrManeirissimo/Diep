using static RuntimeFacade;
using UnityEngine;

public static class PlayerManagerFacade {
    public static DiepPlayerManager GetPlayerManager() => GetGameInstance<DiepGameInstance>().PlayerManager;

    public static PlayerController GetLocalPlayerController => GetPlayerManager().LocalPlayerController;
}