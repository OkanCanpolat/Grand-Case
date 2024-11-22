using UnityEngine;
using Zenject;

public class TileInstaller : MonoInstaller
{
    [SerializeField] private TileBase tile;
    public override void InstallBindings()
    {
        Container.Bind<TileBase>().FromInstance(tile).AsSingle();
        Container.Bind<IMatchable>().To<BasicTileMatchable>().AsSingle().WhenInjectedInto<BasicTile>();
    }
}