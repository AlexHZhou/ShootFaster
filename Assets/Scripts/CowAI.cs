using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class CowAI : MonoBehaviour {
    //Field. Will not leave bounds.
    public Transform leftBound;
    public Transform rightBound;
    
    private Rigidbody2D rb;

    //The AI's speed per second
    public float speed;
    public ForceMode2D fMode;
    
    private Animator animator;

    private bool moveRight = true;
    private bool shouldMoveRight = true;
    public bool canMove = true;

    private float rand;

    // Update is called once per frame
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        StartCoroutine(LazyMove());
    }

    IEnumerator LazyMove()
    {
        if (!canMove) yield return null;
        int stallTime = Random.Range(0, 6);
        //randomizes movement direction
        rand = Random.value;
        if (rand <= .4f)
        {
            animator.SetBool("isWalking", true);
            RandomizeMoveDirec();

            BeginMove();//40% begin move
        }
        else animator.SetBool("isWalking", false);
        yield return new WaitForSeconds(stallTime);
        StartCoroutine(LazyMove());
    }

    void RandomizeMoveDirec()
    {
        rand = Random.Range(0, 1);
        if (rand <= .5f) // move right
        {
            if (transform.position.x > leftBound.position.x) //if too far right, flip
            {
                shouldMoveRight = false;
            }
            else shouldMoveRight = true;
        }
        else //move left
        {
            if (transform.position.x < rightBound.position.x) shouldMoveRight = true;
            else shouldMoveRight = false;
        }
    }

    void BeginMove() {
        
        rand = Random.value;

        Vector2 dir = new Vector2(1, 0) * speed;
        //cows can only move left or right. No y component
        if (!shouldMoveRight)
        {
            dir.x *= -1;
        }
       
        if (moveRight != shouldMoveRight) Flip();

       
        rand = Random.Range(5, 20);
        while (rand > 0)
        {
            Moove(dir, fMode);
            rand--;
        }
        if (dir.x >= 0) moveRight = true;
        else moveRight = false;
        
    }

    private void Moove(Vector2 dir, ForceMode2D fm)
    {

        //Debug.Log("Moving! in " + dir);
        rb.AddForce(dir, fMode);
        //if (rand <= .90f) Moove(dir, fm); //75% of time, loop move.
    }

    private void Flip()
    {
        // Switch the way the player is labelled as facing.
        moveRight = !moveRight;

        // Multiply the player's x local scale by -1.
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
    
}
