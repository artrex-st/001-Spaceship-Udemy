using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour
{
    public float enemyHealth, enemySpeed, enemyattack, cd=0, shootRate=5;
    public GameObject[] rngEnemy;
    public GameObject boss;
    public GameObject bullet;
    public static float bulletspeed;
    public GameObject explosionEffect, explosion;
    public static Boolean targetOn;
    public Boolean isBoss, bossMoveR;
    public LineRenderer lineShot;
    public Transform[] firePoint;
    public AudioClip shotBullet, laserShot;



    void Start()
    {
        int i = Random.Range(0,rngEnemy.Length);
        if (!isBoss)
        {
            Instantiate(rngEnemy[i], transform.position, transform.rotation, gameObject.transform);
            enemyattack = i * 4f;
            enemyHealth = i * 2f;
            enemySpeed = (5 * enemyHealth) / (enemyattack + 1);
            bulletspeed = enemySpeed + enemyattack + 10;
        }
        else
        {
            Instantiate(boss, transform.position, transform.rotation, gameObject.transform);
            enemyattack = 50f;
            enemyHealth = 200f;
            enemySpeed = 20;
            shootRate = 2f;
            lineShot = GetComponent<LineRenderer>();
        }

        //Debug.Log($"Nave escolhida é a: {i}");
    }

    void Update()
    {
        if (!isBoss)
        {
            transform.position += new Vector3(0, 0, enemySpeed * -Time.deltaTime);
        }else
        if (isBoss)
        {
            BossMove();
        }


        cd += Time.deltaTime;
        if (enemyattack>0 && cd >= shootRate && !isBoss)
        {
            Shoot();
            cd = 0;
        }else
        if (enemyattack > 0 && cd >= shootRate && isBoss)
        {
            StartCoroutine(ShootLaser());
            cd = Random.Range(0.5f, 1.5f) / enemyHealth;
        }

    }

    IEnumerator ShootLaser()
    {       //Instantiate(bullet, firePointLeft.position, firePointLeft.rotation);
            //Instantiate(bullet, firePointRight.position, firePointRight.rotation);
        GetComponent<AudioSource>().clip = laserShot;
        GetComponent<AudioSource>().Play();
        int iFire = Random.Range(0, 4);
        RaycastHit hitShot;
        if (Physics.Raycast(firePoint[iFire].position, transform.TransformDirection(Vector3.forward), out hitShot, Mathf.Infinity))
        {
            lineShot.SetPosition(0, firePoint[iFire].position);
            lineShot.SetPosition(1, hitShot.point);
            if (hitShot.transform.CompareTag("Player"))
            {
                hitShot.transform.GetComponent<PlayerController>().ApplyDmg(enemyattack);
                Instantiate(explosion, hitShot.point, transform.rotation);
                Debug.Log($"raycast enemy acertou o player: {hitShot.transform.name}");
            }
        }
        else
        {
            lineShot.SetPosition(0, firePoint[iFire].position);
            lineShot.SetPosition(1, firePoint[iFire].position + firePoint[iFire].position * 350);
            Debug.LogWarning($"raycast enemy não acertou nada");
        }
        lineShot.enabled = true;
        yield return 0.2f;
        lineShot.enabled = false;
    }

    private void BossMove()
    {
        Vector3 bossMove;
        if (bossMoveR)
        {
            bossMove = new Vector3(Mathf.Clamp(transform.position.x + enemySpeed * Time.deltaTime, -20, 20), 0, transform.position.z);
            transform.position = bossMove;
            if (transform.position.x >= Random.Range(10, 25))
            {
                bossMoveR = false;
            }
        }
        else
        {
            bossMove = new Vector3(Mathf.Clamp(transform.position.x - enemySpeed * Time.deltaTime, -20, 20), 0, transform.position.z);
            transform.position = bossMove;
            if (transform.position.x <= Random.Range(-25, -10))
            {
                bossMoveR = true;
            }

        }

    }
    private void Shoot()
    {
        GetComponent<AudioSource>().clip = shotBullet;
        GetComponent<AudioSource>().Play();
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
        if (enemyHealth <= 0)
        {
            Destroy(gameObject, 0.2f);
            Instantiate(explosionEffect, transform.position, transform.rotation);
        }
    }
    public void Lights(Boolean target)
    {
        GetComponentInChildren<Light>().enabled = target;
    }

}
