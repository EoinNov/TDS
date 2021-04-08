using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Zombie : MonoBehaviour
{
    public float followDistance;
    public float attackDistance;
    public int zombieHeals;
    Animator anim;
    float distance;
    public bool isSleep;
    float viewRotate = 80 ;

    public Action onCheckHeals;

    Vector2 startPosition;
    Player player;
    ZombieMove movement;

    public GameObject[] wayPoint;
    System.Random rnd;
    ZombieState activeState;

    enum ZombieState
    {
        STAND,
        MOVE,
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
        rnd = new System.Random();
        startPosition = transform.position;
        player = FindObjectOfType<Player>();
        activeState = ZombieState.STAND;
        StartCoroutine(Patruling());
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        print("Damage!");
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
                    ChangeState(ZombieState.MOVE);
                }
                //check field of view
                break;
            case ZombieState.MOVE:
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
                    ChangeState(ZombieState.MOVE);
                }
               
                float distanceToPoint = Vector2.Distance(transform.position, startPosition);
                if (distanceToPoint <= 0.1)
                {
                    ChangeState(ZombieState.STAND);
                }
                break;
            case ZombieState.ATTACK:
                if (distance > attackDistance)
                {
                    
                    ChangeState(ZombieState.MOVE);
                }
                Rotate();

            
                    anim.SetTrigger("shoot");


               

                break;
        }
    }
    private IEnumerator Patruling()
    {
        while(true)
        {
            yield return new WaitForSeconds(15);

            if (movement.targetPosition != player.transform.position)
            {
                int point = rnd.Next(0, wayPoint.Length);
                startPosition = wayPoint[point].transform.position;
                ChangeState(ZombieState.RETURN);
            }
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
            case ZombieState.RETURN:
                movement.targetPosition =startPosition;
                movement.enabled = true;

                break;
            case ZombieState.MOVE:
               
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
  

    public void DoDamage()
    {
        if (distance < attackDistance)
        {
            print("Damage");
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "bullet")
        {
            onCheckHeals();
            zombieHeals--;
        }
    }
}
