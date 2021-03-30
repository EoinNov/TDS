using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Zombie : MonoBehaviour
{
    public float followDistance;
    public float attackDistance;
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
        MOVE,
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
        onCheckHeals += heal;
        player = FindObjectOfType<Player>();
        activeState = ZombieState.STAND;
        
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
                if (distance <= attackDistance)
                {
                    ChangeState(ZombieState.ATTACK);
                }
                else if (distance >= followDistance)
                {
                    ChangeState(ZombieState.STAND);
                }
                Rotate();
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
    private void ChangeState(ZombieState newState)
    {
        activeState = newState;
        switch (activeState)
        {
            case ZombieState.STAND:
                movement.enabled = false;
                //movement.StopMovement();
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
    void heal()
    {
        print("Жизни Зомби");
    }
    public void DoDamage()
    {
        if (distance < attackDistance)
        {
            print("Damage");
        }
    }
    }
