using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

public class PlayerMisc : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        other.GetComponent<IGrabable>().Grab();
    }
}
