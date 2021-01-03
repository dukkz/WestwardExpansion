using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMotor : NetworkBehaviour
{
    public float moveSpeed, maxVelocityMagnitude, axisDeadzone;
    public bool isMoving;
    public Camera playerCamera;

    private Rigidbody2D rb;
    private bool facingLeft, facingRight, facingUp, facingDown, isAttacking;
    private float verticalInput, horizontalInput;
    private Vector2 cursorScreenPosition, cursorWorldPosition;
    private GameObject weapon;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        weapon = GetComponentInChildren<Weapon>().gameObject;
    }

    private void Update()
    {
        PlayerAttack();
        DetermineDirection();
        WeaponFaceMouse();

        if(!isMoving)
        {
            rb.velocity = Vector2.zero;
        }
    }

    void FixedUpdate()
    {
        MovePlayer();
    }

    void MovePlayer()
    {
        if(isLocalPlayer)
        {
            verticalInput = Input.GetAxisRaw("Vertical");
            horizontalInput = Input.GetAxisRaw("Horizontal");
            if (Mathf.Abs(verticalInput) >= axisDeadzone || Mathf.Abs(horizontalInput) >= axisDeadzone)
            {
                isMoving = true;
                rb.velocity = new Vector2(horizontalInput * moveSpeed, verticalInput * moveSpeed);
                rb.velocity = Vector2.ClampMagnitude(rb.velocity, maxVelocityMagnitude);
            }
            else
            {
                isMoving = false;
                rb.velocity = Vector2.zero;
            }
        }
        
    }

    void PlayerAttack()
    {
        if(isLocalPlayer)
        {
            if (Input.GetAxis("Fire1") > 0)
            {
                isAttacking = true;
                GetComponent<PlayerAttack>().Attack();
            }
            else isAttacking = false;
        }
            
    }

    void DetermineDirection()
    {
        if(isLocalPlayer)
        {
                //Get cursor position in world space
            cursorScreenPosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            cursorWorldPosition = Camera.main.ScreenToWorldPoint(cursorScreenPosition);

            //Determine facing direction via movement
            if (isMoving)
            {
                if(Mathf.Abs(horizontalInput) > Mathf.Abs(verticalInput))
                {
                    if(horizontalInput > 0)
                    {
                        facingRight = true;
                        facingLeft = false;
                        facingUp = false;
                        facingDown = false;
                    }
                    else if(horizontalInput < 0)
                    {
                        facingRight = false;
                        facingLeft = true;
                        facingUp = false;
                        facingDown = false;
                    }
                }

                if (Mathf.Abs(verticalInput) > Mathf.Abs(horizontalInput))
                {
                    if (verticalInput > 0)
                    {
                        facingRight = false;
                        facingLeft = false;
                        facingUp = true;
                        facingDown = false;
                    }
                    else if (verticalInput < 0)
                    {
                        facingRight = false;
                        facingLeft = false;
                        facingUp = false;
                        facingDown = true;
                    }
                }
            }

                //If attacking, override facing direction to be towards cursor
            if(isAttacking)
            {
                if(cursorWorldPosition.x - this.transform.position.x < 0 && Mathf.Abs(cursorWorldPosition.x - this.transform.position.x) > Mathf.Abs(cursorWorldPosition.y - this.transform.position.y))
                {
                    facingRight = false;
                    facingLeft = true;
                    facingUp = false;
                    facingDown = false;
                }
                if (cursorWorldPosition.x - this.transform.position.x > 0 && Mathf.Abs(cursorWorldPosition.x - this.transform.position.x) > Mathf.Abs(cursorWorldPosition.y - this.transform.position.y))
                {
                    facingRight = true;
                    facingLeft = false;
                    facingUp = false;
                    facingDown = false;
                }
                if (cursorWorldPosition.y - this.transform.position.y < 0 && Mathf.Abs(cursorWorldPosition.y - this.transform.position.y) > Mathf.Abs(cursorWorldPosition.x - this.transform.position.x))
                {
                    facingRight = false;
                    facingLeft = false;
                    facingUp = false;
                    facingDown = true;
                }
                if (cursorWorldPosition.y - this.transform.position.y > 0 && Mathf.Abs(cursorWorldPosition.y - this.transform.position.y) > Mathf.Abs(cursorWorldPosition.x - this.transform.position.x))
                {
                    facingRight = false;
                    facingLeft = false;
                    facingUp = true;
                    facingDown = false;
                }
            }
        } 
    }

    void WeaponFaceMouse()
    {
        if(isLocalPlayer)
        {
            Vector2 dir = Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position);
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            weapon.transform.rotation = Quaternion.AngleAxis(angle - 90f, Vector3.forward);
        }
    }
}
