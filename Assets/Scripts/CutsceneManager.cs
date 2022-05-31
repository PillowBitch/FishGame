using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CutsceneManager : MonoBehaviour
{
    public static CutsceneManager instance;

    [Header("Level Data")]
    public int nextLevelIndex = -1;
    public bool isCutscene = true;

    [Header("Camera Settings")]
    public Camera levelCamera;
    public Camera editingCamera;
    public float cameraSize = 5;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        if (isCutscene)
            GameManager.ChangeGameState(GameState.Cutscene);

        if (editingCamera != null)
            editingCamera.gameObject.SetActive(false);

        levelCamera = GameManager.instance.mainCamera;
        SetupCamera();

    }

    private void Update()
    {
        if (Input.anyKeyDown && nextLevelIndex > 0)
        {
            NextLevel();
        }
    }


    public void SetupCamera()
    {
        levelCamera.orthographicSize = cameraSize;
    }

    public void NextLevel()
    {
        if (nextLevelIndex > 0)
        {
            GameManager.instance.LoadSceneTransition(nextLevelIndex);
        }
        else
        {
            Debug.LogError("Invalid index: " + nextLevelIndex);
        }
    }

    private void OnDrawGizmosSelected()
    {
        editingCamera.orthographicSize = cameraSize;
    }
}
