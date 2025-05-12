using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageEnemy : MonoBehaviour
{
    [SerializeField] private int damageAmount = 1;
    public GameObject player;
      private void OnTriggerEnter2D(Collider2D other){
        if(other.gameObject.GetComponent<EnemyHealth>()){
            EnemyHealth enemyHealth = other.gameObject.GetComponent<EnemyHealth>();
            enemyHealth.TakeDamage(damageAmount + player.GetComponent<PlayerStats>().attack_damage);
        }
    }
}
