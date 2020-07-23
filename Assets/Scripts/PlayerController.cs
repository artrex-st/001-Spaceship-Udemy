using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float left = -30, right = 30, top= 30, bot= -30, speed = 50, cd=1;
    public Transform firePointRight, firePointLeft;
    public GameObject bullet, explosion;
    public AudioClip shotSound;
    private Transform _selection;
    public LayerMask layerEnemy;
    public LineRenderer lineShot;

    private void Start()
    {
    }
    private void Update()
    {
        cd += Time.deltaTime;
        Move();
        if (Input.GetButtonDown("Fire1") && cd >=1)
        {
            StartCoroutine(Shoot());
            cd = 0;
        }

        ///


        if (_selection != null)
        {
            _selection.GetComponentInChildren<Light>().enabled = false;
            _selection = null;
        }
        RaycastHit hit;
        if (Physics.Raycast(firePointLeft.position, transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, layerEnemy) || Physics.Raycast(firePointRight.position, transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, layerEnemy))
        {
            //Debug.DrawRay(firePointLeft.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.red);
            //Debug.DrawRay(firePointRight.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
            //Debug.Log($"Raycast hit: {hit.transform.name} @@@@@@");
            
            hit.transform.GetComponentInChildren<Light>().enabled = true;
            _selection = hit.transform;
        }
        //Debug.LogWarning($"raycast= {hit.point}");


        ///

    }

    private void Move()
    {

        //Vector3 pos = new Vector3(Input.GetAxis("Horizontal") * speed, Input.GetAxis("Vertical") * speed) * Time.deltaTime;
        Vector3 pos = new Vector3(Input.GetAxis("Horizontal") * speed, 0) * Time.deltaTime;
        GetComponentInChildren<Animator>().SetFloat("Direction",pos.x);
        transform.position = new Vector3(Mathf.Clamp(transform.position.x + pos.x, right, left), Mathf.Clamp(transform.position.y + pos.y, bot, top),transform.position.z);

    }

    IEnumerator Shoot()
    {
        //Instantiate(bullet, firePointLeft.position, firePointLeft.rotation);
        //Instantiate(bullet, firePointRight.position, firePointRight.rotation);
        GetComponent<AudioSource>().clip = shotSound;
        GetComponent<AudioSource>().Play();
        RaycastHit hitShot;
        if (Physics.Raycast(firePointLeft.position, transform.TransformDirection(Vector3.forward), out hitShot, Mathf.Infinity))
        {
            lineShot.SetPosition(0, firePointLeft.position);
            lineShot.SetPosition(1, hitShot.point);
            if (hitShot.transform.CompareTag("Enemy"))
            {
                hitShot.transform.GetComponent<Enemy>().ApplyDmg(2);
                hitShot.transform.GetComponent<AudioSource>().Play();
                Instantiate(explosion, hitShot.point, transform.rotation);
            }
        }
        else
        {
            lineShot.SetPosition(0, firePointLeft.position);
            lineShot.SetPosition(1, firePointLeft.position + firePointLeft.position * 350);
        }
        lineShot.enabled = true;
        yield return 0.02f;
        lineShot.enabled = false;

        RaycastHit hitShotR;
        if (Physics.Raycast(firePointRight.position, transform.TransformDirection(Vector3.forward), out hitShotR, Mathf.Infinity))
        {
            lineShot.SetPosition(0, firePointRight.position);
            lineShot.SetPosition(1, hitShotR.point);
            if (hitShotR.transform.CompareTag("Enemy"))
            {
                hitShotR.transform.GetComponent<Enemy>().ApplyDmg(2);
                hitShotR.transform.GetComponent<AudioSource>().Play();
                Instantiate(explosion, hitShotR.point, transform.rotation);
            }
        }
        else
        {
            lineShot.SetPosition(0, firePointRight.position);
            lineShot.SetPosition(1, firePointRight.position + firePointRight.position * 350);
        }
        lineShot.enabled = true;
        yield return 0.02f;
        lineShot.enabled = false;
    }
}
