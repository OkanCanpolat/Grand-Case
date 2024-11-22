using UnityEngine;
using Zenject;

public class GameInstaller : MonoInstaller
{
    public float ReferenceCameraSize;
    public SwipeConfig SwipeConfig;
    public WinScreenConfig WinScreenConfig;
    public ObjectPoolConfig ObjectPoolConfig;
    public GameViewSoundConfig GameViewSoundConfig;
    public WinScreenSoundConfig WinScreenSoundConfig;
    public LoseScreenSoundConfig LoseScreenSoundConfig;
    public BoardConfig BoardConfig;
    public LoseScrenConfig LoseScrenConfig;
    public SpinConfig SpinConfig;
    public override void InstallBindings()
    {
        SignalBusInstaller.Install(Container);
        Container.DeclareSignal<BoardCreateSignal>().OptionalSubscriber();
        Container.DeclareSignal<SpinReachMaximumSpeedSignal>().OptionalSubscriber();
        Container.DeclareSignal<SpinStopSignal>().OptionalSubscriber();
        Container.DeclareSignal<MatchFoundSignal>().OptionalSubscriber();
        Container.DeclareSignal<SwipeStartSignal>().OptionalSubscriber();
        Container.DeclareSignal<SpinStartSignal>().OptionalSubscriber();
        Container.DeclareSignal<CameraSetupSignal>().OptionalSubscriber();
        Container.DeclareSignal<RestartGameSignal>().OptionalSubscriber();
        Container.DeclareSignal<UnvalidSwipeSignal>().OptionalSubscriber();
        Container.DeclareSignal<LevelFailedSignal>().OptionalSubscriber();


        Container.BindInstance(SpinConfig).AsSingle();
        Container.BindInstance(BoardConfig).AsSingle();
        Container.BindInstance(LoseScrenConfig).AsSingle();
        Container.BindInstance(LoseScreenSoundConfig).AsSingle();
        Container.BindInstance(ObjectPoolConfig).AsSingle();
        Container.BindInstance(GameViewSoundConfig).AsSingle();
        Container.BindInstance(WinScreenSoundConfig).AsSingle();
        Container.BindInstance(SwipeConfig).AsSingle();
        Container.BindInstance(WinScreenConfig).AsSingle();


        Container.Bind<MoveCountController>().FromComponentInHierarchy().AsSingle();
        Container.Bind<IMinMoveCalculator>().To<BFS_MinMoveCalculator>().AsSingle();


        Container.Bind<IMatchFinder>().To<BasicMatchFinder>().AsSingle().WhenInjectedInto(typeof(BasicTile));
        Container.Bind<IAudioService>().To<AudioManager>().FromComponentInHierarchy().AsSingle();
        Container.BindInstance(ReferenceCameraSize).WithId("ReferenceCamSize").AsSingle();

        BoardInstaller.Install(Container);
        SpinControllerInstaller.Install(Container);
        ObjectPoolInstaller.Install(Container);
    }
}

public class SpinControllerInstaller : Installer<SpinControllerInstaller>
{
    public override void InstallBindings()
    {
        Container.Bind<SpinController>().FromComponentInHierarchy().AsSingle();
        Container.Bind<SpinStateMachine>().AsSingle();
        Container.Bind(typeof(ISpinState), typeof(SpinIdleState)).WithId("Idle").To<SpinIdleState>().AsSingle();
        Container.Bind(typeof(ISpinState), typeof(SpinBeginState)).WithId("Begin").To<SpinBeginState>().AsSingle();
        Container.Bind(typeof(ISpinState), typeof(SpinStopState)).WithId("Stop").To<SpinStopState>().AsSingle();
    }
}
public class BoardInstaller : Installer<BoardInstaller>
{
    public override void InstallBindings()
    {
        Container.Bind<Board>().FromComponentInHierarchy().AsSingle();
        Container.Bind<ISwipeDirectionCalculator>().To<TangentDirectionCalculator>().AsSingle();
        Container.Bind<IBoardCreator>().To<MinXRandomBoardCreator>().AsSingle();
        Container.Bind<BoardStateMachine>().AsSingle();
        Container.Bind(typeof(IBoardState), typeof(BoardReadySwipeState)).WithId("ReadySwipe").To<BoardReadySwipeState>().AsSingle();
        Container.Bind(typeof(IBoardState), typeof(BoardSwipingState)).WithId("Swiping").To<BoardSwipingState>().AsSingle();
        Container.Bind(typeof(IBoardState), typeof(BoardLockState)).WithId("Lock").To<BoardLockState>().AsSingle();
    }
}
public class ObjectPoolInstaller : Installer<ObjectPoolInstaller>
{
    public override void InstallBindings()
    {
        Container.Bind<IObjectPool>().To<ObjectPool>().AsSingle().Lazy();
        Container.BindFactory<GameObject, GameObject, Factory>().FromFactory<CustomFactory>();
    }
}
