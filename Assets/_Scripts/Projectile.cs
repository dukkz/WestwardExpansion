using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Projectile : MonoBehaviour
{
    public float speed, existTime, maxExistTime, damage;

    // Update is called once per frame
    void Update()
    {
        this.transform.position += transform.up * speed;

        existTime += Time.deltaTime;

        if(existTime > maxExistTime)
        {
            Destroy(this.gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if(col.gameObject.GetComponent<Stats>())
        {
            col.gameObject.GetComponent<Stats>().hp -= damage;
        }
        Destroy(this.gameObject);
    }
}
