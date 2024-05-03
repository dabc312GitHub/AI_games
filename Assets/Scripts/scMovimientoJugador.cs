using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scMovimientoJugador : MonoBehaviour
{
    [SerializeField] private float velocidad = 0.1f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Movimiento del jugador
        if (Input.GetKey(KeyCode.W))
        {
            transform.Translate(Vector3.forward * velocidad);
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.Translate(Vector3.back * velocidad);
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.Translate(Vector3.left * velocidad);
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.Translate(Vector3.right * velocidad);
        }

        // Rotación del jugador
        if (Input.GetKey(KeyCode.Q))
        {
            transform.Rotate(Vector3.up, -1);
        }
        if (Input.GetKey(KeyCode.E))
        {
            transform.Rotate(Vector3.up, 1);
        }
        
        // Salto del jugador
        if (Input.GetKeyDown(KeyCode.Space))
        {
            transform.Translate(Vector3.up * 0.5f);
        }
    }
}
