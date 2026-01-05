using UnityEngine;
using UnityEngine.UIElements;

public class GameOverScreen : UIScreen
{
    private VisualElement _mainContainer;
    private Button _button;

    public static GameOverScreen Instantiate(VisualElement ParentElement)
    {
        var instance = ScriptableObject.CreateInstance<GameOverScreen>();
        instance.RootElement = ParentElement;
        instance._mainContainer = instance.RootElement.Q<VisualElement>("main_container");
        instance._button = instance._mainContainer.Q<Button>("play_again_button");

        instance._button.clicked += instance.PlayAgain;

        return instance;
    }

    private void PlayAgain()
    {
        TriggerStateChange(GameFSMStates.WAITING_FOR_PLAYERS_STATE, UIFSMStates.GAME_WAITING_FOR_PLAYERS);
    }
}
