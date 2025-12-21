using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

public class GameFightingScreen : UIScreen
{
    private VisualElement _mainContainer;
    private VisualElement _timerContainer;
    private Label _timerLabel;
    
    public static GameFightingScreen Instantiate(VisualElement ParentElement)
    {
        var instance = ScriptableObject.CreateInstance<GameFightingScreen>();
        instance.RootElement =  ParentElement;
        instance._mainContainer = instance.RootElement.Q<VisualElement>("main_container");
        instance._timerContainer = instance._mainContainer.Q<VisualElement>("timer_container");
        
        instance._timerLabel = instance._timerContainer.Q<Label>("timer_label");

        return instance;
    }


    public void BindData(GameFightingViewModel data)
    {
        _mainContainer.dataSource = data;
    }
}

