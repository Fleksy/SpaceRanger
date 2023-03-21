using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BulletScript : MonoBehaviour
{
    [SerializeField] private float speed = 1f;

    public float damage;
    [SerializeField] private PlayerScript player;
    
   
    void Start()
    {
        player = transform.parent.parent.GetComponent<PlayerScript>();
        damage = player.damage;
    }

    
    /// <summary>
    /// Пуля уничтожается автоматически, если по какой-то причине покидает радиус стрельбы не попав в противника
    /// Такое возможно, если при высокой скорости стрельбы пуля была выпущена, когда предыдущая уже почти убила врага
    /// </summary>
    void Update()
    {
        if (Vector2.Distance(transform.position, player.transform.position) < player.shootingDistance)
        {
            transform.Translate(0,speed*Time.deltaTime,0);
        }
        else
        {
            DestroyImmediate(gameObject);
        }
        
    }
}
