using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerLives : MonoBehaviour
{
    public int lives = 6;
    public Image[] livesUI;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            lives--;
            Debug.Log($"Life lost! Remaining: {lives}");

            // destroy the enemy bullet/enemy
            Destroy(collision.gameObject);

            if (lives <= 0)
            {
                Debug.Log("Game Over");
                Destroy(gameObject);  // kill the player
            }
        }
    }
    //reference for when we add the ui elements and such
//     void OnTriggerEnter2D(Collider2D collision){
//     if (collision.CompareTag("Enemy"))
//     {
//          Debug.Log("Life lost due to collision with: " + collision.gameObject.name);
//         lives--;
//         print("num of" + lives);
//         //for (int i = 0; i < livesUI.Length; i++)
//             //livesUI[i].enabled = (i < lives);

//         if (lives <= 0)
//         {
//             // Game over: play the big explosion sound only.
//             //AudioSource.PlayClipAtPoint(explosionAudio, collision.transform.position, 1.5f);
//             //Instantiate(explosionPrefab, collision.transform.position, Quaternion.identity);
//             Destroy(gameObject);            
//             // Game Over here
//             Destroy(collision.gameObject);
//             Destroy(gameObject);
//             //MyScript.GameOver(); 
//             //gameScript.die();
//             print("Game Over");
//         }
//         else
//         {
//             // Still alive: play the small explosion sound.
//             //Instantiate(smallExplosionPrefab, collision.transform.position, Quaternion.identity);
//             //AudioSource.PlayClipAtPoint(smallExplosionAudio, collision.transform.position, 1.5f);
//             Destroy(collision.gameObject);
//         }
//     }
//     else if (collision.CompareTag("Boss"))
//     {
//         Debug.Log("Life lost due to collision with Boss");
//         //Instantiate(smallExplosionPrefab, collision.transform.position, Quaternion.identity);

//         //lives--;
//         //for (int i = 0; i < livesUI.Length; i++)
//             //livesUI[i].enabled = (i < lives);

//         //if (lives <= 0)
//         {
//             //Instantiate(explosionPrefab, collision.transform.position, Quaternion.identity);
//             //AudioSource.PlayClipAtPoint(explosionAudio, collision.transform.position,1.5f);
//             //Destroy(gameObject);
//             //MyScript.GameOver(); 
//             //gameScript.die();
//             // Game Over here
//         }
      
//     }
// }
}
