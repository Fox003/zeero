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

    public static void OnInitStateEnter(Entity uiFsm, Entity gameFsm, DynamicBuffer<EnableStateRequest> uiFSMBuffer, DynamicBuffer<EnableStateRequest> gameFSMBuffer)
    {
        // 1. Load new Map
        // 2. Reset player position

        // 3. Change to starting game state when done
        gameFSMBuffer.Add(new EnableStateRequest()
        {
            Entity = gameFsm,
            StateToEnable = GameFSMStates.COUNTDOWN_STATE,
        });

        uiFSMBuffer.Add(new EnableStateRequest()
        {
            Entity = uiFsm,
            StateToEnable = UIFSMStates.GAME_COUNTDOWN_STATE
        });
    }

    public static void OnFightingStateEnter(ref GameTimerData timerData)
    {
        timerData.IsPaused = false;
    }
}