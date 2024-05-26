﻿using Common;
using PathSystem.NodesScript;
using Player;
using UnityEngine;

namespace InputSystem
{
    public class TouchInput : IInputComponent
    {
        private IInputService inputService;
        private Vector2 startPos, endPos;
        private Directions direction;
        private int nodeLayer = 1 << 9;
        GameObject gameObject;

        public TouchInput()
        {
        }

        public void OnInitialized(IInputService inputService)
        {
            this.inputService = inputService;
        }

        public void OnTick()
        {
            if (Input.touchCount >= 1)
            {
                Touch touch = Input.GetTouch(0);

                if (touch.phase == TouchPhase.Began)
                {
                    gameObject = inputService.GetTapDetect().ReturnObject(touch.position, nodeLayer);

                    if (gameObject != null)
                    {
                        if (gameObject.GetComponent<NodeControllerView>() != null)
                        {
                            inputService.PassNodeID(gameObject.GetComponent<NodeControllerView>().nodeID);
                        }
                        else if (gameObject.GetComponent<PlayerView>() != null)
                        {
                            startPos = touch.position;
                            endPos = touch.position;
                        }
                    }
                }
                else if (touch.phase == TouchPhase.Ended)
                {
                    if (gameObject != null)
                    {
                        if (gameObject.GetComponent<PlayerView>() != null)
                        {
                            endPos = touch.position;

                            inputService.PassDirection(inputService.GetSwipeDirection()
                            .GetDirection(startPos, endPos));
                            gameObject = null;
                        }
                    }
                }

            }
        }

        public void StartPosition(Vector3 pos)
        {
            //startPos = pos;
        }
    }
}