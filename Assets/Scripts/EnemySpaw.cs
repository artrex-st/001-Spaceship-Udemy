using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpaw : MonoBehaviour
{
    public GameObject enemy;
    public float left = -30, right = 30, top = 30, bot = -30, distance = 250;
    [Range(0,10)]
    public float cd;
    public float score;
    public Boolean bossON = false;
    public Color bossLightCollor;

    private void Start()
    {
        Debug.Log("Screen Height : " + Screen.height);
    }
    // Update is called once per frame
    void Update()
    {
        if (!bossON)
        {
            cd += Time.deltaTime;
        }
        if (cd >= 5 && score <= 100)
        {
            Instantiate(enemy, new Vector3(UnityEngine.Random.Range(left, right), UnityEngine.Random.Range(bot, top), distance), new Quaternion(transform.rotation.x, 180, transform.rotation.z, transform.rotation.w));
            cd = 0;
        }else
        if (score>=100 && !bossON)
        {
            GameObject bossInstance = (GameObject)Instantiate(enemy, new Vector3(UnityEngine.Random.Range(left, right), UnityEngine.Random.Range(bot, top), distance), new Quaternion(transform.rotation.x, 180, transform.rotation.z, transform.rotation.w));
            bossON = true;
            bossInstance.GetComponent<Enemy>().isBoss = true;
            bossInstance.GetComponentInChildren<Light>().color = bossLightCollor;
            //bossInstance.GetComponentInChildren<Light>().range *= 10;
        }
    }
}
