using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class TotalScoreScript : MonoBehaviour
{
    private int killCount;
    private int money;

    [SerializeField] private TextMeshProUGUI killCountText;

    [SerializeField] private TextMeshProUGUI moneyText;

    [SerializeField] private TextMeshProUGUI killCountTextInGame;
    // Start is called before the first frame update
    void Start()
    {
        EnemyScript.OnEnemyDied.AddListener(AddEnemyScore);
    }
    /// <summary>
    ///  Когда противник умирает, обновляется итоговый счет убитых врагов
    /// </summary>
    /// <param name="deadEnemy"></param>
    void AddEnemyScore(EnemyScript deadEnemy)
    {
        killCount++;
        killCountTextInGame.text = "Врагов уничтожено: " + killCount;
        money += deadEnemy.moneyForWinning;
    }

    /// <summary>
    ///  Когда игрок проигрывает, показывается табличка с общей суммой собранных денег и убитых врагов.
    /// </summary>
    public void ShowTotalScore()
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(true);
        }

        moneyText.text = money.ToString();
        killCountText.text = killCount.ToString();
    }

    public void Exit()
    {
        SceneManager.LoadScene("StartScene");
    }
}