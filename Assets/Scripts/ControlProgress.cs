using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YG;

public class ControlProgress : MonoBehaviour
{
    public CheckPoint[] CheckpointArray;
    public int LastSave;
    public GameObject Player;
    public UIBehaviour uIBehaviour;

    private void Start()
    {
        YandexGame.GameReadyAPI();
        LastSave = Load();
        Spawn(true);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Pause))
        {
            LastSave = 0;
            YandexGame.savesData.LastSave = LastSave;
            YandexGame.SaveProgress();
        }
    }

    public void Save(int currentCheckPoint)
    {
        if (currentCheckPoint <= LastSave) return;
        LastSave = currentCheckPoint;
        YandexGame.savesData.LastSave = LastSave;
        YandexGame.SaveProgress();
        uIBehaviour.ChangeProgressbar(LastSave/ 73f);
    }

    public void InvisibleCursor()
    {
        if (YandexGame.EnvironmentData.isDesktop)
        {
            StartCoroutine(Lock());
        }
    }

    private IEnumerator Lock()
    {
        yield return new WaitForSeconds(0.1f);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public int Load()
    {
        return YandexGame.savesData.LastSave;
    }

    public void Spawn(bool isDead)
    {
        if (isDead)
        {
            Instantiate(Player, CheckpointArray[LastSave].transform.position, CheckpointArray[LastSave].transform.rotation);
        }
    }
}
