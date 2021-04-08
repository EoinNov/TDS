using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieMove : MonoBehaviour
{
    public float speed;
    public Vector3 targetPosition;
    Rigidbody2D rb;
    Animator anim;


    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

    }
  

    // Update is called once per frame
    void Update()
    {
        Vector3 zombiePoition = transform.position;

        Vector3 direction = targetPosition - zombiePoition;
        Move(direction);
        Rotate(direction);

    }

  

    void Move(Vector3 direction)
    {

        anim.SetFloat("speed", rb.velocity.magnitude);
        rb.velocity = direction.normalized * speed;



    }

    void Rotate(Vector3 direction)
    {
        
        direction.z = 0;
        transform.up = -direction;
    }
    private void OnDisable()
    {
        
        rb.velocity = Vector2.zero;
        anim.SetFloat("speed", rb.velocity.magnitude);

    }
}
