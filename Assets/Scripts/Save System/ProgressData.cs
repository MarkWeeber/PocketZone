using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PocketZone.Space
{
    [System.Serializable]
    public class ProgressData
    {
        public bool[] levels;
        public InventorySaveData[] InventorySaveData;
        public ProgressData(int maxNumberOfScenes, int currentSceneIndex)
        {
            levels = new bool[maxNumberOfScenes];
            InventorySaveData = new InventorySaveData[maxNumberOfScenes];
            for (int i = 0; i < maxNumberOfScenes; i++)
            {
                InventorySaveData[i] = new InventorySaveData(InventoryUI.maxSlots);
            }
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
            this.InventorySaveData = progressData.InventorySaveData;
        }

        public override string ToString()
        {
            string result = "";
            result += "{" + "@";
            result += "&slevels:@";
            result += "&s&s[@";
            for (int i = 0; i < levels.Length; i++)
            {
                result += $"&s&s&s{i} : {levels[i]}@";
                result += "&s&s&s&s[";
                result += $"&s&s&s&s&sItems:@";
                for (int j = 0; j < InventorySaveData[i].Dimension; j++)
                {
                    result += $"&s&s&s&s&s&s {InventorySaveData[i].IdAndQuantity[j, 0]} : {InventorySaveData[i].IdAndQuantity[j, 1]}";
                }
                result += "&s&s&s&s]@";
            }
            result += "&s&s]@";
            result += "}";
            result = result.Replace("&s", " ");
            result = result.Replace("@", System.Environment.NewLine);
            return result;
        }
    }

    [System.Serializable]
    public struct InventorySaveData
    {
        public int Dimension;
        public int[,] IdAndQuantity;
        public InventorySaveData(int dimension)
        {
            Dimension = dimension;
            IdAndQuantity = new int[dimension, 2];
            for (int i = 0; i < dimension; i++)
            {
                IdAndQuantity[i, 0] = 0;
                IdAndQuantity[i, 1] = 0;
            }
        }
    }

}