using UnityEngine;
using UnityEngine.UIElements;

public class WaitingForPlayersScreen : UIScreen
{
    private VisualElement _mainContainer;
    private VisualElement _label;

    public static WaitingForPlayersScreen Instantiate(VisualElement ParentElement)
    {
        var instance = ScriptableObject.CreateInstance<WaitingForPlayersScreen>();
        instance.RootElement = ParentElement;
        instance._mainContainer = instance.RootElement.Q<VisualElement>("main_container");
        instance._label = instance._mainContainer.Q<VisualElement>("waiting_title");

        instance.StartBreathingAnimation(instance._label);

        return instance;
    }

    private void StartBreathingAnimation(VisualElement target)
    {
        // Scale Up to 1.1x
        target.experimental.animation.Scale(1.1f, 1000)
            .OnCompleted(() =>
            {
                // Scale Down to 0.9x
                target.experimental.animation.Scale(0.9f, 1000)
                    .OnCompleted(() => StartBreathingAnimation(target)); // Loop back
            });
    }
}
