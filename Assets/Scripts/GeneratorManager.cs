
using UnityEngine;
using TMPro;

public class GeneratorManager : MonoBehaviour
{
    public int generatorsActivated = 0;
    public TextMeshProUGUI generatorText;
    public EnemySpawner enemySpawner;

    private void Start()
    {
        // init generator progress display
        UpdateUI();
    }

    public void GeneratorActivated()
    {
        // increase number of activated generators
        generatorsActivated++;

        // update generator progress displayed on screen
        UpdateUI();

        // spawn addtl enemies to increase difficulty
        if (enemySpawner != null)
        {
            enemySpawner.SpawnEnemies();
        }

        // log curr generator progress (for debugging)
        Debug.Log(
            "Generators: " + generatorsActivated + " / 3"
        );
    }

    private void UpdateUI()
    {
        // display # of activated generators out of three
        if (generatorText != null)
        {
            generatorText.text =
                "Generators: " + generatorsActivated + " / 3";
        }
    }
}
