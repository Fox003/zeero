using Unity.Entities;
using UnityEngine;
using UnityEngine.UIElements;

public class MainMenuScreen : UIScreen
{
    private Button _changeSceneButton;
    private Button _switchButton;
    private VisualElement _menuContainer;
    private VisualElement _settingsContainer;
    
    private MainMenuInterfaceState _currentInterfaceState;

    public enum MainMenuInterfaceState
    {
        MainMenu,
        Settings,
    }
    
    public struct ChangeSceneClickedEvent : IComponentData { }
    
    public static MainMenuScreen Instantiate(VisualElement ParentElement)
    {
        var instance = ScriptableObject.CreateInstance<MainMenuScreen>();
        instance.RootElement =  ParentElement;
        
        instance._menuContainer = instance.RootElement.Q<VisualElement>("menu_container");
        instance._settingsContainer = instance.RootElement.Q<VisualElement>("settings_container");
        
        instance._changeSceneButton = instance._menuContainer.Q<Button>("change_scene_button");
        instance._switchButton = instance._menuContainer.Q<Button>("switch_button");

        instance._changeSceneButton.clicked += instance.OnChangeSceneButtonClicked;
        instance._switchButton.clicked += instance.OnSwitchButtonClicked;
        
        return instance;
    }
    
    public override void Show()
    {
        RootElement.style.display = DisplayStyle.Flex;
    }

    public override void Hide()
    {
        RootElement.style.display = DisplayStyle.None;
    }

    public void ChangeInterfaceState(MainMenuInterfaceState newState)
    {
        _currentInterfaceState = newState;
        
        switch (_currentInterfaceState)
        {
            case MainMenuInterfaceState.MainMenu:
                ShowMenu();
                break;
            case MainMenuInterfaceState.Settings:
                ShowSettings();
                break;
        }
    }

    private void ShowMenu()
    {
        _menuContainer.style.display = DisplayStyle.Flex;
        _settingsContainer.style.display = DisplayStyle.None;
    }

    private void ShowSettings()
    {
        _menuContainer.style.display = DisplayStyle.None;
        _settingsContainer.style.display = DisplayStyle.Flex;
    }

    private void OnChangeSceneButtonClicked()
    {
        SceneController.Instance
            .NewTransition()
            .Unload(SceneDatabase.Slots.Menu)
            .Load(SceneDatabase.Slots.Game, SceneDatabase.Scenes.Game)
            .Load(SceneDatabase.Slots.Session, SceneDatabase.Scenes.Session)
            .WithOverlay()
            .Perform();
    }
    
    private void OnSwitchButtonClicked()
    {
        ChangeInterfaceState(MainMenuInterfaceState.Settings);
    }
    
}
