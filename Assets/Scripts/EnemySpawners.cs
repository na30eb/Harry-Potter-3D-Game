using UnityEngine;

public class EnemySpawner : MonoBehaviour {
    public GameObject enemyPrefab;
    public int numberOfEnemies = 5;
    public float spawnRadius = 10f;

    void Start() {
        SpawnEnemiesRandomly(); 
    }

    void SpawnEnemiesRandomly() {
        for (int i = 0; i < numberOfEnemies; i++) {
            Vector3 randomPosition = GetRandomPosition();
            Instantiate(enemyPrefab, randomPosition, Quaternion.identity);
        }
    }

    Vector3 GetRandomPosition() {
        Vector2 randomCircle = Random.insideUnitCircle * spawnRadius;
        Vector3 randomPosition = new Vector3(randomCircle.x, 0f, randomCircle.y) + transform.position;
        return randomPosition;
    }
}
