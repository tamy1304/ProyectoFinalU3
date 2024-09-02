using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camara : MonoBehaviour
{
    public GameObject target;  // El personaje al que sigue la c�mara
    public Vector3 offset;  // Offset de la c�mara con respecto al personaje

    // Start is called before the first frame update
    void Start()
    {
        // Posicionar la c�mara detr�s del personaje al inicio
        transform.position = target.transform.position + target.transform.TransformDirection(offset);
        transform.LookAt(target.transform);  // Asegura que la c�mara est� mirando al personaje
    }

    public void FollowPlayer()
    {
        if (target)
        {
            // Posiciona la c�mara detr�s del personaje sin demora
            transform.position = target.transform.position + target.transform.TransformDirection(offset);

            // Hace que la c�mara apunte al personaje
            transform.rotation = target.transform.rotation;
        }
    }

    // Update is called once per frame
    void LateUpdate()  // Usar LateUpdate para asegurarse de que la c�mara se actualiza despu�s de que el personaje se haya movido
    {
        FollowPlayer();
    }
}
