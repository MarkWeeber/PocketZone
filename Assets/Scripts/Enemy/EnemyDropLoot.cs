using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace PocketZone.Space
{
    public class EnemyDropLoot : MonoBehaviour
    {
        [SerializeField] private EnemyHealth health;
        [SerializeField] private float dropRadius = 0.5f;
        [SerializeField] private GameObject lootPrefab1;
        [SerializeField] private int minDrop1 = 0;
        [SerializeField] private int maxDrop1 = 2;
        [SerializeField] private GameObject lootPrefab2;
        [SerializeField] private int minDrop2 = 0;
        [SerializeField] private int maxDrop2 = 2;
        [SerializeField] private GameObject lootPrefab3;
        [SerializeField] private int minDrop3 = 0;
        [SerializeField] private int maxDrop3 = 2;

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
            int firstLootQuantity = Random.Range(minDrop1, maxDrop1 + 1);
            int secondLootQuantity = Random.Range(minDrop2, maxDrop2 + 1);
            int thirdLootQuantity = Random.Range(minDrop3, maxDrop3 + 1);
            if (lootPrefab1 != null)
            {
                for (int i = 0; i < firstLootQuantity; i++)
                {
                    spawnedPrefab = Instantiate(lootPrefab1, transform);
                    spawnedPrefab.SetActive(false);
                    lootList.Add(spawnedPrefab);
                }
            }
            if (lootPrefab2 != null)
            {
                for (int i = 0; i < secondLootQuantity; i++)
                {
                    spawnedPrefab = Instantiate(lootPrefab2, transform);
                    spawnedPrefab.SetActive(false);
                    lootList.Add(spawnedPrefab);
                }
            }
            if (lootPrefab3 != null)
            {
                for (int i = 0; i < thirdLootQuantity; i++)
                {
                    spawnedPrefab = Instantiate(lootPrefab3, transform);
                    spawnedPrefab.SetActive(false);
                    lootList.Add(spawnedPrefab);
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
}