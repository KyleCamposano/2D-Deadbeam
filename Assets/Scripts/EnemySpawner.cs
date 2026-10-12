
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class EnemySpawner : MonoBehaviour
{
    [Header("Enemy Settings")]
    public GameObject enemyPrefab;
    public Transform player;
    public Light2D flashlight;

    [Header("Spawn Locations")]
    public Transform[] spawnPoints;

    [Header("Difficulty")]
    public int enemiesPerGenerator = 1;

    public void SpawnEnemies()
    {
        // ensure enemy prefab and spawn locs are assigned
        if (enemyPrefab == null || spawnPoints.Length == 0)
            return;

        // spawn addtl enemies whenever a generator is activated
        for (int i = 0; i < enemiesPerGenerator; i++)
        {
            // randomly select spawn loc
            Transform spawnPoint =
                spawnPoints[Random.Range(0, spawnPoints.Length)];

            // create new shadow creature at selected loc
            GameObject enemy = Instantiate(
                enemyPrefab,
                spawnPoint.position,
                Quaternion.identity
            );

            // get newly spawned creature's script
            ShadowCreature creature =
                enemy.GetComponent<ShadowCreature>();

            // assign player and flashlight ref
            if (creature != null)
            {
                creature.player = player;
                creature.flashlight = flashlight;
            }
        }
    }
}
