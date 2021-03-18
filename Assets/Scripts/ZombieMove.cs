using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieMove : MonoBehaviour
{
    public float speed;
    public Player player;
    Rigidbody2D rb;
    Animator anim;


    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

    }
    private void Start()
    {
        player = FindObjectOfType<Player>();

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 zombiePoition = transform.position;
        Vector3 playerPosition = player.transform.position;

        Vector3 direction = playerPosition - zombiePoition;
        Move(direction);
        Rotate(direction);

    }

  

    void Move(Vector3 direction)
    {

        anim.SetFloat("speed", rb.velocity.magnitude);
        rb.velocity = direction * speed;



    }

    void Rotate(Vector3 direction)
    {
        
        direction.z = 0;
        transform.up = -direction;
    }
    private void OnDisable()
    {
        rb.velocity = Vector2.zero;
    }
}
