using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy : MonoBehaviour
{
    protected Animator animator;
    protected virtual void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    private void Update()
    {
        
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
