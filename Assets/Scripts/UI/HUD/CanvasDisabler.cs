using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasDisabler : MonoBehaviour
{
    [SerializeField] GameObject canvas;
    bool isActive=false;

    // Start is called before the first frame update
    void Start()
    {
        canvas.SetActive(false);
    }

    public void SetActiveCanvas()
    {
        isActive = true;
        canvas.SetActive(isActive);
    }
    
    public void SetDisableCanvas()
    {
        isActive=false;
        canvas.SetActive(isActive);
    }

}
