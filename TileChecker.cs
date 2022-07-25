using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileChecker : MonoBehaviour
{
    public GameObject towerBody;
    public GameObject towerHead;
    public Transform raycasterPosition;
    public Material activeMaterial;
    public Material inActiveMaterial;
    public MeshRenderer bodyMeshRenderer;
    public MeshRenderer headMeshRenderer;
    public Tower towerScript;

    public void CheckTile()
    {
        int layerMask = 1 << 6;
        RaycastHit hit;
        
        if (Physics.Raycast(raycasterPosition.position, transform.TransformDirection(Vector3.down), out hit, Mathf.Infinity, layerMask))
        {
           //Debug.Log("downRay Tag = " + hit.transform.tag+"name "+ this.name);
            
            if (hit.transform.tag == "Grass")
            {
                if (bodyMeshRenderer !=activeMaterial)
                {
                    towerScript.canShoot = true;
                    bodyMeshRenderer.material = activeMaterial;
                    headMeshRenderer.material = activeMaterial;
                }
            }
            else if (hit.transform.tag == "Road")
            {
                if (bodyMeshRenderer !=inActiveMaterial) 
                {
                    towerScript.canShoot = false;
                    bodyMeshRenderer.material = inActiveMaterial;
                    headMeshRenderer.material = inActiveMaterial;
                }
                
            } 
        }
    }
}
