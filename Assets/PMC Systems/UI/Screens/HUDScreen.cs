using UnityEngine;
using UnityEngine.UIElements;

public class HUDScreen : UIScreen
{
    public static HUDScreen Instantiate(VisualElement ParentElement)
    {
        var instance = ScriptableObject.CreateInstance<HUDScreen>();
        instance.RootElement =  ParentElement;

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
}
