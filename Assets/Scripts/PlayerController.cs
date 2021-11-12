using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour 
{

    private Rigidbody2D rb;
    private Animator animator;
    private Collider2D collider;

    private Animator frogAnimator;
    
    private enum State { idle, running, jumping, falling, hurt}
    private State state = State.idle;

    [SerializeField] private LayerMask groundMask;
    [SerializeField] private float speed;
    [SerializeField] private float jumpForce;
    [SerializeField] private int cherries = 0;
    [SerializeField] private Text cherryText;
    [SerializeField] private float recoil = 10f;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        collider = GetComponent<Collider2D>();
    }

    public void Update()
    {
        if (state != State.hurt)
        {
            movement();
        }
        animationState();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Collectible")
        {
            Destroy(collision.gameObject);
            cherries++;
            cherryText.text = cherries.ToString();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            frog myEnemy = collision.gameObject.GetComponent<frog>();
            if (state == State.falling)
            {
                myEnemy.stomped();
                Jump();
            } 
            else
            {
                state = State.hurt;
                if (collision.gameObject.transform.position.x > transform.position.x)
                {
                    //Enemy is on the right, I should recoil to the left
                    rb.velocity = new Vector2(-recoil, rb.velocity.y);
                }
                else
                {
                    //Enemy is on left, therefore I should recoil to the right
                    rb.velocity = new Vector2(recoil, rb.velocity.y);
                }
            }
        }
    }

    private void movement()
    {
        float hDirection = Input.GetAxis("Horizontal");

        if (hDirection < 0)
        {
            rb.velocity = new Vector2(-speed, rb.velocity.y);
            transform.localScale = new Vector2(-1, 1);
        }
        else if (hDirection > 0)
        {
            rb.velocity = new Vector2(speed, rb.velocity.y);
            transform.localScale = new Vector2(1, 1);
        }

        if (collider.IsTouchingLayers(groundMask) && Input.GetButtonDown("Jump"))
        {
            Jump();
        }
    }

    private void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        state = State.jumping;
    }

    private void animationState()
    {
        if (state == State.jumping)
        {
            if (rb.velocity.y < 0.1f)
            {
                state = State.falling;
            }

        }
        else if (state == State.falling)
        {
            if (collider.IsTouchingLayers(groundMask))
            {
                state = State.idle;
            }
        }
        else if (state == State.hurt)
        {
            if (Mathf.Abs(rb.velocity.x) < 0.1f)
            {
                state = State.idle;
            }
        }
        else if (Mathf.Abs(rb.velocity.x) > 2f)
        {
            state = State.running;
        }
        else
        {
            state = State.idle;
        }

        animator.SetInteger("state", (int)state);
    }
}
