using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YG;

public class ControlProgress : MonoBehaviour
{
    public CheckPoint[] CheckpointArray;
    public int LastSave;
    public GameObject Player;

    private void Start()
    {
        YandexGame.GameReadyAPI();
        LastSave = Load();
        Spawn();
    }

    public void Save(int currentCheckPoint)
    {
        if (currentCheckPoint <= LastSave) return;
        LastSave = currentCheckPoint;
        YandexGame.savesData.LastSave = LastSave;
        YandexGame.SaveProgress();
    }

    public int Load()
    {
        return YandexGame.savesData.LastSave;
    }

    public void Spawn()
    {
        Instantiate(Player, CheckpointArray[LastSave].transform.position, CheckpointArray[LastSave].transform.rotation);
    }
}
