using System.Collections;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(DestroyBulletAfterSeconds());
    }

    void Update()
    {
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerMovement player = collision.gameObject.GetComponent<PlayerMovement>();
            if (player != null)
            {
                player.Die();
            }
        }

        Destroy(gameObject);
    }

    IEnumerator DestroyBulletAfterSeconds()
    {
       yield return new WaitForSeconds(5);
       Destroy(gameObject);

    }
} 

