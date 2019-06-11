//Script Created By Rees Herbert
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ai_controller : MonoBehaviour
{
    [SerializeField] private NavMeshAgent navAgent = null;
    [SerializeField] private GameObject target = null;
    private bool attacking = false;
    private bool searching = false;
    public bool patrolling = true;
    
    private Animator anim;
    private Vector3 playerLastKnownPosition;

    private int patrolTargetNum;
    private const int IDLE = 0;
    private const int PATROLLING = 1;
    private const int PURSUING = 2;
    private const int ATTACKING = 3;
    private const int SEARCHING_FIRST = 4;
    private const int SEARCHING_SECOND = 5;
    private const int HURT = 6;

    public Transform eyePosition;
    public float rotationSpeed;
    public float attackSpeed;
    public float creepSpeed;
    public float runSpeed;
    public float attackRange;
    public float maxRange;
    public float attackAngle;
    public float viewAngle;
    public float patrolSwitchRange;
    public GameObject[] patrolTargets;
    public GameObject eyeDirection;
    public GameObject marker;

    [Header("Sight Variables")]
    [SerializeField] private Vector3 sightRadius = Vector3.zero;
    [SerializeField] private float sightOffset = 0f;

    //initialize monster ai_controller
    void Start()
    {
        patrolTargetNum = 0;
        anim = GetComponent<Animator>();
        stateSwitcher(PATROLLING);
    }

    //reset monster behavior
    public void init()
    {
        attacking = false;
        stateSwitcher(PATROLLING);
    }

    /// <summary>
    /// This function takes in a state case (determined by the previous state that called it)
    /// and starts the appropriate co-routine, based on the state. It is necessary to stop
    /// all co-routines because they will eventually build up on the stack otherwise.
    /// </summary>
    /// <param name="stateCase"></param>
    /// <returns></returns>
    private bool stateSwitcher(int stateCase)
    {
        StopAllCoroutines();
        switch (stateCase)
        {
            case IDLE:
                {
                    StartCoroutine("Idle");
                    break;
                }
            case PATROLLING:
                {
                    StartCoroutine("Patrolling");
                    break;
                }
            case PURSUING:
                {
                    StartCoroutine("Pursuing");
                    break;
                }
            case ATTACKING:
                {
                    StartCoroutine("Attacking");
                    break;
                }
            case SEARCHING_FIRST:
                {
                    StartCoroutine("SearchingFirst");
                    break;
                }
            case SEARCHING_SECOND:
                {
                    StartCoroutine("SearchingSecond");
                    break;
                }
            case HURT:
                {
                    StartCoroutine("Hurt");
                    break;
                }
            default:
                {
                    return false;
                }
        }
        return true;
    }

    /*
     * Idle
     * Patrolling
     * Pursuing
     * Attacking
     * Searching - first search
     * Searching - second search
     * Hurt/Dying
     * */

    //Enemy is stopped.
    //Enemy is playing idle animation
    //Enemy is checking for player
    //Calls stateSwitcher if check return != 0
    IEnumerator Idle()
    {
        navAgent.isStopped = true;
        navAgent.velocity = Vector3.zero;
        setAllAnimParamsFalse();
        anim.SetBool("Idle", true);
        yield return new WaitForSeconds(0.1f);
        int checkInRangeReturn;
        while (true)
        {
            checkInRangeReturn = checkInRange();
            if (checkInRangeReturn != IDLE)
            {
                stateSwitcher(checkInRangeReturn);
                break;
            }

            yield return null;
        }
    }

    /// <summary>
    /// This function sets ai variables to properly engage the patrolling behavior. 
    /// it then loops until the player is detected. While patrolling once the ai is
    /// within range of a patrol point it sets the destination to the next patrol point.
    /// </summary>
    /// <returns></returns>
    IEnumerator Patrolling()
    {
        float timer = 1.0f;
        float currentTime = 0.0f;
        bool changingTarget = false;

        navAgent.isStopped = false;
        setAllAnimParamsFalse();
        anim.SetBool("Creep", true);
        setSpeedCreep();
        navAgent.SetDestination(patrolTargets[patrolTargetNum].transform.position);
        int checkInRangeReturn;
        patrolling = true;
        
        while (patrolling == true)
        {
            if(changingTarget == true)
            {
                currentTime += Time.deltaTime;
                if(currentTime > timer)
                {
                    changingTarget = false;
                }
            }
            checkInRangeReturn = checkInRange();
            if (checkInRangeReturn != IDLE)
            {
                patrolling = false;
                stateSwitcher(checkInRangeReturn);
                break;
            }

            if (navAgent.remainingDistance < patrolSwitchRange && changingTarget == false)
            {
                changingTarget = true;
                currentTime = 0.0f;
                patrolTargetNum = (patrolTargetNum + 1) % patrolTargets.Length;
                Debug.Log("Patrol target num: " + patrolTargetNum.ToString());
                navAgent.SetDestination(patrolTargets[patrolTargetNum].transform.position);
            }

            yield return null;
        }
    }

    /// <summary>
    /// This function sets ai variables to properly engage the pursuit behavior.
    /// The function loops until the player is either in attack range, or no longer
    /// in view. The checkInRange() function determines whether or not the player
    /// is within attack range, or no longer in view.
    /// </summary>
    /// <returns></returns>
    IEnumerator Pursuing()
    {
        navAgent.isStopped = false;
        setAllAnimParamsFalse();
        anim.SetBool("Run", true);
        setSpeedRun();
        int checkInRangeReturn;
        while (true)
        {
            checkInRangeReturn = checkInRange();
            if (checkInRangeReturn == IDLE)
            {
                stateSwitcher(SEARCHING_FIRST);
                break;
            }
            if (checkInRangeReturn == ATTACKING)
            {
                stateSwitcher(checkInRangeReturn);
                break;
            }
                
            navAgent.SetDestination(target.transform.position);
            FaceTarget(playerLastKnownPosition);
            yield return null;
        }
    }

    /// <summary>
    /// This function sets ai variables to properly engage the attack behavior.
    /// The function loops while the attack animation is playing and checks if the
    /// player is in range. If the player is no longer visible, the function calls
    /// the state switcher and begins a search.
    /// </summary>
    /// <returns></returns>
    IEnumerator Attacking()
    {
        if(anim.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            anim.Play("Attack",-1,0.0f);
        }
        navAgent.isStopped = true;
        navAgent.velocity = Vector3.zero;
        setAllAnimParamsFalse();
        anim.SetBool("Attack", true);
        attacking = true;
        int checkInRangeReturn;
        while (attacking == true)
        {
            checkInRangeReturn = checkInRange();
            yield return null;
        }
        checkInRangeReturn = checkInRange();
        if (checkInRangeReturn == IDLE)
        {
            
            stateSwitcher(SEARCHING_FIRST);
        }
        else if(checkInRangeReturn == PURSUING)
        {
            stateSwitcher(checkInRangeReturn);
        }
        else
        {
            
            stateSwitcher(IDLE);
        }
    }

    /// <summary>
    /// This function sets ai variables to properly engage the first search behavior.
    /// The ai will turn on the spot and search for the player. If the player is detected
    /// the function will begin a pursuit, otherwise the function will call the second 
    /// search function.
    /// </summary>
    /// <returns></returns>
    IEnumerator SearchingFirst()
    {
        navAgent.isStopped = true;
        navAgent.velocity = Vector3.zero;
        setAllAnimParamsFalse();
        anim.SetBool("Search", true);
        searching = true;

        int checkInRangeReturn;
        while (searching)
        {
            checkInRangeReturn = checkInRange();
            if (checkInRangeReturn != IDLE)
            {
                searching = false;
                stateSwitcher(checkInRangeReturn);
                break;
            }
            yield return null;
        }
        stateSwitcher(SEARCHING_SECOND);
    }

    /// <summary>
    /// This function sets ai variables to properly engage the second search behavior.
    /// The ai will move to the last direct line of sight location of the player 
    /// (not determined by gaze, just an unblocked line to the player) and begin another
    /// search. If the player is detected it will start pursuit, otherwise it will return
    /// to patrol.
    /// </summary>
    /// <returns></returns>
    IEnumerator SearchingSecond()
    {
        bool changedToPursuit = false;

        navAgent.isStopped = false;
        setAllAnimParamsFalse();
        anim.SetBool("Run", true);
        setSpeedRun();
        patrolling = true;
        searching = false;
        navAgent.SetDestination(playerLastKnownPosition);
        yield return null;
        if (navAgent.path.status != NavMeshPathStatus.PathComplete) { yield return null; }
        int checkInRangeReturn;
        while (patrolling == true)
        {
            checkInRangeReturn = checkInRange();
            if (checkInRangeReturn != IDLE)
            {
                patrolling = false;
                searching = false;
                stateSwitcher(checkInRangeReturn);
                break;
            }
            if (navAgent.remainingDistance < patrolSwitchRange)
            {
                patrolling = false;
                break;
            }

            yield return null;
        }

        searching = true;
        navAgent.isStopped = true;
        navAgent.velocity = Vector3.zero;
        setAllAnimParamsFalse();
        anim.SetBool("Search", true);
        while (searching && !patrolling)
        {
            checkInRangeReturn = checkInRange();
            if (checkInRangeReturn != IDLE)
            {
                patrolling = false;
                searching = false;
                changedToPursuit = true;
                stateSwitcher(checkInRangeReturn);
                break;
            }
            yield return null;
        }

        if (changedToPursuit == false)
        {
            stateSwitcher(PATROLLING);
        }
    }

    IEnumerator Hurt()
    {
        yield return null;
    }

    /// <summary>
    /// checks if the target (set as a public game object) is within range and within viewing angle
    /// returns 2 if player is within attack range and within attackAngle (player directly-ish in front of enemy)
    /// returns 1 if player is within max range and viewAngle (player spotted)
    /// 0 if none of those conditions are true
    /// </summary>
    /// <returns></returns>
    int checkInRange()
    {
        int layerMask = 1 << 2;
        layerMask = ~layerMask;
        Vector3 heading = (target.transform.position - eyeDirection.transform.position).normalized;
        RaycastHit hit;

        float dot = Vector3.Dot(eyeDirection.transform.forward, heading);
        float distance = Vector3.Distance(transform.position, target.transform.position);

        Debug.DrawRay(eyePosition.position, (target.transform.position - eyePosition.position));

        if (Physics.Raycast(eyeDirection.transform.position, (target.transform.position - eyeDirection.transform.position), out hit, maxRange, layerMask))
        {
            if (dot > attackAngle && distance < attackRange && hit.transform.tag == "Player")
            {
                setLastKnownPosition();
                return ATTACKING;
            }
            else if (dot > viewAngle && distance < maxRange && hit.transform.tag == "Player")
            {
                setLastKnownPosition();
                return PURSUING;
            }
            if (hit.transform.tag == "Player")
            {
                setLastKnownPosition();
            }
        }
        return IDLE;
    }

    /// <summary>
    /// turns ai to face player, destination is player transform.position
    /// </summary>
    /// <param name="destination"></param>
    private void FaceTarget(Vector3 destination)
    {
        Vector3 lookPos = destination - transform.position;
        lookPos.y = 0;
        Quaternion rotation = Quaternion.LookRotation(lookPos);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationSpeed);
    }

    /// <summary>
    /// Sets the speed of the navmesh agent to the public creepSpeed value.
    /// </summary>
    private void setSpeedCreep()
    {
        GetComponent<NavMeshAgent>().speed = creepSpeed;
    }

    /// <summary>
    /// Sets the speed of the navmesh agent to the public runSpeed value.
    /// </summary>
    private void setSpeedRun()
    {
        GetComponent<NavMeshAgent>().speed = runSpeed;
    }

    /// <summary>
    /// Stores the position of the player object.
    /// </summary>
    private void setLastKnownPosition()
    {
        playerLastKnownPosition = new Vector3(target.transform.position.x, target.transform.position.y, target.transform.position.z);
    }

    /// <summary>
    /// resets all of the animation parameters to false
    /// </summary>
    private void setAllAnimParamsFalse()
    {
        anim.SetBool("Walk", false);
        anim.SetBool("Creep", false);
        anim.SetBool("Run", false);
        anim.SetBool("Attack", false);
        anim.SetBool("Idle", false);
        anim.SetBool("Search", false);
    }

    /// <summary>
    /// Sets attacking to true.
    /// </summary>
    public void attackStart()
    {
        attacking = true;
    }

    /// <summary>
    /// Sets attacking to false.
    /// </summary>
    public void attackEnd()
    {
        attacking = false;
    }

    /// <summary>
    /// Sets searching to false.
    /// </summary>
    void endSearch()
    {
        searching = false;
    }

    /// <summary>
    /// Draws a wire cube in the scene editor.
    /// </summary>
    private void OnDrawGizmosSelected()
    {
        // Drawing SightBox
        Gizmos.color = Color.white;
        Gizmos.DrawWireCube(transform.position + (navAgent.velocity * sightOffset), sightRadius);
    } 
}
