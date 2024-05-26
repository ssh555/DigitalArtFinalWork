﻿using UnityEngine;
using System.Collections;
using System.Threading.Tasks;
using InteractableSystem;
using Common;

namespace Player
{
    public interface IPlayerController
    {
        Vector3 GetCurrentLocation();
        Task MoveToLocation(Vector3 location);
        void DisablePlayer();
        void Reset();
        Task ChangePlayerState(PlayerStates _state, PlayerStates stateToChange, IInteractableController interactableController = null);
        PlayerStates GetPlayerState();
        int GetID();

        Task PerformMovement(int _nodeID);
        void PerformAction(Directions direction);
        EnemyType GetDisguiseType();
        void SetDisguiseType(DisguiseSignal disguiseSignal);
        Task PlayAnimation(PlayerStates dEAD);
    }
}