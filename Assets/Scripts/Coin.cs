using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    void Update()
    {
        RotationCoin();
    }

    void RotationCoin()
    {
        transform.Rotate(0, 40 * Time.deltaTime, 0);
    }
}
