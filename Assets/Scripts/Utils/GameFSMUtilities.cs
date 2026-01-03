using System;
using System.Collections.Generic;
using System.Reflection;
using Unity.Entities;
using UnityEngine;

public static class GameFSMUtilities
{
    public enum GameState {
        None = 0,
        StartingState,
        FightingState,
        GameOverState,
    }

    public static void OnInitStateEnter()
    {
        // 1. Load new Map
        // 2. Reset player position
        // 3. Reset player state components (MovementState, WeaponState, etc.)

        
    }

    public static void OnInitLoadNewMap()
    {

    }
    
    public static void OnInitResetPlayerPosition()
    {

    }

    public static void OnInitResetPlayerStates()
    {

    }
}