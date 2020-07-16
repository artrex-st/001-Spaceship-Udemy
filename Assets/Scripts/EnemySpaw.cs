using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpaw : MonoBehaviour
{
    public GameObject enemy;
    public float left = -30, right = 30, top = 30, bot = -30, distance = 250;
    [Range(0,10)]
    public float cd;

    // Update is called once per frame
    void Update()
    {
        cd += Time.deltaTime;
        if (cd >= 5)
        {
            Instantiate(enemy, new Vector3(Random.Range(left, right), Random.Range(bot, top), distance), new Quaternion(transform.rotation.x, 180, transform.rotation.z, transform.rotation.w));
            cd = 0;
        }
    }
}
