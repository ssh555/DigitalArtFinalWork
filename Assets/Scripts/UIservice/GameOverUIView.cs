using System.IO;
using UnityEngine;
using UnityEngine.Video;
using Video;

namespace UIservice
{
    public class GameOverUIView : MonoBehaviour, IUIView
    {
        public void DestroyUI()
        {
            gameObject.SetActive(false);
            // ����ս��CG
            //PlayFailCG();
        }

        public void DisplayUI()
        {
            gameObject.SetActive(true);
        }

        public void DisplayFailCG()
        {
            Invoke("PlayFailCG", 1f);
        }

        private void PlayFailCG()
        {
            // ��ȡ Videos �ļ��������� .mp4 �ļ���·��
            string[] mp4Files = Directory.GetFiles(Application.streamingAssetsPath + "/Videos/Failure", "*.mp4");
            // ����Ƿ���� .mp4 �ļ�
            if (mp4Files.Length > 0)
            {
                // ���ѡ��һ�� .mp4 �ļ���·��
                string randomMp4FilePath = GetRandomFilePath(mp4Files);

                // ��ʧ��CG�����ѡȡһ������
                VideoManager.Instance.PlayByAbs(randomMp4FilePath,
                    onstart: (videoplayer) =>
                    {
                        VideoManager.Instance.IsShowOnEnd = true;
                        Time.timeScale = 0;
                    },
                    onend: (videoplayer) =>
                    {
                        VideoManager.Instance.IsShowOnEnd = true;
                        Time.timeScale = 1;
                    }
                );
            }
            // ���ļ�·�����������ѡ��һ���ļ�·��
            string GetRandomFilePath(string[] filePaths)
            {
                int randomIndex = Random.Range(0, filePaths.Length);
                return filePaths[randomIndex];
            }
        }
    }
}