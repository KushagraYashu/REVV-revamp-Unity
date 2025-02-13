using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Path : MonoBehaviour
{
    public Color lineColor;
    public List<Transform> nodes = new List<Transform>();

    private void OnDrawGizmos()
    {
        Gizmos.color = lineColor;

        Transform[] pathTransforms = GetComponentsInChildren<Transform>();
        nodes = new List<Transform>();

        foreach (Transform t in pathTransforms)
        {
            if (t != transform)
            {
                nodes.Add(t);
            }
        }

        for (int i = 0; i < nodes.Count; i++)
        {
            Vector3 curNode = nodes[i].position;
            Vector3 prevNode = Vector3.zero;

            if (i > 0)
            {
                prevNode = nodes[i - 1].position;
            }
            else
            {
                prevNode = nodes[nodes.Count - 1].position;
            }

            Gizmos.DrawLine(prevNode, curNode);

        }

    }

}
