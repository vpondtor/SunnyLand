using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class frog : MonoBehaviour
{
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private float leftCap;
    [SerializeField] private float rightCap;
    [SerializeField] private float jumpLength;
    [SerializeField] private float jumpHeight;

    protected Animator animator;
    private Collider2D collider;
    private Rigidbody2D rb;
    private bool facingLeft = true;

    private enum State { idle, jumping, falling, death}
    private State frogState = State.idle;

    // Start is called before the first frame update
    private void Start()
    {

        collider = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

    }

    // Update is called once per frame
    private void Update()
    {

        animateState();

    }

    public void move()
    {
        if (facingLeft)
        {
            if (transform.position.x > leftCap)
            {
                if (collider.IsTouchingLayers(groundMask))
                {
                    transform.localScale = new Vector3(1, 1, 1);
                    frogState = State.jumping;
                    rb.velocity = new Vector2(-jumpLength, jumpHeight);
                }
            }
            else
            {
                facingLeft = false;
            }
        }
        else
        {
            if (transform.position.x < rightCap)
            {
                if (collider.IsTouchingLayers(groundMask))
                {
                    transform.localScale = new Vector3(-1, 1, 1);
                    frogState = State.jumping;
                    rb.velocity = new Vector2(jumpLength, jumpHeight);
                }
            }
            else
            {
                facingLeft = true;
            }

        }
    }

    private void animateState()
    {
        if (frogState == State.jumping)
        {
            if (rb.velocity.y < 0.1f)
            {
                frogState = State.falling;
            }

        }
        else if (frogState == State.falling)
        {
            if (collider.IsTouchingLayers(groundMask))
            {
                frogState = State.idle;
            }
        }

        animator.SetInteger("frogState", (int)frogState);
    }

    public void stomped()
    {
        animator.SetTrigger("death");
    }

    public void selfDestruct()
    {
        Destroy(this.gameObject);
    }
}
