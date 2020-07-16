using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float enemyHealth, enemySpeed, enemyattack, cd=0, shootRate=5;
    public GameObject[] rngEnemy;
    public GameObject bullet;
    public static float bulletspeed;
    public GameObject explosionEffect;

    
    void Start()
    {
        int i = Random.Range(0,rngEnemy.Length);
        Instantiate(rngEnemy[i], transform.position, transform.rotation, gameObject.transform);
        enemyattack = i * 2f;
        enemyHealth = i * 2f;
        enemySpeed = (5 * enemyHealth) / (enemyattack + 1);
        bulletspeed = enemySpeed + enemyattack + 10;
        Debug.Log($"Nave escolhida é a: {i}");
    }

    void Update()
    {
        transform.position += new Vector3(0, 0, enemySpeed * -Time.deltaTime);
        cd += Time.deltaTime;
        if (enemyattack>0 && cd >= shootRate)
        {
            shoot();
            cd = 0;
        }

    }

    private void shoot()
    {
        Instantiate(bullet, new Vector3(transform.position.x, transform.position.y, transform.position.z-10), transform.rotation);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            Debug.Log($"Hit: {collision.gameObject.name}, Hp restante: {enemyHealth}");
            ApplyDmg(2);

        }
    }
    public void ApplyDmg(float dmg)
    {
        enemyHealth -= dmg;
        GetComponent<AudioSource>().Play();
        if (enemyHealth <= 0)
        {
            Destroy(gameObject, 0.2f);
            Instantiate(explosionEffect, transform.position, transform.rotation);
        }
    }
}
