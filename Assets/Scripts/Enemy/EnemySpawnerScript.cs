using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class EnemySpawnerScript : MonoBehaviour
{
    [SerializeField] private GameObject[] enemiesTypeList;
    public int spawnDistance = 7;
    [SerializeField] private Transform player;

    private Vector3 enemyPosition;
    private Quaternion enemyRotation;

    private int waveNumber;
    [SerializeField] private TextMeshProUGUI waveNumberText;
    [SerializeField] private float initialWaitTime = 5f;
    [SerializeField] private float accelerationSpawn = 1.2f;

    [SerializeField] private Image progressBar;

    void Start()
    {
        StartCoroutine(nameof(SpawnEnemies));
    }
    /// <summary>
    /// Подсчет волн, создание врагов соотвествующего количества и опасности. Постепенно скорость создания врагов увеличивается.
    /// </summary>
    /// <returns></returns>
    private IEnumerator SpawnEnemies()
    {
        while (!PlayerScript.endGame)
        {
            waveNumber++;
            waveNumberText.text = "Волна: " + waveNumber;
            for (float i = 0; i < 2 + waveNumber; i++)
            {
                progressBar.fillAmount = (i / (2 + waveNumber));
                SpawnEnemy(enemiesTypeList[ChooseEnemyType()]);
                yield return new WaitForSeconds((float)((initialWaitTime + 1) -
                                                        Math.Pow(accelerationSpawn,
                                                            waveNumber > 10 ? 9 : waveNumber - 1)));
            }
        }
    }

    /// <summary>
    /// Расчет точки создания врага и создание его там
    /// </summary>
    /// <param name="enemyPrefab"></param>
    private void SpawnEnemy(GameObject enemyPrefab)
    {
        int angel = Random.Range(0, 361);
        enemyPosition = new Vector2((float)Math.Cos(angel), (float)Math.Sin(angel)) * spawnDistance;
        enemyRotation = Quaternion.LookRotation(Vector3.forward, player.position - enemyPosition);
        Instantiate(enemyPrefab, enemyPosition, enemyRotation, transform);
    }

    /// <summary>
    ///  C каждой волной увеличивается шанс спавна более опасных врагов
    /// </summary>
    /// <returns></returns>
    private int ChooseEnemyType()
    {
        int rnd = Random.Range(0, 1 + (waveNumber > 10 ? 100 : 10 * waveNumber));
        if (rnd > 70)
        {
            return 2;
        }
        else if (rnd > 50)
        {
            return 1;
        }
        else
        {
            return 0;
        }
    }
}