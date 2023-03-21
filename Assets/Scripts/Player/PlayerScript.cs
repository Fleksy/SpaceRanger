using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;


public class PlayerScript : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform bulletCreationPoint;

    [SerializeField] private Transform enemiesList;

    [SerializeField] public Transform shootingArea;
    private Transform target;
    private bool isHaveTarget = false;

    [SerializeField] private float rotationSpeed = 3.5f;
    private Quaternion rotationZ;

    public float damage = 1;
    public float shootingSpeed = 3;
    public float shootingDistance = 2.5f;
    private float initialShootingSpeed;
    public TotalScoreScript totalScorePanel;
    private AudioSource _audioSource;


    [HideInInspector] public static bool endGame = false;

    void Start()
    {
        endGame = false;
        _audioSource = transform.GetComponent<AudioSource>();
        initialShootingSpeed = shootingSpeed;
        StartCoroutine(nameof(Shooting));
    }

    private void Update()
    {
        RotateToTarget();
        if (Input.GetKey(KeyCode.Escape))
        {
            SceneManager.LoadScene("StartScene");
        }
    }
    
    private IEnumerator Shooting()
    {
        while (!endGame)
        {
            if (isHaveTarget)
            {
                Shot();
            }

            yield return new WaitForSeconds(initialShootingSpeed/ shootingSpeed);
        }
    }

    void Shot()
    {
        if (CheckTrajectory())
        {
            Instantiate(bulletPrefab, bulletCreationPoint.position, bulletCreationPoint.rotation, bulletCreationPoint);
            _audioSource.Play();
        }
    }

    
    /// <summary>
    /// Если цель обнаружена, игрок разворачивается к ней для стрельбы
    /// </summary>
    void RotateToTarget()
    {
        target = ChosenTarget();
        if (isHaveTarget)
        {
            rotationZ = Quaternion.LookRotation(Vector3.forward, target.position - transform.position);
            transform.rotation = Quaternion.Lerp(transform.rotation, rotationZ, rotationSpeed * Time.deltaTime);
        }
    }
    
    /// <summary>
    /// Выбирается ближайший противник, находящийся в радиусе стрельбы
    /// </summary>
    /// <returns></returns>
    Transform ChosenTarget()
    {
        if (enemiesList.transform.childCount > 0)
        {
            Transform chosenTarget = null;
            isHaveTarget = false;
            float minimumDistance = shootingDistance;
            foreach (Transform enemy in enemiesList.transform)
            {
                float distanceToEnemy =
                    Vector2.Distance(transform.position, enemy.position);
                if (distanceToEnemy <= minimumDistance)
                {
                    chosenTarget = enemy;
                    isHaveTarget = true;
                    minimumDistance = distanceToEnemy;
                }
            }
            return chosenTarget;
        }
        else
        {
            isHaveTarget = false;
            return null;
        }
    }
    
    /// <summary>
    /// Проверка, находятся ли выбранный противник на траектории стрельбы игрока
    /// </summary>
    /// <returns></returns>
    private bool CheckTrajectory()
    {
        return ((Math.Abs(Math.Abs(transform.rotation.eulerAngles.z % 90) -
                          Math.Abs(target.transform.rotation.eulerAngles.z % 90)) < 5));
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Enemy"))
        {
            EndGame();
        }
    }

    void EndGame()
    {
        endGame = true;
        StopCoroutine(nameof(Shooting));
        totalScorePanel.ShowTotalScore();
    }
}