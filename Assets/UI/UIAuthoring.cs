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
            TestScreen1 = TestScreen1.Instantiate(root.Q<VisualElement>("test_screen_1")),
            TestScreen2 = TestScreen2.Instantiate(root.Q<VisualElement>("test_screen_2")),
            GameStartScreen = GameStartScreen.Instantiate(root.Q<VisualElement>("game_start_screen")),
            GameFightingScreen = GameFightingScreen.Instantiate(root.Q<VisualElement>("game_fighting_screen")),
            //GameCountdownScreen = GameStartScreen.Instantiate(root.Q<VisualElement>("game_countdown_screen")),
        });
        
        AddComponentObject(entity, new GameFightingViewModel());
    }
}


