﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameState;
using Zenject;
using PathSystem;
using Common;
using StarSystem;
using SavingSystem;

namespace UIservice
{
    public class MenuUIControllerView : MonoBehaviour
    {

        [Inject] IGameService gameService;
        [Inject] IPathService pathService;
        [Inject] ISaveService saveService;
        bool levelComplete;
        [Inject] IStarService starService;
        List<CardControllerView> cards = new List<CardControllerView>();
        public Button nextButton, retryButton, LobyButton;
        public RectTransform starPanalTransform;
        public GameObject card;
        public void SetLevelFinishedMenu()
        {
            ClearCards();
            nextButton.gameObject.SetActive(true);
            LobyButton.gameObject.SetActive(false);
            levelComplete = true;
            SetCards();
        }
        void ClearCards()
        {
            for (int i = 0; i < cards.Count; i++)
            {
                Destroy(cards[i].gameObject);
            }
            cards.Clear();
        }
        public void SetLevelPausedMenu()
        {
            ClearCards();
            levelComplete = false;
            nextButton.gameObject.SetActive(false);
            LobyButton.gameObject.SetActive(true);
            SetCards();
        }
       
        // Start is called before the first frame update
        private void OnEnable()
        {
            LobyButton.onClick.AddListener(GoToLobby);
            nextButton.onClick.AddListener(LoadNext);
            retryButton.onClick.AddListener(Retry);
        }
        void OnDisable()
        {
            LobyButton.onClick.RemoveListener(GoToLobby);
            nextButton.onClick.RemoveListener(LoadNext);
            retryButton.onClick.RemoveListener(Retry);
        }
        public void GoToLobby()
        {
            gameService.ChangeToLobbyState();
        }
        void LoadNext()
        {
            gameService.IncrimentLevel();
            gameService.ChangeToLoadLevelState();
        }
        void Retry()
        {
            gameService.ChangeToGameRetryState();
        }
        public void SetCards()
        {
            List<StarData> stars = pathService.GetStarsForLevel();
            CardControllerView cardview;
            for (int i = 0; i < stars.Count; i++)
            { 
                cardview = Instantiate(card, starPanalTransform).GetComponent<CardControllerView>();
                cardview.setCardName(stars[i].name);
                cardview.SetAchievement(saveService.ReadStarTypeForLevel(gameService.GetCurrentLevel(),stars[i].type));
                cards.Add(cardview);
            }
        }
        // Update is called once per frame
    }
}
