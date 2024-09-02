using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using TMPro;

public class EnemigoController : MonoBehaviour
{
    public Animator ani;
    public NavMeshAgent agente;
    public float radio_vision = 10f; 
    public float speed = 3.5f;        

    public GameObject target;
    private Vector3 destinoAleatorio;
    private bool tieneDestino = false;

    public TMP_Text mensajeRaptado;  
    public Button botonReiniciar;

    void Start()
    {
        ani = GetComponent<Animator>();
        agente = GetComponent<NavMeshAgent>(); 
        agente.speed = speed; 

        mensajeRaptado.gameObject.SetActive(false);  
        botonReiniciar.gameObject.SetActive(false);
    }

    void Update()
    {
        target = GameObject.Find("cat"); 
        ComportamientoEnemigo();
    }

    public void ComportamientoEnemigo()
    {
        float distancia = Vector3.Distance(transform.position, target.transform.position);

        if (distancia > radio_vision)
        {
            ani.SetBool("run", false);
            ani.SetBool("walk", true);

            if (!tieneDestino || agente.remainingDistance < 0.5f) 
            {
                GenerarDestinoAleatorio();
            }
            agente.SetDestination(destinoAleatorio);

        }
        else
        {
            tieneDestino = false;

            if (agente.isOnNavMesh)
            {
                agente.SetDestination(target.transform.position);

                var lookPos = target.transform.position - transform.position;
                lookPos.y = 0;
                var rotation = Quaternion.LookRotation(lookPos);

                transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, 1);
                ani.SetBool("walk", false);
                ani.SetBool("run", true);
            }
            else
            {
                Debug.LogWarning("El agente no está en un NavMesh válido. Reposicionando...");
                NavMeshHit hit;
                if (NavMesh.SamplePosition(transform.position, out hit, 1.0f, NavMesh.AllAreas))
                {
                    transform.position = hit.position;
                    agente.Warp(hit.position);
                }
                else
                {
                    Debug.LogError("No se encontró un punto válido en el NavMesh cercano.");
                }
            }
        }
    }

    void GenerarDestinoAleatorio()
    {
        Vector3 randomDirection = Random.insideUnitSphere * radio_vision;
        randomDirection += transform.position;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomDirection, out hit, radio_vision, NavMesh.AllAreas))
        {
            destinoAleatorio = hit.position;
            tieneDestino = true;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "cat")
        {
            Time.timeScale = 0; 
            mensajeRaptado.text = "Has sido raptado"; 
            mensajeRaptado.gameObject.SetActive(true); 
            botonReiniciar.gameObject.SetActive(true);
        }
    }
}
