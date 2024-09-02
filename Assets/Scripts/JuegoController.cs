using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 

public class JuegoController : MonoBehaviour
{
    public void IniciarJuego()
    {
        SceneManager.LoadScene("SimplePoly City - Low Poly Assets_Demo Scene");
    }
}
