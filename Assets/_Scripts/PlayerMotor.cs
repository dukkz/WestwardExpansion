using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMotor : MonoBehaviour
{
    public float moveSpeed, maxVelocityMagnitude, axisDeadzone;
    public bool isMoving;
    public Camera playerCamera;

    private Rigidbody2D rb;
    private bool facingLeft, facingRight, facingUp, facingDown, isAttacking;
    private float verticalInput, horizontalInput;
    private Vector2 cursorScreenPosition, cursorWorldPosition;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        PlayerAttack();
        DetermineDirection();
    }

    void FixedUpdate()
    {
        MovePlayer();
    }

    void MovePlayer()
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

    void PlayerAttack()
    {
        if (Input.GetAxis("Fire1") > 0)
        {
            isAttacking = true;
        }
        else isAttacking = false;
    }

    void DetermineDirection()
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
