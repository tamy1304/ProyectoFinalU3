using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camara : MonoBehaviour
{
    public GameObject target;  // El personaje al que sigue la cámara
    public Vector3 offset;  // Offset de la cámara con respecto al personaje

    // Start is called before the first frame update
    void Start()
    {
        // Posicionar la cámara detrás del personaje al inicio
        transform.position = target.transform.position + target.transform.TransformDirection(offset);
        transform.LookAt(target.transform);  // Asegura que la cámara esté mirando al personaje
    }

    public void FollowPlayer()
    {
        if (target)
        {
            // Posiciona la cámara detrás del personaje sin demora
            transform.position = target.transform.position + target.transform.TransformDirection(offset);

            // Hace que la cámara apunte al personaje
            transform.rotation = target.transform.rotation;
        }
    }

    // Update is called once per frame
    void LateUpdate()  // Usar LateUpdate para asegurarse de que la cámara se actualiza después de que el personaje se haya movido
    {
        FollowPlayer();
    }
}
