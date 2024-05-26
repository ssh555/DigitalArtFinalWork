using System.Collections.Generic;
using Common;
namespace GameState
{
    public interface IGameService
    {
         bool IncrimentMaxLevel();
         GameStatesType GetCurrentState();
         void ChangeToPlayerState();
         int GetCurrentLevel();
         void ChangeToLobbyState();
         List<StarData> GetStarsForLevel(int level);
         void SetCurrentLevel(int level);
         void ChangeToEnemyState();
         void ChangeToGameOverState();
         void ChangeToLoadLevelState();
        void ChangeToGameRetryState();
        void IncrimentLevel();
         int GetNumberOfLevels();
    }
}