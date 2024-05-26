using Zenject;
using UnityEngine;
using Enemy;
using GameState;
using Common;
using InteractableSystem;
namespace StarSystem
{
    public class StarSystemService : IStarService
    {
        bool noKill = true, briefCase = false, completed = false, killalldogs = false, killnodogs = true;
        int totalEnemyInLevel, killCount = 0, playerMoves = 0, maxPlayerMoves;
        SignalBus signalBus;
        StarSystemService(SignalBus signalBus)
        {
            //PlayerPrefs.DeleteAll();
            this.signalBus = signalBus;
            signalBus.Subscribe<BriefCaseSignal>(BriefCasePicked);
            signalBus.Subscribe<EnemyDeathSignal>(IncreaseKillCount);
            signalBus.Subscribe<StateChangeSignal>(StateChanged);
            signalBus.Subscribe<LevelFinishedSignal>(Finished);
        }
        public void Finished()
        {
            completed = true;
            Debug.Log("compled this level" + completed);
        }
        public void SetTotalEnemyandMaxPlayerMoves(int enemy, int playerMoves)
        {
            totalEnemyInLevel = enemy;
            maxPlayerMoves = playerMoves;
        }
        void IncreaseKillCount()
        {

            noKill = false;
            killCount++;

        }
        void BriefCasePicked()
        {
            briefCase = true;
        }
        void StateChanged(StateChangeSignal state)
        {
            GameStatesType stateType = state.newGameState;
            switch (stateType)
            {
                case GameStatesType.PLAYERSTATE: playerMoves++; break;
                case GameStatesType.GAMEOVERSTATE: Reset(); break;
                case GameStatesType.LOADLEVELSTATE: Reset(); break;
            }
        }
        void Reset()
        {

            briefCase = false;
            killalldogs = false;
            killnodogs = true;
            killCount = 0;
            noKill = true;
            playerMoves = 0;
        }
        public bool CheckForStar(StarTypes starType)
        {
            bool result = false;
            switch (starType)
            {
                case StarTypes.COMPLETED: result = completed; Debug.Log("compled this level"); break;
                case StarTypes.ALLKILL: result = (killCount == totalEnemyInLevel); break;
                case StarTypes.NOKILL: result = noKill; break;
                case StarTypes.PICKBRIEFCASE: result = briefCase; break;
                case StarTypes.PLAYERMOVES: result = playerMoves < maxPlayerMoves; break;
                case StarTypes.KILLDOGS: result = killalldogs; break;
                case StarTypes.NOKILLDOGS: result = killnodogs; break;
            }
            return result;
        }

        public void DogsKilled()
        {
            killnodogs = false;
        }

        public void AllDogsKilled()
        {
            killalldogs = true;
        }
    }
}