using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace PocketZone.Space
{
    public class GameManager : MonoBehaviour
    {
        private static GameManager instance;
        public static GameManager Instance { get => instance; }
        public ProgressData ProgressData;
        private GameState gameState;

        public UnityEvent OnDeathEventUI;
        public UnityEvent OnLevelCompleteEventUI;

        public event Action<GameState> OnGameStateChanged;

        private void Awake()
        {
            instance = this;
            RefreshProgressData();
        }

        private void RefreshProgressData()
        {
            ProgressData = SaveSystem.LoadProgressData();
            if (ProgressData == null)
            {
                ProgressData = new ProgressData(SceneManager.sceneCountInBuildSettings, SceneManager.GetActiveScene().buildIndex);
                SaveSystem.SaveProgress(ProgressData);
                InfoUI.Instance.SendInformation(InfoUI.LOAD_SUCCESS_NEW, MessageType.SUCCESS);
            }
            else
            {
                if (ProgressData.levels.Length != SceneManager.sceneCountInBuildSettings)
                {
                    ProgressData = new ProgressData(SceneManager.sceneCountInBuildSettings, SceneManager.GetActiveScene().buildIndex);
                    SaveSystem.SaveProgress(ProgressData);
                    InfoUI.Instance.SendInformation(InfoUI.LOAD_SUCCESS_NEW, MessageType.SUCCESS);
                }
                else
                {
                    InfoUI.Instance.SendInformation(InfoUI.LOAD_SUCCESS_PERSISTS, MessageType.SUCCESS);
                }
            }
        }

        private void HandleOnLevelComplete()
        {
            int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            int nextLevel = (currentSceneIndex == SceneManager.sceneCountInBuildSettings - 1) ? currentSceneIndex : currentSceneIndex + 1;
            ProgressData.levels[nextLevel] = true;// = new ProgressData(SceneManager.sceneCountInBuildSettings, nextLevel);
            ProgressData.InventorySaveData[nextLevel] = LevelUIManager.Instance.InventoryUI.InventorySaveData;
            SaveSystem.SaveProgress(ProgressData);
        }

        public void SetGameState(GameState state)
        {
            gameState = state;
            switch (gameState)
            {
                case GameState.InGame:
                    ResumeTime();
                    break;
                case GameState.PasuedInGame:
                    PauseTime();
                    break;
                case GameState.PlayerDeadInGame:
                    OnDeathEventUI?.Invoke();
                    break;
                case GameState.LevelComplete:
                    HandleOnLevelComplete();
                    OnLevelCompleteEventUI?.Invoke();
                    break;
                default:
                    break;
            }
            OnGameStateChanged?.Invoke(state);
        }

        private void PauseTime()
        {
            Time.timeScale = 0f;
        }

        private void ResumeTime()
        {
            Time.timeScale = 1f;
        }

        #region Public Methods
        public void LoadLevel(int levelIndex)
        {
            SceneManager.LoadScene(levelIndex);
        }

        public void QuitGame()
        {
            Application.Quit();
        }

        public void PauseGame()
        {
            SetGameState(GameState.PasuedInGame);
        }

        public void ResumeGame()
        {
            SetGameState(GameState.InGame);
        }

        public void GoToMainMenu()
        {
            SetGameState(GameState.InGame);
            SceneManager.LoadScene(0);
        }

        public void RestartLevel()
        {
            SetGameState(GameState.InGame);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        public void SaveCurrentLevelProgress(int sceneIndex)
        {
            ProgressData.levels[sceneIndex] = true;
            SaveSystem.SaveProgress(ProgressData);
        }

        public void TryGoToNextLevel()
        {
            int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            int lastSceneIndex = SceneManager.sceneCountInBuildSettings - 1;
            if (currentSceneIndex == lastSceneIndex)
            {
                GoToMainMenu();
            }
            else
            {
                LoadLevel(currentSceneIndex + 1);
            }
        }
        #endregion
    }

    public enum GameState
    {
        InGame = 0,
        PasuedInGame = 1,
        PlayerDeadInGame = 2,
        LevelComplete = 3
    }
}