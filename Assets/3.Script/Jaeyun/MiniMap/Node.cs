using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public bool walkable; // ���̾�� �Ǵ�
    public Vector3 worldPosition; // node ��ġ
    public int gridX; // node x ��ǥ
    public int gridY; // node y ��ǥ

    public Node parent;

    public int gCost; // startNode���� currentNode������ ���
    public int hCost; // currentNode ���� endNode������ ���� ���

    public int fCost // A*
    {
        get
        {
            return gCost + hCost;
        }
    }

    public Node(bool _walkable, Vector3 _worldPos, int _gridX, int _gridY)
    {
        walkable = _walkable;
        worldPosition = _worldPos;
        gridX = _gridX;
        gridY = _gridY;
    }
}
