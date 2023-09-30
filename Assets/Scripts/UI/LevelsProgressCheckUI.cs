using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PocketZone.Space
{
    public class LevelsProgressCheckUI : MonoBehaviour
    {
        public GameObject[] levelButtons;

        private void Start()
        {
            CheckProgress();
        }

        private void CheckProgress()
        {
            ProgressData progressData = GameManager.Instance.ProgressData;
            for (int i = 1; i < progressData.levels.Length; i++)
            {
                levelButtons[i - 1].SetActive(progressData.levels[i]); // asume 0 is main menu
            }
            levelButtons[0].SetActive(true);
        }
    }
}