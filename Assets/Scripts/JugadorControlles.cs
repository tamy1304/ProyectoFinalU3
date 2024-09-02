using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JugadorControlles : MonoBehaviour
{
    public float velocidadMov = 5.0f;
    public float velocidadRot = 200.0f;

    public Animator animator;
    public float x, y;

    public Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }
    void Update()
    {
        Mover();
    }

    public void Mover()
    {
        x = Input.GetAxis("Horizontal");
        y = Input.GetAxis("Vertical");

        transform.Rotate(0, x * Time.deltaTime * velocidadRot, 0);
        transform.Translate(0, 0, y * Time.deltaTime * velocidadMov);

        animator.SetFloat("VelX", x);
        animator.SetFloat("VelY", y);
    }


}