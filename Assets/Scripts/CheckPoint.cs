using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    public int Number;
    public ControlProgress ControlProgressScript;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            Save();
        }
    }

    private void Save()
    {
        ControlProgressScript.Save(Number);
    }
}
