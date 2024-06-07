using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Path : MonoBehaviour
{
    [SerializeField] Color gizmoColor;
    public List<GameObject> nodes;
    List<PathNode> segments;

    private void Start()
    {
        segments = GetSegments();
    }

    public List<PathNode> GetSegments()
    {
        List<PathNode> segments = new List<PathNode>();
        for (int i = 0; i < nodes.Count - 1; i++)
        {
            Vector3 src = nodes[i].transform.position;
            Vector3 dest = nodes[i + 1].transform.position;
            PathNode segment = new PathNode(src, dest);
            segments.Add(segment);
        }

        return segments;
    }

    public float GetParam(Vector3 pos, float lastParam)
    {
        float param = 0f;
        PathNode currentSeg = null;
        float tmpParam = 0f;

        foreach (PathNode ps in segments)
        {
            tmpParam += Vector3.Distance(ps.a, ps.b);
            if (lastParam <= tmpParam)
            {
                currentSeg = ps;
                break;
            }


        }

        if (currentSeg == null)
        {
            return 0f;
        }

        Vector3 curPos = pos - currentSeg.a;
        Vector3 segDir = currentSeg.b - currentSeg.a;
        segDir.Normalize();
        Vector3 pointInSegment = Vector3.Project(curPos, segDir);
        param = tmpParam - Vector3.Distance(currentSeg.a, currentSeg.b);
        param += pointInSegment.magnitude;
        return param;
    }

    public Vector3 GetPosition(float param)
    {
        Vector3 pos = Vector3.zero;
        PathNode curSeg = null;
        float tmpParam = 0f;

        foreach (PathNode ps in segments)
        {
            tmpParam += Vector3.Distance(ps.a, ps.b);
            if (param <= tmpParam)
            {
                curSeg = ps;
                break;
            }
        }

        if (curSeg == null)
        {
            return Vector3.zero;
        }

        Vector3 segDir = curSeg.b - curSeg.a;

        segDir.Normalize();
        tmpParam -= Vector3.Distance(curSeg.a, curSeg.b);
        tmpParam = param - tmpParam;
        pos = curSeg.a + segDir * tmpParam;
        return pos;
    }

    private void OnDrawGizmos()
    {
        Vector3 dir;
        Color tmp = Gizmos.color;
        Gizmos.color = gizmoColor;

        for (int i = 0; i < nodes.Count - 1; i++)
        {
            Vector3 src = nodes[i].transform.position;
            Vector3 dest = nodes[i + 1].transform.position;

            dir = dest - src;
            Gizmos.DrawRay(src, dir);
        }

        Gizmos.color = tmp;
    }
}
