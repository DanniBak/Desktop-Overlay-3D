using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class RandomPosition : MonoBehaviour
{

    Vector3 initialPos;
    public int sphereMult = 10;

     // Update is called once per frame
    void Start()
    {
        initialPos = transform.position;
    }


    private void OnTriggerEnter()
    {
        transform.position = NewRandomPosition();
    }
    Vector3 NewRandomPosition()
    {
        return Random.insideUnitSphere * sphereMult + initialPos;
    }
}
