using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public float lerpSpeed;
    private GameObject player;
    public bool foundPlayer = false;

    
    void Update()
    {
        if(!foundPlayer)
        {
            foreach(GameObject character in GameObject.FindGameObjectsWithTag("Player"))
            {
                if(character.gameObject.GetComponent<PlayerMotor>().isLocalPlayer)
                {
                    player = character;
                    foundPlayer = true;
                }
            }
        } else return;


    }

    void FixedUpdate()
    {

        this.transform.position = Vector3.Lerp(this.transform.position, new Vector3(player.transform.position.x, player.transform.position.y, 0), lerpSpeed * Time.deltaTime);
    }
}
