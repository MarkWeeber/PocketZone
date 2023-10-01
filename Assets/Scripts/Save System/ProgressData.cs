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

        public override string ToString()
        {
            string result = "";
            result += "{" + "@";
            result += "levels:@";
            for (int i = 0; i < levels.Length; i++)
            {
                result += $"{i} : {levels[i].ToString()}@";
            }
            result += "}";
            result = result.Replace("@", System.Environment.NewLine);
            return result;
        }
    }
}