using Core.Controllers;
using Core.Services;
using Core.Services.Events;
using Core.Services.Loaders.Configs;
using Core.Services.Match;
using UnityEngine;
using VContainer;
using VContainer.Unity;

public class GameLifetimeScope : LifetimeScope
{
    [SerializeField] private Camera _mainCamera;
        
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
    }    
        
    private void RegisterMonoComponents(IContainerBuilder builder)
    {
        builder.RegisterComponent(_mainCamera);
    } 
    private void RegisterInterfaces(IContainerBuilder builder)
    {
        builder.Register<GameInputController>(Lifetime.Singleton).As<ITickable>();
        builder.Register<AssetsHelper>(Lifetime.Singleton).AsImplementedInterfaces();
        builder.Register<MatchService>(Lifetime.Singleton).AsImplementedInterfaces();
        builder.Register<DispatcherService>(Lifetime.Singleton).AsImplementedInterfaces();
    }
    
    private void RegisterEntryPoints(IContainerBuilder builder)
    {
        builder.RegisterEntryPoint<EntryPointService>().AsSelf();
    }
}