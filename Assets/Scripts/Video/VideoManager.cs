using CameraSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

namespace Video
{
    public class VideoManager : MonoBehaviour
    {
        #region Instance
        public static VideoManager Instance;
        #endregion

        #region Unity

        private void Awake()
        {
            if(Instance != null)
            {
                Destroy(this);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(this);

            videoPlayer = this.GetComponent<VideoPlayer>();
            videoCamera = this.GetComponentInChildren<Camera>();
            videoCamera.gameObject.SetActive(false);
        }

        private void Start()
        {
            // ��ʼ����ʱ��������UI
            videoPlayer.started += VideoPlayer_Start;
            // ��������ʱ��ʾ����UI
            videoPlayer.loopPointReached += VideoPlayer_End;
        }


        private void OnDestroy()
        {
            if(Instance == this)
            {
                Instance = null;
                videoPlayer.started -= VideoPlayer_Start;
                videoPlayer.loopPointReached -= VideoPlayer_End;
            }
        }
        #endregion

        #region VideoPlayer
        public readonly string VideoPath = Application.streamingAssetsPath + "/Videos/";
        public VideoPlayer videoPlayer;
        public Camera videoCamera;
        /// <summary>
        /// ��ʼ����ʱ�Ƿ���������UI
        /// </summary>
        public bool IsHideOnStart = true;
        /// <summary>
        /// ��������ʱ�Ƿ���ʾ����UI
        /// </summary>
        public bool IsShowOnEnd = true;

        /// <summary>
        /// ��Ƶ�������ʱ�Ļص�
        /// </summary>
        private System.Action<VideoPlayer> PlayOnEnd = null;
        /// <summary>
        /// ��Ƶ���ſ�ʼʱ�Ļص�
        /// </summary>
        private System.Action<VideoPlayer> PlayOnStart = null;
        public void Play(string path, System.Action<VideoPlayer> onstart = null, System.Action<VideoPlayer> onend = null)
        {
            // ���ò��ŵ���Ƶ
            videoPlayer.url = VideoPath + path;
            PlayOnStart = onstart;
            PlayOnEnd = onend;
            // ����Video
            videoPlayer.Play();
        }

        public void PlayByAbs(string abspath, System.Action<VideoPlayer> onstart = null, System.Action<VideoPlayer> onend = null)
        {
            // ���ò��ŵ���Ƶ
            videoPlayer.url = abspath;
            PlayOnStart = onstart;
            PlayOnEnd = onend;
            // ����Video
            videoPlayer.Play();
        }

        public void HideAllCanvas()
        {
            if(IsHideOnStart)
            {
                SetAllCanvasActive(false);
            }
        }

        public void ShowAllCanvas()
        {
            if(IsShowOnEnd)
            {
                SetAllCanvasActive(true);
            }
        }

        private void SetAllCanvasActive(bool active)
        {
            foreach (Canvas canvas in GameObject.FindObjectsByType(typeof(Canvas), FindObjectsInactive.Include, FindObjectsSortMode.None))
            {
                canvas.gameObject.SetActive(active);
            }
        }

        #region Events
        public Camera _camera;
        private void VideoPlayer_Start(VideoPlayer source)
        {
            _camera = Camera.main;
            _camera.gameObject.SetActive(false);
            HideAllCanvas();
            PlayOnStart?.Invoke(source);
            PlayOnStart = null;
            videoCamera.gameObject.SetActive(true);
        }

        private void VideoPlayer_End(VideoPlayer source)
        {
            _camera.gameObject.SetActive(true);
            ShowAllCanvas();
            PlayOnEnd?.Invoke(source);
            PlayOnEnd = null;
            Invoke("ShowCamera", 0.02f);
        }

        private void ShowCamera()
        {
            videoCamera.gameObject.SetActive(false);
        }
        #endregion
        #endregion
    }

}
