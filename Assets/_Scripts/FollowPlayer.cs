using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public Transform player;
    public float lerpSpeed;

    
    void Awake()
    {
        player = GameObject.Find("Player").transform;
    }

    void FixedUpdate()
    {

        this.transform.position = Vector3.Lerp(this.transform.position, new Vector3(player.position.x, player.position.y, 0), lerpSpeed * Time.deltaTime);
    }
}
