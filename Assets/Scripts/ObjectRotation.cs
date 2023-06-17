using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectRotation : MonoBehaviour
{
    public float rotationSpeed;
    public Vector3 rotationAxis;
    private bool _isRotating;

    public List<Vector3> listaNumeros;
    private void Update()
    {
        Rotate();
    }
    private void Rotate()
    {
        transform.Rotate(rotationSpeed * Time.deltaTime * rotationAxis);
    }
}
