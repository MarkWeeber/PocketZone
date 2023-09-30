using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PocketZone.Space
{
    [System.Serializable]
    public class ProgressData
    {
        public bool[] levels;

        public ProgressData(int maxNumberOfScenes, int currentSceneIndex)
        {
            levels = new bool[maxNumberOfScenes];
            for (int i = 0; i < maxNumberOfScenes; i++)
            {
                if (i <= currentSceneIndex)
                {
                    levels[i] = true;
                }
                else
                {
                    levels[i] = false;
                }
            }
        }

        public ProgressData(ProgressData progressData)
        {
            this.levels = progressData.levels;
        }
    }
}