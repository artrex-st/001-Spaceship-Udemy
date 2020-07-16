using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletEnemy : MonoBehaviour
{
    public float speed = 10;
    public GameObject explosion;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 15f);
        speed = Enemy.bulletspeed;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += new Vector3(0, 0, speed * -Time.deltaTime);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log($"Hit: {collision.gameObject.name}");
            Destroy(gameObject, 0.1f);
            Instantiate(explosion, transform.position, transform.rotation);

        }
    }
}
