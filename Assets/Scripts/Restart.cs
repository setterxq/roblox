using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YG;

public class Restart : MonoBehaviour
{
    public ControlProgress controlProgress;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            YandexGame.savesData.LastSave = 0;
            Destroy(other.gameObject);
            controlProgress.LastSave = 0;
            controlProgress.Spawn(true);
        }
    }
}
