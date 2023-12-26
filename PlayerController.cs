using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float runSpeed;


    Rigidbody2D m_Rigidbody;
    Vector2 m_Movement;

    void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        run();
    }


    void run()
    {
        float horizintal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");


        m_Movement = new Vector2(horizintal * runSpeed, vertical * runSpeed);

        m_Rigidbody.velocity = m_Movement;
    }
}