using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Zombie : MonoBehaviour
{
    public float followDistance;
    public float attackDistance;
    Vector3 startPosition;

    public GameObject[] wayPoints;
    System.Random rdn;

    [Header("ZombieParam")]
    public int zombieHealth = 10;

    Animator anim;
    float distance;
    public bool isSleep;
    float viewRotate = 80 ;


    public Action onCheckHeals;
   

    Player player;
    ZombieMove movement;


    ZombieState activeState;

    enum ZombieState
    {
        STAND,
        MOVEtoPlayer,
        RETURN,
        ATTACK
    }


    private void Awake()
    {
        anim = GetComponent<Animator>();
        movement = GetComponent<ZombieMove>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
        rdn = new System.Random();
        startPosition = transform.position;
        
        player = FindObjectOfType<Player>();
        //activeState = ZombieState.STAND;
        StartCoroutine(PatrolZombie());
        
    }



    // Update is called once per frame
    void FixedUpdate()
    {
        
        distance = Vector2.Distance(transform.position, player.transform.position);
        UpdateState();






    }
    void UpdateState()
    {
      

       

        switch (activeState)
        {
            case ZombieState.STAND:

                if (CheckToMove())
                {
                    ChangeState(ZombieState.MOVEtoPlayer);
                }
                //check field of view
                break;
            case ZombieState.MOVEtoPlayer:
                movement.targetPosition = player.transform.position;
                if (distance <= attackDistance)
                {
                    ChangeState(ZombieState.ATTACK);
                }
                else if (distance >= followDistance)
                {
                    ChangeState(ZombieState.RETURN);
                }
                Rotate();
                break;
            case ZombieState.RETURN:
                if (CheckToMove())
                {
                    ChangeState(ZombieState.MOVEtoPlayer);
                }
                float distanseToPoint = Vector2.Distance(transform.position, startPosition);
                if (distanseToPoint <= 0.01)
                {
                    ChangeState(ZombieState.STAND);
                }
                break;
            case ZombieState.ATTACK:
                if (distance > attackDistance)
                {
                    
                    ChangeState(ZombieState.MOVEtoPlayer);
                }
                Rotate();

            
                    anim.SetTrigger("shoot");

               


                break;
        }
    }
    private void ChangeState(ZombieState newState)
    {
        activeState = newState;
        switch (activeState)
        {
            case ZombieState.STAND:
                movement.enabled = false;
                //movement.StopMovement();
                break;
            case ZombieState.MOVEtoPlayer:
             
                movement.enabled = true;
                break;
            case ZombieState.RETURN:

                movement.targetPosition = startPosition;
                movement.enabled = true;
                break;
            case ZombieState.ATTACK:
                onCheckHeals();
                movement.enabled = false;
                // movement.StopMovement();
                break;
        }
    }
    void Rotate()
    {
        Vector2 direction = player.transform.position - transform.position;
        transform.up = -direction;
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;

        Gizmos.DrawWireSphere(transform.position, followDistance);

        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(transform.position, attackDistance);

    }
    private bool CheckToMove()
    {
        
       
        if (distance <= followDistance)
        {
            LayerMask layerMask = LayerMask.GetMask("Walls");
            Vector2 direction = player.transform.position - transform.position;
            float angle = Vector3.Angle(-transform.up, direction);
            if (angle > viewRotate / 2) 
            {
                return false;
            }
            Debug.DrawRay(transform.position, direction, Color.red);
            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, distance, layerMask);

            if (hit.collider == null)
            {
                return true;
            }
        }
        return false;
    }


    IEnumerator PatrolZombie()
    {
        while (true)
        {
            yield return new WaitForSeconds(22);
            if (movement.targetPosition != player.transform.position)
            {
                int point = rdn.Next(0, wayPoints.Length);
                startPosition = wayPoints[point].transform.position;
                ChangeState(ZombieState.RETURN);
            }
        }

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "bullet")
        {
            onCheckHeals();
            zombieHealth--;
        }
    }
    public void DoDamage()
    {
        if (distance < attackDistance)
        {
            print("Damage");
        }
    }
    }
