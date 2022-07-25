//Adapted from: https://gamedevbeginner.com/how-to-snap-objects-in-game-in-unity/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerSnapToPosition : MonoBehaviour
{
    public float snapDistance = 0.05f;
    public List<Transform> nodes = new List<Transform>();

    Vector3 currentPosition1;
    Vector3 closestPosition1;
    Vector3 currentPosition2;
    Vector3 closestPosition2;
    Vector3 currentPosition3;
    Vector3 closestPosition3;
    Vector3 currentPosition4;
    Vector3 closestPosition4;

    Quaternion closestRotation1;
    Quaternion closestRotation2;
    Quaternion closestRotation3;
    Quaternion closestRotation4;
    
    
    public Transform tower1Transform;
    public Transform tower1NewPosition;
    public Transform tower2Transform;
    public Transform tower2NewPosition;
    public Transform tower3Transform;
    public Transform tower3NewPosition;
    public Transform tower4Transform;
    public Transform tower4NewPosition;

    private void Start()
    {
      
    }
    void Update()
    {
        float smallestDistance1 = snapDistance;
        float smallestDistance2 = snapDistance; 
        float smallestDistance3 = snapDistance;
        float smallestDistance4 = snapDistance;
        //for tower 1
        currentPosition1=tower1Transform.position;
        foreach (Transform node in nodes)
        {
            if (Vector3.Distance(node.position, currentPosition1) < smallestDistance1)
            {
                closestPosition1 = node.position;
                closestRotation1 = node.rotation;
                smallestDistance1 = Vector3.Distance(node.position, currentPosition1);
                closestPosition1.y += 0.003f;
              // Debug.Log("new closest position1");
            }
        }
        tower1NewPosition.position = closestPosition1;
        tower1NewPosition.rotation = closestRotation1;
        tower1NewPosition.GetComponent<TileChecker>().CheckTile();

        //for tower 2
        currentPosition2=tower2Transform.position;
        foreach (Transform node in nodes)
        {
            if (Vector3.Distance(node.position, currentPosition2) < smallestDistance2)
            {
                closestPosition2 = node.position;
                closestRotation2 = node.rotation;
                smallestDistance2 = Vector3.Distance(node.position, currentPosition2);
                closestPosition2.y += 0.003f;
               //Debug.Log("new closest position2");
            }
        }
        tower2NewPosition.position = closestPosition2;
        tower2NewPosition.rotation = closestRotation2;
        tower2NewPosition.GetComponent<TileChecker>().CheckTile();

        //for tower 3
        currentPosition3=tower3Transform.position;
        foreach (Transform node in nodes)
        {
            if (Vector3.Distance(node.position, currentPosition3) < smallestDistance3)
            {
                closestPosition3 = node.position;
                closestRotation3 = node.rotation;
                smallestDistance3 = Vector3.Distance(node.position, currentPosition3);
                closestPosition3.y += 0.003f;
              // Debug.Log("new closest position3");
            }
        }
        tower3NewPosition.position = closestPosition3;
        tower3NewPosition.rotation = closestRotation3;
        tower3NewPosition.GetComponent<TileChecker>().CheckTile();

        //for tower 4
        currentPosition4=tower4Transform.position;
        foreach (Transform node in nodes)
        {
            if (Vector3.Distance(node.position, currentPosition4) < smallestDistance4)
            {
                closestPosition4 = node.position;
                closestRotation4 = node.rotation;
                smallestDistance4 = Vector3.Distance(node.position, currentPosition4);
                closestPosition4.y += 0.003f;
              // Debug.Log("new closest position 4");
            }
        }
        tower4NewPosition.position = closestPosition4;
        tower4NewPosition.rotation = closestRotation4;
        tower4NewPosition.GetComponent<TileChecker>().CheckTile();

    }
}
