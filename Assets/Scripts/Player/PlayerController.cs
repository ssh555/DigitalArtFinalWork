﻿using Common;
using GameState;
using InteractableSystem;
using PathSystem;
using SoundSystem;
using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Player
{
    public class PlayerController : IPlayerController
    {
        private IPlayerService playerService;
        private IPlayerView currentPlayerView;
        private IGameService gameService;
        private IInteractable interactableService;
        private IPathService pathService;
        private PlayerStateMachine playerStateMachine;
        private PlayerScriptableObject scriptableObject;
        private int playerNodeID;
        GameObject playerInstance;
        private Vector3 spawnLocation;
        private EnemyType disguiseType = EnemyType.None;
        readonly SignalBus signalBus;

        public PlayerController(IPlayerService _playerService, IGameService _gameService, IPathService _pathService, IInteractable _interactableService, PlayerScriptableObject _playerScriptableObject, SignalBus signalBus)
        {
            this.signalBus = signalBus;
            playerService = _playerService;
            pathService = _pathService;
            gameService = _gameService;
            interactableService = _interactableService;

            scriptableObject = _playerScriptableObject;

            playerNodeID = pathService.GetPlayerNodeID();
            spawnLocation = pathService.GetNodeLocation(playerNodeID);

            SpawnPlayerView();
            playerService.GetSignalBus().Subscribe<DisguiseSignal>(SetDisguiseType);
        }

        public Vector3 GetCurrentLocation()
        {
            return spawnLocation;
        }

        async public Task MoveToLocation(Vector3 _location)
        {
            signalBus.TryFire(new SignalPlayOneShot()
            { soundName = SoundName.playerMove });
            await currentPlayerView.MoveToLocation(_location);
        }

        private void SpawnPlayerView()
        {
            // currentPlayerView=scriptableObject.playerView;
            playerInstance = GameObject.Instantiate(scriptableObject.playerView.gameObject);
            currentPlayerView = playerInstance.GetComponent<PlayerView>();
            playerStateMachine = new PlayerStateMachine(currentPlayerView, playerService);
            playerNodeID = 0;
            playerInstance.transform.localPosition = spawnLocation;
        }

        public void DisablePlayer()
        {
            currentPlayerView.DisablePlayer();
        }

        public void Reset()
        {
            playerService.GetSignalBus().Unsubscribe<DisguiseSignal>(SetDisguiseType);
            currentPlayerView.Reset();
        }

        private PlayerStateMachine GetCurrentStateMachine()
        {
            return playerStateMachine;
        }

        async public Task ChangePlayerState(PlayerStates _state, PlayerStates stateToChange, IInteractableController interactableController = null)
        {
            await playerStateMachine.ChangePlayerState(_state, stateToChange, interactableController);

        }
        public PlayerStates GetPlayerState()
        {
            return playerStateMachine.GetPlayerState();
        }

        async public Task PerformMovement(int nextNodeID)
        {

            Vector3 nextLocation = pathService.GetNodeLocation(nextNodeID);
            if (pathService.CheckIfTeleportable(playerNodeID, nextNodeID))
            {
                pathService.HighlightTeleportNodes(nextNodeID);
            }
            //else { 
            //    pathService.UnhighlightTeleportableNodes();
            //}

            await MoveToLocation(nextLocation);
            playerNodeID = nextNodeID;


            if (interactableService.CheckForInteractable(nextNodeID))
            {
                IInteractableController interactableController = interactableService.ReturnInteractableController(nextNodeID);
                PerformInteractableAction(interactableController);
            }
            if (IsGameFinished())
            {

                playerService.FireLevelFinishedSignal();

            }
            else if (GetPlayerState() != PlayerStates.WAIT_FOR_INPUT)
            {
                gameService.ChangeToEnemyState();
            }
        }
        async public void PerformInteractableAction(IInteractableController _interactableController)
        {
            int nodeID = playerService.GetTargetNode();

            switch (_interactableController.GetInteractablePickup())
            {
                case InteractablePickup.AMBUSH_PLANT:
                    await ChangePlayerState(PlayerStates.AMBUSH, PlayerStates.NONE);
                    _interactableController.TakeAction(playerNodeID);
                    break;
                case InteractablePickup.BONE:
                    pathService.ShowThrowableNodes(playerNodeID);
                    playerService.SetTargetTap(-1);
                    await ChangePlayerState(PlayerStates.WAIT_FOR_INPUT, PlayerStates.THROWING, _interactableController);

                    break;
                case InteractablePickup.BREIFCASE:
                    await ChangePlayerState(PlayerStates.IDLE, PlayerStates.NONE);
                    _interactableController.TakeAction(playerNodeID);
                    await new WaitForEndOfFrame();
                    break;
                case InteractablePickup.COLOR_KEY:
                    await ChangePlayerState(PlayerStates.IDLE, PlayerStates.NONE);
                    _interactableController.TakeAction(playerNodeID);

                    break;
                case InteractablePickup.DUAL_GUN:
                    await ChangePlayerState(PlayerStates.IDLE, PlayerStates.NONE);
                    _interactableController.TakeAction(playerNodeID);

                    break;
                case InteractablePickup.GUARD_DISGUISE:
                    await ChangePlayerState(PlayerStates.DISGUISE, PlayerStates.NONE);
                    _interactableController.TakeAction(playerNodeID);

                    break;
                case InteractablePickup.SNIPER_GUN:
                    playerService.SetTargetTap(-1);
                    currentPlayerView.PlayAnimation(PlayerStates.SHOOTING);
                    await ChangePlayerState(PlayerStates.WAIT_FOR_INPUT, PlayerStates.SHOOTING, _interactableController);

                    break;
                case InteractablePickup.STONE:
                    pathService.ShowThrowableNodes(playerNodeID);
                    playerService.SetTargetTap(-1);
                    await ChangePlayerState(PlayerStates.WAIT_FOR_INPUT, PlayerStates.THROWING, _interactableController);
                    break;
                case InteractablePickup.TRAP_DOOR:
                    playerService.SetTargetTap(-1);
                    await ChangePlayerState(PlayerStates.WAIT_FOR_INPUT, PlayerStates.UNLOCK_DOOR, _interactableController);
                    break;
            }
            await new WaitForEndOfFrame();

        }

        public int GetID()
        {
            return playerNodeID;
        }

        public EnemyType GetDisguiseType()
        {
            return disguiseType;
        }

        public void SetDisguiseType(DisguiseSignal disguiseSignal)
        {
            disguiseType = disguiseSignal.enemyType;

            currentPlayerView.SetDisguise(disguiseType);
        }

        private bool IsGameFinished()
        {
            return pathService.CheckForTargetNode(playerNodeID);
        }

        async public void PerformAction(Directions _direction)
        {
            int nextNodeID = pathService.GetNextNodeID(GetID(), _direction);
            if (nextNodeID == -1)
            {
                ChangePlayerState(PlayerStates.IDLE, PlayerStates.NONE);
                return;
            }
            await PerformMovement(nextNodeID);

        }

        async public Task PlayAnimation(PlayerStates state)
        {
            await currentPlayerView.PlayAnimation(state);
        }
    }
}