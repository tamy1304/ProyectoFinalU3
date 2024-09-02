using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Necesario para manejar la UI
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public Rigidbody rb;
    public float vel;
    public float rotSpeed = 5f;
    public Animator anim;
    public Transform Eje;

    public bool tocaSuelo;
    private RaycastHit hit;
    public float distancia;
    public Vector3 v3;

    private Vector3 moveDirection = Vector3.zero;
    private Quaternion currentRotation;

    public TMP_Text mensajeCasa;  // Referencia al texto de la UI para mostrar el mensaje
    public Button botonReiniciar;

    void Start()
    {
        currentRotation = transform.rotation;
        mensajeCasa.gameObject.SetActive(false);  // Asegúrate de que el mensaje esté oculto al inicio
        botonReiniciar.gameObject.SetActive(false);
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawRay(transform.position + v3, Vector3.up * -1 * distancia);
    }

    void Mover()
    {
        moveDirection = Vector3.zero;
        if (Input.GetKey(KeyCode.W))
        {
            moveDirection += Eje.transform.forward;
        }
        if (Input.GetKey(KeyCode.S))
        {
            moveDirection -= Eje.transform.forward;
        }
        if (Input.GetKey(KeyCode.D))
        {
            moveDirection += Eje.transform.right;
        }
        if (Input.GetKey(KeyCode.A))
        {
            moveDirection -= Eje.transform.right;
        }

        moveDirection = moveDirection.normalized;

        Quaternion targetRotation = Quaternion.LookRotation(moveDirection);

        if (moveDirection != Vector3.zero)
        {
            currentRotation = Quaternion.Slerp(currentRotation, targetRotation, rotSpeed * Time.deltaTime);
            transform.rotation = currentRotation;
            var dir = moveDirection * vel * Time.fixedDeltaTime;
            dir.y = rb.velocity.y;
            rb.velocity = dir;
            anim.SetBool("walk", true);
        }
        else if (tocaSuelo)
        {
            rb.velocity = Vector3.zero;
            anim.SetBool("walk", false);
        }
    }

    void FixedUpdate()
    {
        Mover();
    }

    void Update()
    {
        if (Physics.Raycast(transform.position + v3, transform.up * -1, out hit, distancia))
        {
            if (hit.collider.tag == "piso")
            {
                tocaSuelo = true;
            }
        }
        else
        {
            tocaSuelo = false;
        }
    }

    // Detecta la colisión con la casa
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "casa")
        {
            Time.timeScale = 0;  // Pausa el juego
            mensajeCasa.text = "Has llegado a casa. ¡Felicidades!";  // Cambia el texto del mensaje
            mensajeCasa.gameObject.SetActive(true);  // Muestra el mensaje en pantalla
            botonReiniciar.gameObject.SetActive(true);
        }
    }

    public void ReiniciarJuego()
    {
        Time.timeScale = 1;  // Restablece el tiempo del juego
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);  // Reinicia la escena actual
    }
}
