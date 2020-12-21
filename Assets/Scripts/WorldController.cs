using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldController : MonoBehaviour
{
    public float physicSteps = 0.01f;
    public float frameTime = 0.01f;

    void Start()
    {
        StartCoroutine(PhysicStepper());
    }

    private IEnumerator PhysicStepper()
    {
        while (true)
        {
            Physics.Simulate(physicSteps);
            yield return new WaitForSeconds(frameTime);
        }
    }
}
