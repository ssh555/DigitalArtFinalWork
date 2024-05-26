using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Video;

namespace GameSequence
{
    public class StartGame : MonoBehaviour
    {
        private void Start()
        {
            VideoManager.Instance.Play("¿ª¶Ë.mp4");
        }

        public void PlayGame()
        {
            SceneManager.LoadSceneAsync("GameScene");

        }
    }

}
