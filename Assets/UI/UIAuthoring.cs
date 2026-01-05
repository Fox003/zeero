using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Unity.Entities;
using UnityEngine;
using UnityEngine.UIElements;

class UIAuthoring : MonoBehaviour
{
    public UIDocument UIDoc;
}

class UIAuthoringBaker : Baker<UIAuthoring>
{
    public override void Bake(UIAuthoring authoring)
    {
        var entity = GetEntity(TransformUsageFlags.None);
        VisualElement root =  authoring.UIDoc.rootVisualElement;
        
        AddComponent(entity, new UIScreens()
        {
            GameFightingScreen = GameFightingScreen.Instantiate(root.Q<VisualElement>("game_fighting_screen")),
            GameCountdownScreen = GameCountdownScreen.Instantiate(root.Q<VisualElement>("game_countdown_screen")),
            GameUpgradePhaseScreen = UpgradePhaseScreen.Instantiate(root.Q<VisualElement>("game_upgrade_screen")),
            WaitingForPlayersScreen = WaitingForPlayersScreen.Instantiate(root.Q<VisualElement>("game_waiting_for_players_screen")),
            GameOverScreen = GameOverScreen.Instantiate(root.Q<VisualElement>("game_gameover_screen")),
        });
        
        AddComponentObject(entity, new GameFightingViewModel());
        AddComponentObject(entity, new GameUpgradesViewModel());
    }
}


