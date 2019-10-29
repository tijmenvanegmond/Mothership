using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PIDcontroller
{

    // The gains are chosen experimentally
    public float Kp = 1;
    public float Ki = 0;
    public float Kd = 0.1f;

    float prevError;
    float P, I, D;

    public float GetOutput(float currentError, float dt)
    {
        P = currentError;
        I += P * dt;
        D = (P - prevError) / dt;
        prevError = currentError;

        return P * Kp + I * Ki + D * Kd;
    }
}