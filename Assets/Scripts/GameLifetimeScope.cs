using Core.Services;
using Core.Services.Events;
using Core.Utils;
using Game.MonoBehaviourComponents;
using Game.Services;
using UnityEngine;
using VContainer;
using VContainer.Unity;

public class GameLifetimeScope : LifetimeScope
{
    [SerializeField] private Camera _mainCamera;
    [SerializeField] private LevelManagerComponent _levelManager;
        
    protected override void Configure(IContainerBuilder builder)
    {            
        ValidateSerializedFields();
            
        RegisterMonoComponents(builder);
            
        RegisterInterfaces(builder);
            
        RegisterEntryPoints(builder);
    }

    private void ValidateSerializedFields()
    {
        if (_mainCamera == null) throw new MissingReferenceException($"{nameof(_mainCamera)} is not assigned");
        if (_levelManager == null) throw new MissingReferenceException($"{nameof(_levelManager)} is not assigned");
    }    
        
    private void RegisterMonoComponents(IContainerBuilder builder)
    {
        builder.RegisterComponent(_mainCamera);
        builder.RegisterComponent(_levelManager);
    } 
    private void RegisterInterfaces(IContainerBuilder builder)
    {
        builder.Register<GameInputService>(Lifetime.Singleton).As<ITickable>();
        builder.Register<AssetsHelper>(Lifetime.Singleton).AsImplementedInterfaces();
        builder.Register<GameMatchService>(Lifetime.Singleton).AsImplementedInterfaces();
        builder.Register<DispatcherService>(Lifetime.Singleton).AsImplementedInterfaces();
    }
    
    private void RegisterEntryPoints(IContainerBuilder builder)
    {
        builder.RegisterEntryPoint<EntryPointService>().AsSelf();
    }
}