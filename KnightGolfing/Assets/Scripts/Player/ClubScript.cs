using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClubScript : MonoBehaviour
{
    public GameObject standardMesh;
    GameObject myMesh;

    private void Start()
    {
        if (transform.GetChild(0).childCount > 0) { Destroy(transform.GetChild(0).GetChild(0).gameObject); }
        myMesh = Instantiate(standardMesh, transform.GetChild(0));
    }

    public void SetMesh(GameObject mesh)
    {
        Destroy(myMesh);
        myMesh = Instantiate(mesh, transform.GetChild(0));
    }
}
