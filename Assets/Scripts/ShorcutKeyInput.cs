using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShorcutKeyInput : MonoBehaviour
{
    private bool isWalkableVisualize = false;
    private bool isGridLineShown = false;
    private bool isAStarVariableShown = false;

    private void Start()
    {
        //isWalkableVisualize = false;
        //isGridLineShown = false;
        //isAStarVariableShown = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            if (isWalkableVisualize)
            {
                VisualizationManager.Instance.ResetColor();
            }
            else
            {
                VisualizationManager.Instance.VisualizeWalkablePath();
            }

            isWalkableVisualize = !isWalkableVisualize;
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            isGridLineShown = !isGridLineShown;
            VisualizationManager.Instance.ShowGridLine(isGridLineShown);
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            isAStarVariableShown = !isAStarVariableShown;
            VisualizationManager.Instance.ShowAStarVariables(isAStarVariableShown);
        }
    }
}
