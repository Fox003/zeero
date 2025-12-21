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
}