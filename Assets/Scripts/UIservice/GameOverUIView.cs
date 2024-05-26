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
            // 播放战败CG
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
            // 获取 Videos 文件夹中所有 .mp4 文件的路径
            string[] mp4Files = Directory.GetFiles(Application.streamingAssetsPath + "/Videos/Failure", "*.mp4");
            // 检查是否存在 .mp4 文件
            if (mp4Files.Length > 0)
            {
                // 随机选择一个 .mp4 文件的路径
                string randomMp4FilePath = GetRandomFilePath(mp4Files);

                // 从失败CG中随机选取一个播放
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
            // 从文件路径数组中随机选择一个文件路径
            string GetRandomFilePath(string[] filePaths)
            {
                int randomIndex = Random.Range(0, filePaths.Length);
                return filePaths[randomIndex];
            }
        }
    }
}