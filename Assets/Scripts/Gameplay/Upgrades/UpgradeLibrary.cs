using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UpgradeLibrary", menuName = "Scriptable Objects/UpgradeLibrary")]
public class UpgradeLibrary : ScriptableObject
{
    public List<UpgradeSO> Upgrades;
}
