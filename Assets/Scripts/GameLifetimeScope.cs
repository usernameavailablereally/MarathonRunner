using Core.Services;
using Core.Services.Events;
using Game.Configs;
using Game.MonoBehaviourComponents;
using Game.Services;
using UnityEngine;
using UnityEngine.InputSystem;
using VContainer;
using VContainer.Unity;

public class GameLifetimeScope : LifetimeScope
{ 
    [SerializeField] private MatchConfig _matchConfig;
    [SerializeField] private AssetsConfig _assetsConfig;
    [SerializeField] private InputActionAsset _inputActionAsset;
    [SerializeField] private Camera _mainCamera;
    [SerializeField] private LevelManagerComponent _levelManager;
    [SerializeField] private GameOverLayerComponent _gameOverLayer;
        
    protected override void Configure(IContainerBuilder builder)
    {            
        ValidateSerializedFields();
            
        RegisterSceneReferences(builder);
            
        RegisterInterfaces(builder);
            
        RegisterEntryPoints(builder);
    }

    private void ValidateSerializedFields()
    {
        if (_mainCamera == null) throw new MissingReferenceException($"{nameof(_mainCamera)} is not assigned");
        if (_levelManager == null) throw new MissingReferenceException($"{nameof(_levelManager)} is not assigned");
        if (_matchConfig == null) throw new MissingReferenceException($"{nameof(_matchConfig)} is not assigned");
        if (_assetsConfig == null) throw new MissingReferenceException($"{nameof(_assetsConfig)} is not assigned");
        if (_inputActionAsset == null) throw new MissingReferenceException($"{nameof(_inputActionAsset)} is not assigned");
        if (_gameOverLayer == null) throw new MissingReferenceException($"{nameof(_gameOverLayer)} is not assigned");
    }    
        
    private void RegisterSceneReferences(IContainerBuilder builder)
    {
        builder.RegisterInstance(_matchConfig);
        builder.RegisterInstance(_assetsConfig);
        builder.RegisterInstance(_inputActionAsset);
        builder.RegisterComponent(_mainCamera);
        builder.RegisterComponent(_levelManager);
        builder.RegisterComponent(_gameOverLayer);
    } 
    private void RegisterInterfaces(IContainerBuilder builder)
    {
        builder.Register<GameInputService>(Lifetime.Singleton).AsImplementedInterfaces();
        builder.Register<GameMatchService>(Lifetime.Singleton).AsImplementedInterfaces();
        builder.Register<DispatcherService>(Lifetime.Singleton).AsImplementedInterfaces();
    }
    
    private void RegisterEntryPoints(IContainerBuilder builder)
    {
        builder.RegisterEntryPoint<EntryPointService>().AsSelf();
    }
}