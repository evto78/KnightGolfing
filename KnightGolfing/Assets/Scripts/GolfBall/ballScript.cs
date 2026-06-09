using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEditor.UI;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.WSA;

public class ballScript : MonoBehaviour
{
    [SerializeField]Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void Launch(float _magnitude, Vector3 _forceDir)
    {
        rb.AddForce(_magnitude * _forceDir, ForceMode.Impulse);
    }
}
