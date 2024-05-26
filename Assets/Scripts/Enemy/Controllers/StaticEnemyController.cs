﻿using Common;
using PathSystem;
using Player;
using System.Threading.Tasks;
using GameState;
using System.Collections;
using UnityEngine;
using Zenject;

namespace Enemy
{
    public class StaticEnemyController : EnemyController
    {
        

        public StaticEnemyController(IEnemyService _enemyService, IPathService _pathService, IGameService _gameService, Vector3 _spawnLocation, EnemyScriptableObject _enemyScriptableObject, int currentNodeID, Directions spawnDirection,bool _hasShield) : base(_enemyService, _pathService, _gameService, _spawnLocation, _enemyScriptableObject, currentNodeID, spawnDirection,_hasShield)
        {
            enemyType = EnemyType.STATIC;

        }
        async protected override Task MoveToNextNode(int nodeID)
        {
            if(stateMachine.GetEnemyState()==EnemyStates.CHASE)
            {
                spawnDirection= pathService.GetDirections(currentNodeID, nodeID);
                
                Vector3 rot=GetRotation(spawnDirection);
                await currentEnemyView.RotateEnemy(rot);
                await currentEnemyView.MoveToLocation(pathService.GetNodeLocation(nodeID));
                currentNodeID = nodeID;
              

            }
            if (CheckForPlayerPresence(nodeID))
            {
                if(!currentEnemyService.CheckForKillablePlayer(GetEnemyType()))
                {
                    return;
                }           
                  await currentEnemyView.MoveToLocation(pathService.GetNodeLocation(nodeID));

                currentNodeID = nodeID;
                currentEnemyService.TriggerPlayerDeath();
            }
          
        }

        protected override void SetController()
        {
            currentEnemyView.SetCurrentController(this);
        }

        
    }
}