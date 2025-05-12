using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private int startingHealth = 3;
    [SerializeField] private int coinReward = 1;
    public int CurrentHealth => currentHealth; 
    // Start is called before the first frame update
    private int currentHealth;
    private Knockback knockback;
    private Flash flash;
       private void Awake()
    {
        currentHealth = startingHealth;
        knockback = GetComponent<Knockback>();
        flash = GetComponent<Flash>();
    }

    public void TakeDamage(int damage){
        currentHealth -= damage;
        knockback.GetKnockedBack(PlayerMovement.Instance.transform,8f);
        Debug.Log($"Enemy took {damage}, now at {currentHealth} HP.");
        StartCoroutine(flash.FlashRoutine());
    }
    public void DetectDeath(){
        if (currentHealth <= 0){
            Destroy(gameObject);
            Coins.Instance.AddCoins(coinReward);
            print("You defeated an enemy");
        }
    }



    // Update is called once per frame
    void Update()
    {
        
    }
}
