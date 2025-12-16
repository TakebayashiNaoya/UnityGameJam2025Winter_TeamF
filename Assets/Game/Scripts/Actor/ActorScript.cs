using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ActorScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        InitializeObject();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateObject();
    }


    protected abstract void InitializeObject();


    protected abstract void UpdateObject();
}
