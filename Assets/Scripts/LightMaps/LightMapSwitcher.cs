using UnityEngine;

using System.Linq;

public class LightMapSwitcher : MonoBehaviour
{
    public Texture2D[] Stage1Near;
    public Texture2D[] Stage1Far;
    public Texture2D[] Stage2Near;
    public Texture2D[] Stage2Far;

    private LightmapData[] stage1LightMaps;
    private LightmapData[] stage2LightMaps;

    void Start()
    {
        GameManager.gm.OnStage2 += SetToStage2;

        if ((Stage1Near.Length != Stage1Far.Length) || (Stage2Near.Length != Stage2Far.Length))
        {
            Debug.Log("In order for LightMapSwitcher to work, the Near and Far LightMap lists must be of equal length");
            return;
        }

        // Sort the Day and Night arrays in numerical order, so you can just blindly drag and drop them into the inspector
        Stage1Near = Stage1Near.OrderBy(t2d => t2d.name, new NaturalSortComparer<string>()).ToArray();
        Stage1Far = Stage1Far.OrderBy(t2d => t2d.name, new NaturalSortComparer<string>()).ToArray();
        Stage2Near = Stage2Near.OrderBy(t2d => t2d.name, new NaturalSortComparer<string>()).ToArray();
        Stage2Far = Stage2Far.OrderBy(t2d => t2d.name, new NaturalSortComparer<string>()).ToArray();

        // Put them in a LightMapData structure
        stage1LightMaps = new LightmapData[Stage1Near.Length];
        for (int i = 0; i < Stage1Near.Length; i++)
        {
            stage1LightMaps[i] = new LightmapData();
            stage1LightMaps[i].lightmapDir = Stage1Near[i];
            stage1LightMaps[i].lightmapColor = Stage1Far[i];
        }

        stage2LightMaps = new LightmapData[Stage2Near.Length];
        for (int i = 0; i < Stage2Near.Length; i++)
        {
            stage2LightMaps[i] = new LightmapData();
            stage2LightMaps[i].lightmapDir = Stage2Near[i];
            stage2LightMaps[i].lightmapColor = Stage2Far[i];
        }

        // Set Stage 1
        SetToStage1();
    }

    #region Publics
    public void SetToStage1()
    {
        LightmapSettings.lightmaps = stage1LightMaps;
    }

    public void SetToStage2()
    {
        LightmapSettings.lightmaps = stage2LightMaps;
    }
    #endregion

    #region Debug
    [ContextMenu("Set to Night")]
    void Debug00()
    {
        SetToStage2();
    }

    [ContextMenu("Set to Day")]
    void Debug01()
    {
        SetToStage1();
    }
    #endregion
}