using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animal : MonoBehaviour
{
    public Vector3 move;
    public Quaternion rot;

    public void RandomMove()
    {
        transform.position = move;
        transform.rotation = rot;
    }
}
