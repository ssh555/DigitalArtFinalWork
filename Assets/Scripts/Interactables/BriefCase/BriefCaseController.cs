﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;
using Zenject;

namespace InteractableSystem
{
    public class BriefCaseController : InteractableController
    {
        private InteractableManager interactableManager;

        public BriefCaseController(Vector3 nodePos, InteractableManager interactableManager
                                            , InteractableView briefCasePrefab)
        {
            this.interactableManager = interactableManager;
            GameObject briefCase = GameObject.Instantiate<GameObject>(briefCasePrefab.gameObject);
            interactableView = briefCase.GetComponent<BriefCaseView>();
            interactableView.transform.position = nodePos;
        }

        protected override void OnInitialized()
        {
            interactablePickup = InteractablePickup.BREIFCASE;
        }

        public override bool CanTakeAction(int playerNode, int nodeID)
        {
            return true;
        }

        public override void TakeAction(int nodeID)
        {
            interactableManager.ReturnSignalBus().Fire<BriefCaseSignal>();
            interactableView.gameObject.SetActive(false);
            interactableManager.RemoveInteractable(this);
        }

        public override void InteractablePickedUp()
        {
            base.InteractablePickedUp();
        }
    }
}