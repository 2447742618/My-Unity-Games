using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemHold : MonoBehaviour
{
    private void OnEnable()
    {
        transform.position = Input.mousePosition;
    }

    private void Update()
    {
        transform.position = Input.mousePosition;
    }
}
