using UnityEngine;
using static NetworkFacade;

public class GameEndingManager : SingletonBehavior<GameEndingManager> {
    private void Awake() {
        DiepEventManager.Instance.ListenToEvent("OnLocalPlayerDeath", (sender, eArgs) => {
            Destroy(eArgs.Get<DiepCharacter>("Player").gameObject);
            Disconnect();
            Application.Quit();
        });
    }
}