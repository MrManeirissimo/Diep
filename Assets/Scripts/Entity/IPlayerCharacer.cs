
public interface IPlayerCharacer : IGameEntity {
    event System.Action<IPlayerController> OnControllerReceived;
    void SetController(IPlayerController controller);
    IPlayerController GetController();
}