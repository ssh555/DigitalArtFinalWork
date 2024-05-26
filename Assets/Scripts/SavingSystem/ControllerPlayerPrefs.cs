using UnityEngine;
using Common;
using UnityEditor;
using System.Numerics;

namespace SavingSystem
{
    public class ControllerPlayerPrefs : ISave
    {
        public int ReadMaxLevel()
        {
            return PlayerPrefs.GetInt("MaxLevel", 0);
        }

        public bool ReadStarTypeForLevel(int level, StarTypes type)
        {
            
            return PlayerPrefs.GetInt("Level" + level + "Star" + type, 0) == 1;
        }
        public void SaveMaxLevel(int level)
        {
            if (ReadMaxLevel() < level)
            {
                PlayerPrefs.SetInt("MaxLevel", level);
            }
        }
        public void SaveStarTypeForLevel(int level, StarTypes type, bool completed)
        {
            int complete = 0;
            if (completed)
            {
                complete = 1;
            }
            
            PlayerPrefs.SetInt("Level" + level + "Star" + type, complete);
        }

        public static void UnlockAllLevels()
        {
            PlayerPrefs.SetInt("MaxLevel", 100);
        }

        public static void ResetLevels()
        {
            PlayerPrefs.SetInt("MaxLevel", 0);
        }
    }
}