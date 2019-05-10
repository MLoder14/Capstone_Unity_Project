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
    private bool searchedAtLocation = false;
    [SerializeField] private bool attacking;
    private bool awareOfPlayer = false;
    private float elapsedTime;
    private Animator anim;
    private int patrolTargetNum;
    private Vector3 playerLastKnownPosition;

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
    public GameObject eyeDirection;

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
        if (awareOfPlayer && !searching && !attacking)
        {
            FaceTarget(playerLastKnownPosition);
        }
        if (attacking == true || searching == true)
        {
            navAgent.isStopped = true;
            navAgent.velocity = Vector3.zero;
        }
        if (true)
        {
            int checkReturn = checkInRange();
            if (checkReturn >= 2)
            {

                if (awareOfPlayer == false)
                {
                    awareOfPlayer = true;
                }
                if (!attacking)
                {
                    initAttack();
                    searchNeeded = true;
                }

            }
            else if (checkReturn == 1)
            {
                if (awareOfPlayer == false)
                {
                    awareOfPlayer = true;
                }
                if (!attacking)
                {
                    initPursuit();
                    searchNeeded = true;
                }
            }
            else if (pursuing == true)
            {
                if (searching == false)
                {

                    initSearch();
                }

            }
            else if (pursuing == false && !searching)
            {
                Debug.Log("Moving to last known location");
                //awareOfPlayer = false;
                if(awareOfPlayer == true)
                {
                    if (searchedAtLocation == false)
                    {
                        if (navAgent.destination != playerLastKnownPosition)
                        {
                            navAgent.SetDestination(playerLastKnownPosition);
                        }
                        if (navAgent.remainingDistance < patrolSwitchRange)
                        {
                            if (awareOfPlayer && searchedAtLocation == false)
                            {
                                Debug.Log("Searching at location");
                                searchedAtLocation = true;
                                initSearch();
                            }
                        }
                    }
                    else if (searchedAtLocation == true)
                    {
                        Debug.Log("Lost player, returning to patrol");
                        awareOfPlayer = false;
                        searchedAtLocation = false;
                        patrolTargetNum = (patrolTargetNum + 1) % patrolTargets.Length;
                        //navAgent.SetDestination(patrolTargets[patrolTargetNum].transform.position);
                        initPatrol();
                    }
                }
                else if (navAgent.remainingDistance < patrolSwitchRange)
                {
                    patrolTargetNum = (patrolTargetNum + 1) % patrolTargets.Length;
                    //navAgent.SetDestination(patrolTargets[patrolTargetNum].transform.position);
                    initPatrol();
                }
                
            }
            Debug.Log("searching: " + searching);
        }

        elapsedTime += Time.deltaTime;
        if (elapsedTime >= attackSpeed)
        {
            canAttack = true;
            elapsedTime = 0;
        }
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
                if (searching == true)
                {
                    searching = false;
                }
                Debug.Log("target in attack range.");
                return 2;
            }
            else if (dot > viewAngle && distance < maxRange && hit.transform.tag == "Player")
            {
                setLastKnownPosition();
                if (searching == true)
                {
                    searching = false;
                }
                Debug.Log("target in sight range.");
                return 1;
            }

            if (dot > viewAngle && distance > maxRange && searchNeeded)
                searchNeeded = false;
        }
        return 0;
    }

    void TrySearch()
    {
        searching = true;
        StartCoroutine("Search");
    }

    void initSearch()
    {
        pursuing = false;
        searching = true;
        setAllAnimParamsFalse();
        anim.SetBool("Search", true);
    }

    void endSearch()
    {
        Debug.Log("Ending search");
        searching = false;
    }

    void initAttack()
    {
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
        navAgent.isStopped = false;
    }

    private void setSpeedCreep()
    {
        GetComponent<NavMeshAgent>().speed = 3.0f;
    }

    private void setSpeedRun()
    {
        GetComponent<NavMeshAgent>().speed = 7.0f;
    }

    private void setLastKnownPosition()
    {
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
        Debug.Log("Attacking Player for " + attackPower + " damage");

        attacking = false;
    }

    private void OnDrawGizmosSelected()
    {
        // Drawing SightBox
        Gizmos.color = Color.white;
        Gizmos.DrawWireCube(transform.position + (navAgent.velocity * sightOffset), sightRadius);
    }

}
