using UnityEngine;
using UnityEngine.UIElements;

public class UpgradePhaseScreen : UIScreen
{
    private VisualElement _mainContainer;
    private Button _testButton;

    public static UpgradePhaseScreen Instantiate(VisualElement ParentElement)
    {
        var instance = ScriptableObject.CreateInstance<UpgradePhaseScreen>();
        instance.RootElement = ParentElement;
        instance._mainContainer = instance.RootElement.Q<VisualElement>("main_container");
        instance._testButton = instance._mainContainer.Q<Button>("test_button");

        instance._testButton.clicked += instance.OnButtonClicked;

        return instance;
    }

    private void OnButtonClicked()
    {
        var entity = ECB.CreateEntity();

        ECB.AddComponent(entity, new UIStateChangeEvent()
        {
            NewGameState = GameFSMStates.INIT_STATE,
            NewUIState = UIFSMStates.HIDDEN_STATE
        });

        ECB.AddComponent<UIEvent>(entity);
    }


    public void BindData(GameFightingViewModel data)
    {
        _mainContainer.dataSource = data;
    }
}
