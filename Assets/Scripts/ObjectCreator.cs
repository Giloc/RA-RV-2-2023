using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class ObjectCreator : MonoBehaviour
{
    public ARRaycastManager raycastManager;
    private List<ARRaycastHit> hits = new List<ARRaycastHit>();
    public GameObject prefab;
    public Camera arCam;
    private GameObject createdObject;

    private void Start()
    {
        createdObject = null;
    }

    private void Update()
    {
        CreateObject();
    }

    private void CreateObject()
    {
        if (Input.touchCount == 0) 
        {
            return;
        }

        RaycastHit hit;
        Ray ray = arCam.ScreenPointToRay(Input.GetTouch(0).position);

        if (raycastManager.Raycast(Input.GetTouch(0).position, hits))
        {
            if (Input.GetTouch(0).phase == TouchPhase.Began && createdObject == null)
            {
                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.collider.gameObject.CompareTag("Createable"))
                    {
                        createdObject = hit.collider.gameObject;
                    }
                    else
                    {
                        createdObject = Instantiate(prefab, hits[0].pose.position, Quaternion.identity);
                    }
                }
            }
            
            else if (Input.GetTouch(0).phase == TouchPhase.Ended)
            {
                createdObject = null;
            }
        }

    }
}
