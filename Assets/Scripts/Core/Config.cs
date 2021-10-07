using UnityEngine;

/// <summary>
/// Used to configure the game instance reference
/// </summary>
/// 
using static RuntimeFacade;

class Config {
    static bool debug = true;
    static void ExecIfDebug(System.Action action) {
        if(debug) action();
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void ConfigGame() {
        DiepGameInstance.Create();

        Debug.Log($"Initialized GameInstance:: {(GetGameInstance() as DiepGameInstance).GetType().Name}");
        //ExecIfDebug(() => {
            
        //});
    }
}