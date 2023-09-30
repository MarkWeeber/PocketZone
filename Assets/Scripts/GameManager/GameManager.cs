using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PocketZone.Space
{
    public class GameManager : MonoBehaviour
    {
        private static GameManager instance;
        public static GameManager Instance { get => instance; }
        public ProgressData ProgressData;
        private GameState gameState;

        public event Action OnLevelComplete;
        public event Action<GameState> OnGameStateChanged;

        private void Awake()
        {
            instance = this;
            RefreshProgressData();
            DontDestroyOnLoad(gameObject);
            OnLevelComplete += HandleOnLevelComplete;
            
        }

        private void OnDestroy()
        {
            OnLevelComplete -= HandleOnLevelComplete;
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
            int nextLevel = (currentSceneIndex == SceneManager.sceneCountInBuildSettings) ? currentSceneIndex : currentSceneIndex + 1;
            ProgressData = new ProgressData(SceneManager.sceneCountInBuildSettings, nextLevel);
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
                    break;
                case GameState.LevelComplete:
                    OnLevelComplete?.Invoke();
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