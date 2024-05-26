﻿using UnityEngine;
using Common;
using SoundSystem;

namespace InteractableSystem
{
    public class AmbushPlantController : InteractableController
    {
        private InteractableManager interactableManager;

        int targetNodeID;

        public AmbushPlantController(Vector3 nodePos, InteractableManager interactableManager, InteractableView ambushPlantPrefab)
        {
            this.interactableManager = interactableManager;
            GameObject anbushPlant = GameObject.Instantiate<GameObject>(ambushPlantPrefab.gameObject);
            interactableView = anbushPlant.GetComponent<AmbushPlantView>();
            interactableView.SetController(this);
            interactableView.transform.position = nodePos;
        }

        protected override void OnInitialized()
        {
            interactablePickup = InteractablePickup.AMBUSH_PLANT;
        }

        public override bool CanTakeAction(int playerNode, int nodeID)
        {
            return true;
        }

        public override void TakeAction(int nodeID)
        {

            //interactableManager.RemoveInteractable(this);
        }

        public override void InteractablePickedUp()
        {
            interactableManager.ReturnSignalBus().TryFire(new SignalPlayOneShot()
            { soundName = SoundName.ambushPlant });
            //base.InteractablePickedUp();
        }
    }
}