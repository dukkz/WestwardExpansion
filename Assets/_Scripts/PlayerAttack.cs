using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerAttack : NetworkBehaviour
{
    public GameObject projectile;
    public bool hasRangeWeapon = true, hasAttacked = false;
    public float attackCD = .5f, defaultAttackCD = .5f, bullets = 6;
    public Vector3 projectileSpawnOffset;
    private Transform projectileSpawnPoint, projectileSpawnRotation;

    void Update()
    {
        if(hasAttacked)
        {
            if(attackCD > 0)
            {
                attackCD -= Time.deltaTime;
            } 
            else 
            {
                hasAttacked = false;
                attackCD = defaultAttackCD;
            }
        }
        else return;
    }

    [Command] public void Attack()
    {
        if(!hasAttacked)
        {
            projectileSpawnRotation = GetComponentInChildren<Weapon>().transform;

            foreach(GameObject weapon in GameObject.FindGameObjectsWithTag("Weapon"))
            {
                if(weapon.gameObject.GetComponentInParent<PlayerMotor>().isLocalPlayer)
                {
                    projectileSpawnPoint = weapon.transform;
                }
            }

            if(hasRangeWeapon && projectileSpawnPoint != null)
            {
                GameObject bullet = Instantiate(projectile, projectileSpawnPoint.position + projectileSpawnOffset, projectileSpawnRotation.rotation);
                NetworkServer.Spawn(bullet);
                hasAttacked = true;
            }
        }
    }
}
