using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PocketZone.Space
{
    public class LevelCompleteChecker : MonoBehaviour
    {
        [SerializeField] private TriggerHandler triggerHandler;

        private List<EnemyHealth> enemyHealths;

        private void Start()
        {
            triggerHandler.OnTriggerEnter += HandleTriggerEnter;
            enemyHealths = FindObjectsOfType<EnemyHealth>().ToList();
            foreach (EnemyHealth _enemyHealth in enemyHealths)
            {
                _enemyHealth.OnEnemyDeath += RemoveDeadEnemyFromList;
            }
        }

        private void OnDestroy()
        {
            triggerHandler.OnTriggerEnter -= HandleTriggerEnter;
        }

        private void RemoveDeadEnemyFromList(EnemyHealth enemyHealth)
        {
            enemyHealths.Remove(enemyHealth);
        }

        private void HandleTriggerEnter(Collider2D collider2D)
        {
            if (!enemyHealths.Any())
            {
                InfoUI.Instance.SendInformation(InfoUI.LEVEL_COMPLETE, MessageType.SUCCESS);
                GameManager.Instance.SetGameState(GameState.LevelComplete);
            }
            else
            {
                InfoUI.Instance.SendInformation(InfoUI.NOT_ALL_ENEMIES_SLAIN, MessageType.WARNING);
            }
        }
    }
}