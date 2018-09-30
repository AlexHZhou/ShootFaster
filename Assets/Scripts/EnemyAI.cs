 using UnityEngine;
using System.Collections;
using Pathfinding;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Seeker))]
public class EnemyAI : MonoBehaviour
{
    
    //public CowAI cow;

    // What to chase?
    public Transform target;

    // How many times each second we will update our path
    public float updateRate = 5f;

    // Caching
    private Seeker seeker;
    private Rigidbody2D rb;

    //The calculated path
    public Path path;

    //The AI's speed per second
    public float speed;
    public ForceMode2D fMode;

    [HideInInspector]
    public bool pathIsEnded = false;

    // The max distance from the AI to a waypoint for it to continue to the next waypoint
    public float nextWaypointDistance = 3;

    // The waypoint we are currently moving towards
    private int currentWaypoint = 0;

    //attacking variables.
    private Animator animator;

    //public delegate void freezeTargetCallback(bool active);
    //public freezeTargetCallback onTargetFreeze;
    public float riseSpeed = 500;

    //private void OnTriggerStay2D(Collider2D col)
    //{
    //    GameObject obj = col.gameObject;
    //    Vector2 tarPos = moveHere.position;
    //    Vector2 objPos = col.transform.position;
    //    Vector2 dir = (objPos - tarPos).normalized;
    //    float dx = objPos.x - tarPos.x;

    //    bool isAttracting;
    //}

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();

        if (target == null)
        {
            LookForPlayer();
        }
        else
        {
            seeker.StartPath(transform.position, target.position, OnPathComplete);
            StartCoroutine(UpdatePath());
        }

    }

    IEnumerator UpdatePath()
    {
        try
        {
            seeker.StartPath(transform.position, target.position, OnPathComplete);
            //Debug.Log("Starting path.");
        }
        catch //player not found
        {
            LookForPlayer();
        }

        yield return new WaitForSeconds(1f / updateRate);
        StartCoroutine(UpdatePath());
    }

    void LookForPlayer()
    {
        try
        {
            GameObject searchResult = GameObject.FindGameObjectWithTag("Target");
            if (searchResult != null)
            {
                target = searchResult.transform;
                Debug.Log("Target found.");
                StartCoroutine(UpdatePath());
            }
        }
        catch
        {
            Debug.Log("Player NOT found. AHHHHH");
        };
    }

    public void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    void FixedUpdate()
    {
        if (target == null)
        {
            LookForPlayer();
            return;
        }
        if (path == null)
            return;

        if (currentWaypoint >= path.vectorPath.Count)
        {
            if (pathIsEnded)
                return;

            pathIsEnded = true;
            return;
        }
        pathIsEnded = false;

        //Direction to the next waypoint
        Vector3 dir = (path.vectorPath[currentWaypoint] - transform.position).normalized;
        dir *= speed * Time.fixedDeltaTime;

        ////Attacking Anim
        //if (Mathf.Abs(transform.position.x - target.position.x) <= 1)
        //{
        //    animator.SetBool("Attacking", true);
        //    cow.canMove = false;
        //    StartCoroutine(TractorBeam());

        //}
        //else
        //{
        //    animator.SetBool("Attacking", false);
        //}

        //Move the AI
        rb.AddForce(dir, fMode);

        float dist = Vector3.Distance(transform.position, path.vectorPath[currentWaypoint]);
        if (dist < nextWaypointDistance)
        {
            currentWaypoint++;
            return;
        }
    }

    //IEnumerator TractorBeam()
    //{
    //    cow.transform.position = Vector2.Lerp(cow.transform.position, target.position, 1);

    //    yield return null;
    //    //if (cow.transform.position.y + 10 > transform.position.y) yield return null;
    //    //else StartCoroutine(TractorBeam());
    //}
}