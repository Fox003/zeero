using System;
using UnityEngine;
using UnityEngine.UIElements;

public class GameCountdownScreen : UIScreen
{
    private VisualElement _mainContainer;
    private Label _countdownLabel;
    
    private int _countdownTime = 4;
    private int _currentCountdown;
    
    private Timer _timer;
    
    public static GameCountdownScreen Instantiate(VisualElement ParentElement)
    {
        var instance = ScriptableObject.CreateInstance<GameCountdownScreen>();
        instance.RootElement =  ParentElement;
        
        instance._mainContainer = instance.RootElement.Q<VisualElement>("main_container");
        instance._countdownLabel = instance._mainContainer.Q<Label>("countdown_label");

        return instance;
    }

    public override void Show()
    {
        RootElement.style.display = DisplayStyle.Flex;
        _currentCountdown = _countdownTime - 1;

        _timer = new Timer(
            durationSeconds: _countdownTime, 
            refreshInterval: TimeSpan.FromSeconds(1),
            onTick: AnimateCountdown,
            onFinish: OnTimerComplete);
        
        _timer.Start();
    }

    public override void Hide()
    {
        RootElement.style.display = DisplayStyle.None;
    }
    

    private void AnimateCountdown()
    {
        _countdownLabel.text = _currentCountdown.ToString();
        _currentCountdown--;
    }

    private void OnTimerComplete()
    {
        var entity = ECB.CreateEntity();
        
        ECB.AddComponent(entity, new UIStateChangeEvent()
        {
            NewGameState = GameFSMStates.FIGHTING_STATE,
            NewUIState = UIFSMStates.GAME_FIGHTING_STATE
        });
        
        ECB.AddComponent<UIEvent>(entity);
    }
}
