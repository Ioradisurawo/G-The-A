using System;
using System.Collections.Generic;
using System.Text;
using Unity.Mathematics;
using UnityEngine;

public class Rotation : MonoBehaviour
{
    [SerializeField]
    private float rotate_x = 0;
    [SerializeField]
    private float rotate_y = 0;
    [SerializeField]
    private float rotate_z = 0;

    public bool isRotating = false;

    private void Update()
    {
        if (!isRotating)
            return;
        transform.Rotate(new Vector3(
            rotate_x,
            rotate_y,
            rotate_z
            ) * Time.deltaTime);
    }

    public void Randomize()
    {
        float max = 180f;
        rotate_x = UnityEngine.Random.Range(0f, max);
        rotate_y = UnityEngine.Random.Range(0f, max);
        rotate_z = UnityEngine.Random.Range(0f, max);
    }
}
