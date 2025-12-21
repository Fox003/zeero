using UnityEngine;
using UnityEngine.UIElements;

public class TestScreen1 : UIScreen
{
    public static TestScreen1 Instantiate(VisualElement ParentElement)
    {
        var instance = ScriptableObject.CreateInstance<TestScreen1>();
        instance.RootElement =  ParentElement;
        
        return instance;
    }
}
