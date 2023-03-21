using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using TMPro;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ImprovementsScript : MonoBehaviour
{
    [SerializeField] private PlayerScript player;

    private int money = 0;
    [SerializeField] private TextMeshProUGUI moneyСounter;

    [SerializeField] private GameObject messagePrefab;
    private MessageScript moneyMessage;
    [SerializeField] private Transform messageList;

    [SerializeField] private Button damageButton;
    [SerializeField] private Button shootingSpeedButton;
    [SerializeField] private Button shootingDistanceButton;

    [SerializeField] public int[] damageImproveCost = { 100, 200, 300 };
    private byte damageImproveStage = 0;
    [SerializeField] private int[] speedImproveCost = { 100, 200, 300 };
    private byte speedImproveStage = 0;
    [SerializeField] private int[] distanceImproveCost = { 100, 200, 300 };
    private byte distanceImproveStage = 0;

    [SerializeField] private float damageBonus = 1;
    [SerializeField] private float speedBonus = 1;
    [SerializeField] private float distanceBonus = 0.5f;

    [SerializeField] private Text damageCostText;
    [SerializeField] private Text shootingSpeedCostText;
    [SerializeField] private Text shootingDistanceCostText;

    private enum Characteristic
    {
        Damage,
        ShootingSpeed,
        ShootingDistance,
    }


    void Start()
    {
        EnemyScript.OnEnemyDied.AddListener(AddMoney);
        // Т.к. на событие нажатия кнопки нельзя подписать метод с двумя аргументами через инспектор, пришлось это делать через код
        damageButton.onClick.AddListener(() => ImproveCharacteristic(Characteristic.Damage));
        shootingSpeedButton.GetComponent<Button>().onClick
            .AddListener(() => ImproveCharacteristic(Characteristic.ShootingSpeed));
        shootingDistanceButton.GetComponent<Button>().onClick
            .AddListener(() => ImproveCharacteristic(Characteristic.ShootingDistance));
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Keypad1))
        {
            damageButton.onClick.Invoke();
        }
        else if (Input.GetKey(KeyCode.Keypad2))
        {
            shootingSpeedButton.onClick.Invoke();
        }
        else if (Input.GetKey(KeyCode.Keypad3))
        {
            shootingDistanceButton.onClick.Invoke();
        }
    }

    /// <summary>
    /// В случае смерти врага игроку начисляются деньги,а на месте смерти всплывает сообщение с полученной суммой
    /// </summary>
    /// <param name="deadEnemy"></param>
    private void AddMoney(EnemyScript deadEnemy)
    {
        money += deadEnemy.moneyForWinning;

        moneyСounter.text = "Money : " + money;
        moneyMessage = Instantiate(messagePrefab, deadEnemy.transform.position, Quaternion.identity, messageList)
            .GetComponent<MessageScript>();
        moneyMessage.GetComponent<TextMeshPro>().text = "$ " + deadEnemy.moneyForWinning;
    }

// Тут происходит дублирование кода, но это было сделано на случай если улучшения будут тригерить еще какие-то
// уникальные эффекты, кроме изменения цифр (собственно это и происходит во втором блоке)

    /// <summary>
    /// Проверка на возможность улучшения, улучшение соответсвующей характеристики, списывание денег, изменение стоимости улучшения
    /// </summary>
    /// <param name="characteristic"></param>
    private void ImproveCharacteristic(Characteristic characteristic)
    {
        switch (characteristic)
        {
            case Characteristic.Damage:
            {
                if (damageImproveStage < damageImproveCost.Length && money >= damageImproveCost[damageImproveStage])
                {
                    player.damage += damageBonus;
                    money -= damageImproveCost[damageImproveStage];
                    moneyСounter.text = "Money : " + money;
                    damageImproveStage++;
                    damageCostText.text =
                        damageImproveStage == damageImproveCost.Length
                            ? ""
                            : "Цена " + damageImproveCost[damageImproveStage];
                }
            }
                break;
            case Characteristic.ShootingDistance:
            {
                if (distanceImproveStage < distanceImproveCost.Length &&
                    money >= distanceImproveCost[distanceImproveStage])
                {
                    player.shootingDistance += distanceBonus;
                    player.shootingArea.localScale =
                        new Vector3(2 * player.shootingDistance, 2 * player.shootingDistance, 1);

                    money -= distanceImproveCost[distanceImproveStage];
                    moneyСounter.text = "Money : " + money;
                    distanceImproveStage++;
                    shootingDistanceCostText.text = distanceImproveStage == distanceImproveCost.Length
                        ? ""
                        : "Цена " + distanceImproveCost[distanceImproveStage];
                }
            }
                break;
            case Characteristic.ShootingSpeed:
            {
                if ((speedImproveStage <= speedImproveCost.Length) && (money >= speedImproveCost[speedImproveStage]))
                {
                    player.shootingSpeed += speedBonus;
                    money -= speedImproveCost[speedImproveStage];
                    moneyСounter.text = "Money : " + money;
                    speedImproveStage++;
                    shootingSpeedCostText.text =
                        speedImproveStage == speedImproveCost.Length
                            ? ""
                            : "Цена " + speedImproveCost[speedImproveStage];
                }
            }
                break;
        }
    }
}