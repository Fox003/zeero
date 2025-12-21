using UnityEngine;
using UnityEngine.UIElements;

public class TestScreen2 : UIScreen
{
    public static TestScreen2 Instantiate(VisualElement ParentElement)
    {
        var instance = ScriptableObject.CreateInstance<TestScreen2>();
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
