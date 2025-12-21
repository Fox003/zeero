using System;
using DG.Tweening;
using Unity.Entities;
using UnityEngine;
using UnityEngine.UIElements;

public abstract class UIScreen : ScriptableObject
{
    public const string k_VisibleClass = "screen-visible";
    public const string k_HiddenClass = "screen-hidden";

    public EntityCommandBuffer ECB { get; set; }
    
    public VisualElement RootElement { get; set; }

    public virtual void Show()
    {
        RootElement.style.display = DisplayStyle.Flex;
    }

    public virtual void Hide()
    {
        RootElement.style.display = DisplayStyle.None;
    }
}
