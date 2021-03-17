using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelLights : MonoBehaviour
{
    [SerializeField] private GameObject stage1Lights;
    [SerializeField] private GameObject stage2Lights;

    private void Start()
    {
        stage2Lights.SetActive(false);
        GameManager.gm.OnStage2 += ToggleLights;
    }

    private void ToggleLights()
    {
        stage1Lights.SetActive(false);
        stage2Lights.SetActive(true);
    }
}
