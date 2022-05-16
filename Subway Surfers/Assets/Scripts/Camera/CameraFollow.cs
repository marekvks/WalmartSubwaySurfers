using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Vector3 offset;
    [SerializeField] private float smoothTime = 10f;
    private void Update()
    {
        transform.position = (Vector3.Lerp(transform.position, player.position + offset, smoothTime * Time.deltaTime));
    }
}
