using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrainWithMemory : MonoBehaviour
{
    public DNAWITHMEMORY dnaG;

    private Renderer renderer;

    public void Init()
    {
        dnaG = new DNAWITHMEMORY();
        renderer = GetComponent<Renderer>();

    }
    private void Start()
    {
        ChangeColor()
            ;

    }
    private void LateUpdate()
    {
       
    }
    public void ChangeColor()
    {
        renderer = GetComponent<Renderer>();
        renderer.material.color = dnaG.color;
    }
}