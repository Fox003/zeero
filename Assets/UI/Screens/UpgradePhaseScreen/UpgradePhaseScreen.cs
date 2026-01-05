using Unity.Entities;
using UnityEngine;
using UnityEngine.InputSystem.UI;
using UnityEngine.UIElements;

public class UpgradePhaseScreen : UIScreen
{
    private GameUpgradesViewModel _viewModel;
    private VisualElement _mainContainer;
    private VisualElement _upgradesContainer;
    private Button _getUpgradeButton1;
    private Button _getUpgradeButton2;
    private Button _getUpgradeButton3;
    private Button _testButton;
    private Label _upgradingPlayerLabel;

    public static UpgradePhaseScreen Instantiate(VisualElement ParentElement)
    {
        var instance = ScriptableObject.CreateInstance<UpgradePhaseScreen>();
        instance.RootElement = ParentElement;
        instance._mainContainer = instance.RootElement.Q<VisualElement>("main_container");
        instance._upgradesContainer = instance.RootElement.Q<VisualElement>("upgrades_container");
        instance._getUpgradeButton1 = instance._upgradesContainer.Q<VisualElement>("upgrade1").Q<Button>("get_upgrade_button");
        instance._getUpgradeButton2 = instance._upgradesContainer.Q<VisualElement>("upgrade2").Q<Button>("get_upgrade_button");
        instance._getUpgradeButton3 = instance._upgradesContainer.Q<VisualElement>("upgrade3").Q<Button>("get_upgrade_button");
        instance._testButton = instance._mainContainer.Q<Button>("test_button");
        instance._upgradingPlayerLabel = instance._mainContainer.Q<Label>("player_upgrading_label");

        instance._getUpgradeButton1.clicked += () => instance.OnGetUpgradeButtonClicked(instance._getUpgradeButton1, instance._viewModel.Upgrade1);
        instance._getUpgradeButton2.clicked += () => instance.OnGetUpgradeButtonClicked(instance._getUpgradeButton2, instance._viewModel.Upgrade2);
        instance._getUpgradeButton3.clicked += () => instance.OnGetUpgradeButtonClicked(instance._getUpgradeButton3, instance._viewModel.Upgrade3);
        instance._testButton.clicked += instance.OnButtonClicked;

        return instance;
    }

    private void OnButtonClicked()
    {
        TriggerStateChange(GameFSMStates.INIT_STATE, UIFSMStates.HIDDEN_STATE);
    }

    private void OnGetUpgradeButtonClicked(Button buttonClicked, UpgradeDefinition upgrade)
    {
        TriggerModifierAdd(upgrade, _viewModel.CurrentUpgradingPlayer);

        buttonClicked.SetEnabled(false);
    }

    private void TriggerModifierAdd(UpgradeDefinition upgrade, Entity Player)
    {
        var entity = ECB.CreateEntity();

        ECB.AddComponent(entity, new AddUpgradeToPlayerEvent()
        {
            UpgradeToAdd = upgrade,
            Player = Player
        });

        ECB.AddComponent<UIEvent>(entity);
    }


    public void BindData(GameUpgradesViewModel data)
    {
        _mainContainer.dataSource = data;
        _viewModel = data;

        _getUpgradeButton1.SetEnabled(true);
        _getUpgradeButton2.SetEnabled(true);
        _getUpgradeButton3.SetEnabled(true);
         
        _getUpgradeButton1.Focus();

        
    }
}
