using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using TMPro;

public class EnemigoController : MonoBehaviour
{
    public int rutina;
    public float cronometro;
    public Animator ani;
    public Quaternion angulo;
    public float grado;

    public NavMeshAgent agente;
    public float radio_vision = 10f;  // Radio de visión del enemigo
    public float speed = 3.5f;        // Velocidad del agente

    public GameObject target;
    private Vector3 destinoAleatorio;
    private bool tieneDestino = false;

    public TMP_Text mensajeRaptado;  // Referencia al texto de la UI para mostrar el mensaje
    public Button botonReiniciar;

    void Start()
    {
        ani = GetComponent<Animator>();
        agente = GetComponent<NavMeshAgent>();  // Inicializa el NavMeshAgent
        agente.speed = speed;  // Asigna la velocidad del agente

        mensajeRaptado.gameObject.SetActive(false);  // Asegúrate de que el mensaje esté oculto al inicio
        botonReiniciar.gameObject.SetActive(false);
    }

    void Update()
    {
        target = GameObject.Find("cat");  // Asigna el objetivo en cada actualización
        ComportamientoEnemigo();
    }

    public void ComportamientoEnemigo()
    {
        float distancia = Vector3.Distance(transform.position, target.transform.position);

        if (distancia > radio_vision)
        {
            ani.SetBool("run", false);
            ani.SetBool("walk", true);

            if (!tieneDestino || agente.remainingDistance < 0.5f) // Si no tiene destino o ya ha llegado al destino anterior
            {
                GenerarDestinoAleatorio();
            }

            agente.SetDestination(destinoAleatorio); // Mover al enemigo hacia el destino aleatorio

        }
        else
        {
            tieneDestino = false; // Resetea el destino aleatorio cuando el jugador está en rango

            if (agente.isOnNavMesh)
            {
                agente.SetDestination(target.transform.position);  // Establece el destino hacia el target

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

                // Intenta reposicionar el agente en un punto válido en el NavMesh
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

    // Detecta la colisión con el jugador
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "cat")
        {
            Time.timeScale = 0;  // Pausa el juego
            mensajeRaptado.text = "Has sido raptado";  // Cambia el texto del mensaje
            mensajeRaptado.gameObject.SetActive(true);  // Muestra el mensaje en pantalla
            botonReiniciar.gameObject.SetActive(true);
        }
    }
}
