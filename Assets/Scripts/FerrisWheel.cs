using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

public class FerrisWheel : MonoBehaviour
{
    [SerializeField] private float rotateSpeed;
    void Update()
    {
        transform.Rotate(0, 0, rotateSpeed * Time.deltaTime);

        // for (int i = 0; i < transform.childCount; i++)
        // {
        //     transform.GetChild(i).transform.rotation = Quaternion.identity;
        // }
    }
}
