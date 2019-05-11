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
    private bool patrolling = true;
    
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

    //[Header("Combat Variables")]
    //[SerializeField] private float attackPower = 10f;

    void Start()
    {
        patrolTargetNum = 0;
        anim = GetComponent<Animator>();
        stateSwitcher(PATROLLING);
    }

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
            //Debug.Log("In idle loop.");
            checkInRangeReturn = checkInRange();
            if (checkInRangeReturn != IDLE)
            {
                stateSwitcher(checkInRangeReturn);
                break;
            }

            yield return null;
        }
    }

    IEnumerator Patrolling()
    {
        navAgent.isStopped = false;
        setAllAnimParamsFalse();
        anim.SetBool("Creep", true);
        setSpeedCreep();
        navAgent.SetDestination(patrolTargets[patrolTargetNum].transform.position);
        int checkInRangeReturn;
        patrolling = true;
        while (patrolling == true)
        {
            //Debug.Log("running patrol coroutine");
            checkInRangeReturn = checkInRange();
            if (checkInRangeReturn != IDLE)
            {
                patrolling = false;
                stateSwitcher(checkInRangeReturn);
                break;
            }

            if (navAgent.remainingDistance < patrolSwitchRange)
            {
                //Debug.Log("Patrolling: changing target.");
                patrolTargetNum = (patrolTargetNum + 1) % patrolTargets.Length;
                navAgent.SetDestination(patrolTargets[patrolTargetNum].transform.position);
            }

            yield return null;
        }
    }

    IEnumerator Pursuing()
    {
        navAgent.isStopped = false;
        setAllAnimParamsFalse();
        anim.SetBool("Run", true);
        setSpeedRun();
        int checkInRangeReturn;
        while (true)
        {
            //Debug.Log("running pursuit coroutine");
            checkInRangeReturn = checkInRange();
            if (checkInRangeReturn == IDLE)
            {
                stateSwitcher(SEARCHING_FIRST);
                break;
            }
            if (checkInRangeReturn == ATTACKING)
            {
                //gotta test
                stateSwitcher(checkInRangeReturn);
                break;
            }
                
            navAgent.SetDestination(target.transform.position);
            FaceTarget(playerLastKnownPosition);
            yield return null;
        }
    }

    IEnumerator Attacking()
    {
        navAgent.isStopped = true;
        navAgent.velocity = Vector3.zero;
        setAllAnimParamsFalse();
        anim.SetBool("Attack", true);
        attacking = true;
        int checkInRangeReturn;
        while (attacking == true)
        {
            //Debug.Log("running attack coroutine");
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
            //Debug.Log("running first search coroutine");
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
        //Instantiate(marker, playerLastKnownPosition, Quaternion.identity);
        yield return null;
        if (navAgent.path.status != NavMeshPathStatus.PathComplete) { yield return null; }
        //yield return new WaitForSeconds(0.1f);
        int checkInRangeReturn;
        while (patrolling == true)
        {
            //Debug.Log("running second search coroutine -- move to location");
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
                //Debug.Log("in range of playerLastKnownLocation");
                
                patrolling = false;
                //searching = true;
                break;
            }

            yield return null;
        }


        //setAllAnimParamsFalse();
        //anim.SetBool("Idle", true);
        //yield return null;

        searching = true;
        navAgent.isStopped = true;
        navAgent.velocity = Vector3.zero;
        setAllAnimParamsFalse();
        anim.SetBool("Search", true);
        while (searching && !patrolling)
        {
            //Debug.Log("running second search coroutine -- search");
            //Debug.DrawLine(playerLastKnownPosition, eyeDirection.transform.position);
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
        //Debug.Log("Checking target range.");
        Vector3 heading = (target.transform.position - eyeDirection.transform.position).normalized;
        RaycastHit hit;

        float dot = Vector3.Dot(eyeDirection.transform.forward, heading);
        float distance = Vector3.Distance(transform.position, target.transform.position);

        //Debug.Log("Dot: " + dot + " Distance: " + distance);
        Debug.DrawRay(eyePosition.position, (target.transform.position - eyePosition.position));

        if (Physics.Raycast(eyeDirection.transform.position, (target.transform.position - eyeDirection.transform.position), out hit, maxRange))
        {
            //Debug.Log("Raycast Hit: " + hit.collider.name);
            if (dot > attackAngle && distance < attackRange && hit.transform.tag == "Player")
            {
                setLastKnownPosition();
                //Debug.Log("target in attack range.");
                return ATTACKING;
            }
            else if (dot > viewAngle && distance < maxRange && hit.transform.tag == "Player")
            {
                setLastKnownPosition();
                //Debug.Log("target in sight range.");
                return PURSUING;
            }
            if (hit.transform.tag == "Player")
            {
                //Debug.Log("raycast hit player.");
                setLastKnownPosition();
            }

            //if (dot > viewAngle && distance > maxRange && searchNeeded)
            //    searchNeeded = false;
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

    private void setSpeedCreep()
    {
        GetComponent<NavMeshAgent>().speed = creepSpeed;
    }

    private void setSpeedRun()
    {
        GetComponent<NavMeshAgent>().speed = runSpeed;
    }

    private void setLastKnownPosition()
    {
        //Debug.Log("Setting last known position.");
        playerLastKnownPosition = new Vector3(target.transform.position.x, target.transform.position.y, target.transform.position.z);
    }

    private void setAllAnimParamsFalse()
    {
        anim.SetBool("Walk", false);
        anim.SetBool("Creep", false);
        anim.SetBool("Run", false);
        anim.SetBool("Attack", false);
        anim.SetBool("Idle", false);
        anim.SetBool("Search", false);
    }

    public void attackStart()
    {
        attacking = true;
    }

    public void attackEnd()
    {
        //Debug.Log("Attacking Player");

        attacking = false;
    }

    void endSearch()
    {
        //Debug.Log("Ending search");
        searching = false;
    }

    private void OnDrawGizmosSelected()
    {
        // Drawing SightBox
        Gizmos.color = Color.white;
        Gizmos.DrawWireCube(transform.position + (navAgent.velocity * sightOffset), sightRadius);
    } 

    /*
     * Hi,

Regarding co-routines in Unity: It was my understanding that when a method is called (say method b) the method that called it (say method a) will remain on the stack until method b ends at which point method b will be removed from the stack, and once method a finishes it will also leave the stack (this is what I learned in C++, but my searches online thus far look like this is similar in c#).

In light of that, I have a few questions: If I call a method, say switchState, from a coroutine  (method a), and that method (switchState) in turn calls a different coroutine (method b) will all three methods be on the stack? Further, if I continue calling switchState from the coroutines and alternating back and forth calling method a and b, will all of those method calls stay on the stack? I am not completely clear on what happens with the caller method when the IEnumerator class does "yield return null;". Does the "yield return null" act like a regular return for the caller method and allow the caller method to finish running and be removed from the stack?

PsuedoCode to illustrate:

    switchstate(case)
    {
        case a:
        call method a;
        case b:
        call method b;
    }
    
    IEnumerator methodA()
    {
        while(condition is true)
        {
            //do things
            yield return null;
        }
        switchState(b);
    }
    
    IEnumerator methodB()
    {
        while(condition is true)
        {
            //do things
            yield return null;
        }
        switchState(a);
    }

    start()
    {
        switchState(a);
    }


I'm concerned that if I use this type of method switching I'll end up with an endlessly growing call stack. I'm not fully understanding how control is passed when using co-routines so any help understanding this would be greatly appreciated.
     * */

}
