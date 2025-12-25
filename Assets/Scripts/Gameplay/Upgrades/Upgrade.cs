using Unity.Entities;
using UnityEngine;

[CreateAssetMenu(fileName = "Upgrade", menuName = "Scriptable Objects/Upgrade")]
public class UpgradeSO : ScriptableObject
{
    public string Name;
    [TextArea] public string Description;
    public Rarity Rarity;   
    public float Duration;
    public PlayerStatsModifiers Modifiers;
}

public enum Rarity { 
    Common,
    Uncommon,
    Rare,
    VeryRare,
    Epic,
    Legendary
}
