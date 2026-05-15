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

        var current_rotation = gameObject.transform.rotation.eulerAngles;

        gameObject.transform.eulerAngles = new Vector3(
            current_rotation.x + rotate_x, 
            current_rotation.y + rotate_y, 
            current_rotation.z + rotate_z
            );
    }

    public void Randomize()
    {
        rotate_x = UnityEngine.Random.Range(0f, 12f);
        rotate_y = UnityEngine.Random.Range(0f, 12f);
        rotate_z = UnityEngine.Random.Range(0f, 12f);
    }
}
