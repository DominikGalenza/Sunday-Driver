using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{
    [SerializeField] private float speed = 15f;
    [SerializeField] private float acceleration = 0.1f;
    [SerializeField] private float turnSpeed = 150f;

    private int steerValue;

    void Update()
    {
        speed += acceleration * Time.deltaTime;
        transform.Rotate(0f, steerValue * turnSpeed * Time.deltaTime, 0f);
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    public void Steer(int value)
    {
        steerValue = value;
    }
}
