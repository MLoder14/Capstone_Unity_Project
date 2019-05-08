using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ai_controller : MonoBehaviour
{
    [SerializeField] private NavMeshAgent navAgent = null;
    [SerializeField] private GameObject target = null;
    private bool canAttack;
    private bool searching = false;
    private bool searchNeeded = false;
    private bool pursuing;
    private bool attacking;
    private float elapsedTime;
    private Animator anim;
    private int patrolTargetNum;

    public float rotationSpeed;
    public float attackSpeed;
    public Transform eyePosition;
    //public GameObject attackObject;
    //public Transform spawnZone;
    public float attackRange;
    public float maxRange;
    public float attackAngle;
    public float viewAngle;
    public float patrolSwitchRange;
    public GameObject[] patrolTargets;

    [Header("Sight Variables")]
    public LayerMask playerLayer;
    public LayerMask wallLayer;
    [SerializeField] private Vector3 sightRadius = Vector3.zero;
    [SerializeField] private float sightOffset = 0f;

    [Header("Combat Variables")]
    [SerializeField] private float attackPower = 10f;

    void Start()
    {
        patrolTargetNum = 0;
        anim = GetComponent<Animator>();
        canAttack = true;
        pursuing = false;
        attacking = false;
        initPatrol();
        //setAllAnimParamsFalse();
        //anim.SetBool("Creep", true);
        //setSpeedCreep();   
    }

    private void setSpeedCreep()
    {
        GetComponent<NavMeshAgent>().speed = 3.0f;
    }

    private void setSpeedRun()
    {
        GetComponent<NavMeshAgent>().speed = 7.0f;
    }

    private void setAllAnimParamsFalse()
    {
        anim.SetBool("Walk", false);
        anim.SetBool("Creep", false);
        anim.SetBool("Run", false);
        anim.SetBool("Attack", false);
        anim.SetBool("Idle", false);
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
    /// checks if the target (set as a public game object) is within range and within viewing angle
    /// returns 2 if player is within attack range and within attackAngle (player directly-ish in front of enemy)
    /// returns 1 if player is within max range and viewAngle (player spotted)
    /// 0 if none of those conditions are true
    /// </summary>
    /// <returns></returns>
    int checkInRange()
    {
        //Debug.Log("Checking target range.");
        Vector3 heading = (target.transform.position - transform.position).normalized;
        RaycastHit hit;
        
        float dot = Vector3.Dot(transform.forward, heading);
        float distance = Vector3.Distance(transform.position, target.transform.position);
        Debug.DrawRay(eyePosition.position, (target.transform.position - eyePosition.position));
        if(Physics.Raycast(eyePosition.position, (target.transform.position - eyePosition.position), out hit, maxRange))
        if (dot > attackAngle && distance < attackRange && hit.transform.tag == "Player")
        {
            //Debug.Log("target in attack range.");
            return 2;
        }
        else if (dot > viewAngle && distance < maxRange && hit.transform.tag == "Player")
        {
            //Debug.Log("target in sight range.");
            return 1;
        }

        if (dot > viewAngle && distance > maxRange && searchNeeded)
            searchNeeded = false;

        return 0;
    }

    /// <summary>
    /// calls check return to check if player is within range or can be seen
    /// check return is 2 (player is directly-ish in front of enemy) stops ai, and starts attacking
    /// check return is 1 sets navagent destination to player and begins pursuit, calls face player, sets pursuing true
    /// check return 0, pursuing true player out of range or view, resets destination to last patrol point
    /// pursuing false cycles to next patrol point when ai gets close enough
    /// tracks timing of attacks to limit attack speed
    /// </summary>
    void Update()
    {
        if (!searching)
        {
            int checkReturn = checkInRange();
            if (checkReturn >= 2)
            {
                initAttack();
                /*navAgent.isStopped = true;
                navAgent.velocity = Vector3.zero;
                //anim.SetBool("Run", false);
                setAllAnimParamsFalse();
                if (canAttack == true)
                {
                    anim.SetBool("Attack", true);
                    canAttack = false;
                    elapsedTime = 0;
                }*/

                searchNeeded = true;
            }
            else if (checkReturn >= 1 && pursuing == false)
            {
                initPursuit();
                /*navAgent.SetDestination(target.transform.position);
                pursuing = true;
                if (attacking == false)
                {
                    navAgent.isStopped = false;
                    setSpeedCreep();
                    setAllAnimParamsFalse();
                    anim.SetBool("Run", true);
                    //anim.SetBool("Attack", false);
                    FaceTarget(target.transform.position);
                }*/
                searchNeeded = true;

            }
            else if (pursuing == true)
            {
                /*//Debug.Log("lost target");
                pursuing = false;
                navAgent.SetDestination(patrolTargets[patrolTargetNum].transform.position);
                setSpeedRun();
                setAllAnimParamsFalse();
                anim.SetBool("Run", true);
                //anim.SetBool("Attack", false);
                navAgent.isStopped = false;*/
                initPatrol();
            }
            else if (pursuing == false && !searching)
            {
                if (searchNeeded)
                {
                    TrySearch();
                    searchNeeded = false;

                    return;
                }

                if (navAgent.remainingDistance < patrolSwitchRange)
                {
                    patrolTargetNum = (patrolTargetNum + 1) % patrolTargets.Length;
                    navAgent.SetDestination(patrolTargets[patrolTargetNum].transform.position);
                }
            }
        }

        elapsedTime += Time.deltaTime;
        if(elapsedTime >= attackSpeed)
        {
            canAttack = true;
            elapsedTime = 0;
        }
    }

    void TrySearch()
    {
        searching = true;
        StartCoroutine("Search");
    }

    void ReturnToPatrol()
    {
        if (pursuing == true)
        {
            //Debug.Log("lost target");
            pursuing = false;
            navAgent.SetDestination(patrolTargets[patrolTargetNum].transform.position);
            setAllAnimParamsFalse();
            anim.SetBool("Run", true);
            //anim.SetBool("Attack", false);
            navAgent.isStopped = false;
        }
        else if (pursuing == false && !searching)
        {
            if (navAgent.remainingDistance < patrolSwitchRange)
            {
                patrolTargetNum = (patrolTargetNum + 1) % patrolTargets.Length;
                navAgent.SetDestination(patrolTargets[patrolTargetNum].transform.position);
            }
        }
    }

    IEnumerator Search()
    {
        // Rotate Around until you find the player or you finish rotating
        float dur = 0.5f;
        float delay = 0.01f;
        int reps = (int)(dur / delay);
        Vector3 rotationInc = new Vector3(0f, 360f / reps, 0f);

        for(int i = 0; i < reps; i++)
        {
            // Rotating The Player
            //transform.Rotate(rotationInc, Space.Self);

            int checkReturn = checkInRange();
            if (checkReturn >= 2)
            {
                /*navAgent.isStopped = true;
                navAgent.velocity = Vector3.zero;
                //anim.SetBool("Run", false);
                setAllAnimParamsFalse();
                if (canAttack == true)
                {
                    anim.SetBool("Attack", true);
                    canAttack = false;
                    elapsedTime = 0;
                }*/
                initAttack();

                searchNeeded = true;
                break;
            }
            else if (checkReturn >= 1)
            {
                initPursuit();
                /*navAgent.SetDestination(target.transform.position);
                pursuing = true;
                if (attacking == false)
                {
                    navAgent.isStopped = false;
                    setAllAnimParamsFalse();
                    anim.SetBool("Run", true);
                    //anim.SetBool("Attack", false);
                    FaceTarget(target.transform.position);
                }*/
                break;
            }

            yield return new WaitForSeconds(delay);
        }
        /*
        searching = false;
        if (navAgent.remainingDistance < patrolSwitchRange)
        {
            patrolTargetNum = (patrolTargetNum + 1) % patrolTargets.Length;
            navAgent.SetDestination(patrolTargets[patrolTargetNum].transform.position);
        }
        */

        searching = false;
    }

    void initAttack()
    {
        navAgent.isStopped = true;
        navAgent.velocity = Vector3.zero;
        //anim.SetBool("Run", false);
        setAllAnimParamsFalse();
        if (canAttack == true)
        {
            anim.SetBool("Attack", true);
            canAttack = false;
            elapsedTime = 0;
        }
    }
    
    void initPursuit()
    {
        navAgent.SetDestination(target.transform.position);
        pursuing = true;
        if (attacking == false)
        {
            navAgent.isStopped = false;
            setAllAnimParamsFalse();
            setSpeedRun();
            anim.SetBool("Run", true);
            //anim.SetBool("Attack", false);
            FaceTarget(target.transform.position);
        }
    }

    void initPatrol()
    {
        //Debug.Log("lost target");
        pursuing = false;
        navAgent.SetDestination(patrolTargets[patrolTargetNum].transform.position);
        setSpeedCreep();
        setAllAnimParamsFalse();
        anim.SetBool("Creep", true);
        //anim.SetBool("Attack", false);
        navAgent.isStopped = false;
    }

    void attackStart()
    {
        attacking = true;
    }

    void attackEnd()
    {
        Debug.Log("Attacking Player for " + attackPower + " damage");

        // Damaging the Player
        //PlayerCombat pControl = target.GetComponent<PlayerCombat>();
        //pControl.TakeDamage(attackPower);
        attacking = false;
    }

    private void OnDrawGizmosSelected()
    {
        // Drawing SightBox
        Gizmos.color = Color.black;
        Gizmos.DrawWireCube(transform.position + (navAgent.velocity * sightOffset), sightRadius);
    }
}
