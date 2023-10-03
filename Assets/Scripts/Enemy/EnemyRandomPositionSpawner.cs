using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.iOS;

namespace PocketZone.Space
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class EnemyRandomPositionSpawner : MonoBehaviour
    {
        [SerializeField] private List<GameObject> enemyPrefabs;
        [SerializeField] private int spawnQuantity = 3;

        private int maxTries = 10;
        private BoxCollider2D boxCollider2D;
        private GameObject prefabToSpawn;
        private Collider2D[] overlapColliders = new Collider2D[50];
        private Vector2 spawnPosition;

        private void Start()
        {
            boxCollider2D = GetComponent<BoxCollider2D>();
            HandleRandomSpawn();
        }

        private void HandleRandomSpawn()
        {
            if (enemyPrefabs.Count > 0)
            {
                for (int i = 0; i < spawnQuantity; i++)
                {
                    int _spawnIndex = 0;
                    _spawnIndex = Random.Range(0, enemyPrefabs.Count);
                    prefabToSpawn = enemyPrefabs[_spawnIndex];
                    float _radius = 2f;
                    if (prefabToSpawn.TryGetComponent<CapsuleCollider2D>(out CapsuleCollider2D _capsuleCollider2D))
                    {
                        _radius = Mathf.Max(_capsuleCollider2D.size.x, _capsuleCollider2D.size.y);
                    }
                    spawnPosition = GetRandomPosition(_radius);
                    Instantiate(prefabToSpawn, spawnPosition, Quaternion.identity);
                }
            }
        }

        private Vector2 GetRandomPosition(float radius)
        {
            int _tries = maxTries;
            float _x = 0f;
            float _y = 0f;
            int _collisions = 0;

            Vector2 _spawnPoint = transform.position;
            Vector2 _colliderCenterWithOffset = boxCollider2D.offset + (Vector2)transform.position;
            while (_tries > 0)
            {
                _x = Random.Range(_colliderCenterWithOffset.x - boxCollider2D.size.x / 2f, _colliderCenterWithOffset.x + boxCollider2D.size.x / 2f);
                _y = Random.Range(_colliderCenterWithOffset.y - boxCollider2D.size.y / 2f, _colliderCenterWithOffset.y + boxCollider2D.size.y / 2f);
                _spawnPoint = new Vector2(_x, _y);
                _collisions = Physics2D.OverlapCircleNonAlloc(_spawnPoint, radius, overlapColliders);
                if(_collisions == 0)
                {
                    return _spawnPoint;
                }
                _tries--;
            }
            return _spawnPoint;
        }
    }
}