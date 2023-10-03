using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace PocketZone.Space
{
    public class EnemyDropLoot : MonoBehaviour
    {
        [SerializeField] private EnemyHealth health;
        [SerializeField] private List<DropLootStatistics> DropLootStatistics;
        [SerializeField] private float dropRadius = 0.5f;

        private List<GameObject> lootList;

        private void Awake()
        {
            lootList = new List<GameObject>();
            calculateLootDropItems();
            health.OnDeath += OnDeath;
        }

        private void OnDestroy()
        {
            health.OnDeath -= OnDeath;
        }

        private void calculateLootDropItems()
        {
            GameObject spawnedPrefab;
            int probability = 0;
            int spawnQuantity = 0;
            foreach (DropLootStatistics _drop in DropLootStatistics)
            {
                probability = Random.Range(-50, 51);
                float lowRange = (_drop.probabilityPercent - 50) / 2f;
                float maxRange = (50 - _drop.probabilityPercent) / 2f;
                if (probability <= lowRange || probability >= maxRange)
                {
                    spawnQuantity = Random.Range(_drop.minDropQuantity, _drop.maxDropQuantity + 1);
                    for (int i = spawnQuantity; i > 0; i--)
                    {
                        spawnedPrefab = Instantiate(_drop.prefab, transform);
                        spawnedPrefab.SetActive(false);
                        lootList.Add(spawnedPrefab);
                    }
                }
            }
        }

        private void OnDeath()
        {
            foreach (GameObject lootDrop in lootList)
            {
                lootDrop.transform.parent = null;
                lootDrop.SetActive(true);
                lootDrop.transform.position = (Vector2)transform.position + Random.insideUnitCircle.normalized * dropRadius;
            }
        }
    }

    [System.Serializable]
    public struct DropLootStatistics
    {
        public int probabilityPercent;
        public int minDropQuantity;
        public int maxDropQuantity;
        public GameObject prefab;
    }
}