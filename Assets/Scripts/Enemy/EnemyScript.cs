
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class EnemyScript : MonoBehaviour
{
    public Transform target;


    [SerializeField] private float rotationSpeed = 3.5f;
    private Quaternion rotationZ;
    private Vector3 difference;

    public static UnityEvent<EnemyScript> OnEnemyDied = new UnityEvent<EnemyScript>();

    [SerializeField] private float speed = 5f;
    [SerializeField] private float hp = 3;

    private AudioSource _audioSource;

    [SerializeField] int dangerLevel;

    [HideInInspector] public int moneyForWinning;

    
    void Start()
    {
        moneyForWinning =  10*dangerLevel + Random.Range(0,6+dangerLevel);
        _audioSource = transform.GetComponent<AudioSource>();
    }

  
    void Update()
    {
        transform.Translate(0, speed * Time.deltaTime, 0);
    }
    
    private void OnTriggerStay2D(Collider2D col)
    {
        if (col.gameObject.tag == "Bullet")
        {
            hp -= col.transform.GetComponent<BulletScript>().damage;
            Destroy(col.gameObject);
            _audioSource.Play();
            if (hp <= 0)
            {
                OnEnemyDied.Invoke(gameObject.GetComponent<EnemyScript>());
                Destroy(gameObject);
            }
        }
    }
    
}