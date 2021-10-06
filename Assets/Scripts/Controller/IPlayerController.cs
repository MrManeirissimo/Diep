
public interface IPlayerController : IGameEntity {
    event System.Action<IPlayerCharacer> OnControlTaken;
    void TakeControl(IPlayerCharacer character);
    IPlayerCharacer GetPlayerCharacer();
}