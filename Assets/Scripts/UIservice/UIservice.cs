using Zenject;
using UnityEngine;
using GameState;
using Common;
using Video;

namespace UIservice
{
    public class UIService : IUIService
    {
        SignalBus signalBus;
        PlayUIView playView;
        GameOverUIView overView;
        LevelFinishedUIView finishedUIView;
        LobbyUIView lobbyUIView;

        IUIController currentUI, previousUI;

        public UIService(SignalBus signalBus)
        {
            this.signalBus = signalBus;
            signalBus.Subscribe<StateChangeSignal>(OnGameStateChanged);
            playView = GameObject.FindObjectOfType<PlayUIView>();
            overView = GameObject.FindObjectOfType<GameOverUIView>();
            finishedUIView = GameObject.FindObjectOfType<LevelFinishedUIView>();
            lobbyUIView = GameObject.FindObjectOfType<LobbyUIView>();
            playView.gameObject.SetActive(false);
            overView.gameObject.SetActive(false);
            finishedUIView.gameObject.SetActive(false);
        }
        public void OnGameStateChanged(StateChangeSignal state)
        {
            switch (state.newGameState)
            {
                case GameStatesType.PLAYERSTATE:
                    if (currentUI != null)
                    {
                        previousUI = currentUI;
                    }
                    else
                    {
                        currentUI = new PlayUIController(playView);
                    }
                    if (currentUI.GetUIState() != GameStatesType.PLAYERSTATE)
                    {
                        currentUI = new PlayUIController(playView);
                        if (previousUI != null && previousUI.GetUIState() != GameStatesType.PLAYERSTATE)
                        {
                            previousUI.DestroyUI();
                        }
                    }
                    break;
                case GameStatesType.GAMEOVERSTATE:
                    previousUI = currentUI;
                    previousUI.DestroyUI();
                    currentUI = new GameOverUIController(overView);
                    overView.DisplayFailCG();
                    break;
                case GameStatesType.GAMERETRYSTATE:
                    previousUI = currentUI;
                    previousUI.DestroyUI();
                    currentUI = new GameOverUIController(overView);
                    break;
                case GameStatesType.LEVELFINISHEDSTATE:
                    previousUI = currentUI;
                    previousUI.DestroyUI();
                    currentUI = new LevelFinishedUIController(finishedUIView);
                    break;
                case GameStatesType.LOBBYSTATE:
                    previousUI = currentUI;
                    if (previousUI != null)
                        previousUI.DestroyUI();

                    currentUI = new LobbyUIController(lobbyUIView);
                    break;
                case GameStatesType.GAMECOMPLETED:
                    GameCompleted();
                    break;

            }
               
        }

        public SignalBus ReturnSignalBus()
        {
            return signalBus; 
        }

        /// <summary>
        /// 游戏通关
        /// </summary>
        private void GameCompleted()
        {
            // 播放通关CG
            PlayCompleteCG();

        }

        private void PlayCompleteCG()
        {
            VideoManager.Instance.Play("End.mp4",
                onstart: (videoplayer) =>
                {
                    VideoManager.Instance.IsShowOnEnd = true;
                    Time.timeScale = 0;
                },
                onend: (videoplayer) =>
                {
                    VideoManager.Instance.IsShowOnEnd = true;
                    Time.timeScale = 1;
                    // 回到关卡选择界面
                    OnGameStateChanged(new StateChangeSignal() { newGameState = GameStatesType.LOBBYSTATE });
                }
            );
        }
    }
}