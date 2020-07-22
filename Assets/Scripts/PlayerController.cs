using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float left = -30, right = 30, top= 30, bot= -30, speed = 50, cd=1;
    public Transform firePointRight, firePointLeft;
    public GameObject Bullet;
    public AudioClip shotSound;
    private Transform _selection;

    private void Start()
    {
    }
    private void Update()
    {
        cd += Time.deltaTime;
        Move();
        if (Input.GetButtonDown("Fire1") && cd >=1)
        {
            shoot();
            cd = 0;
        }

        ///


        if (_selection != null)
        {
            _selection.GetComponentInChildren<Light>().enabled = false;
            _selection = null;
        }
        RaycastHit hit;
        if (Physics.Raycast(firePointLeft.position, transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity) || Physics.Raycast(firePointRight.position, transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity))
        {
            //Debug.DrawRay(firePointLeft.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.red);
            //Debug.DrawRay(firePointRight.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
            //Debug.Log($"Raycast hit: {hit.transform.name} @@@@@@");
            
            if (hit.transform.CompareTag("Enemy"))
            {
                hit.transform.GetComponentInChildren<Light>().enabled = true;
                _selection = hit.transform;
            }
        }



        ///

    }

    private void Move()
    {

        //Vector3 pos = new Vector3(Input.GetAxis("Horizontal") * speed, Input.GetAxis("Vertical") * speed) * Time.deltaTime;
        Vector3 pos = new Vector3(Input.GetAxis("Horizontal") * speed, 0) * Time.deltaTime;
        GetComponentInChildren<Animator>().SetFloat("Direction",pos.x);
        transform.position = new Vector3(Mathf.Clamp(transform.position.x + pos.x, right, left), Mathf.Clamp(transform.position.y + pos.y, bot, top),transform.position.z);

    }

    private void shoot()
    {
        Instantiate(Bullet, firePointLeft.position, firePointLeft.rotation);
        Instantiate(Bullet, firePointRight.position, firePointRight.rotation);
        GetComponent<AudioSource>().clip = shotSound;
        GetComponent<AudioSource>().Play();
    }
}
