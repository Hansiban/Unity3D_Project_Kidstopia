using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Grid : MonoBehaviour
{
    public LayerMask unwalkableMask; // ��ֹ� ǥ�� ���̾�
    public Vector2 gridWorldSize; // node grid size
    Node[,] grid; // grid
    public float nodeRadious; // node�� ������

    float nodeDiameter; // node�� ũ��
    int gridSizeX, gridSizeY; // grid size
    Vector3 worldBottonLeft;
    private void Start()
    {
        // grid space bake
        nodeDiameter = nodeRadious * 2;
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);
        CreateGrid();
    }

    private void CreateGrid()
    { // A* ���� ������ node grid ����
        grid = new Node[gridSizeX, gridSizeY]; // grid �ʱ�ȭ
         worldBottonLeft = transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.forward * gridWorldSize.y / 2;

        for (int x = gridSizeX - 1; x >= 0; x--)
        {
            for (int y = 0; y < gridSizeY; y++)
            { // grid ������ node ���� �ʱ�ȭ
                Vector3 worldPoint = worldBottonLeft + Vector3.right * (x * nodeDiameter + nodeRadious) + Vector3.forward * (y * nodeDiameter + nodeRadious);
                bool walkable = !(Physics.CheckSphere(worldPoint, nodeRadious, unwalkableMask)); // unwalableMask�� �ش��ϴ� node�� false
                grid[x, y] = new Node(walkable, worldPoint, x, y);
            }
        }
    }

    public List<Node> GetNeightnours(Node node)
    {
        List<Node> neighbours = new List<Node>();
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0) continue;
                int checkX = node.gridX + x;
                int checkY = node.gridY + y;

                if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY)
                {
                    neighbours.Add(grid[checkX, checkY]);
                }
            }
        }

        return neighbours;
    }

    public Node NodeFromWorldPoint(Vector3 worldPosition)
    { // target position node
        float percentX = (worldPosition.x  - worldBottonLeft.x) / gridWorldSize.x;
        float percentY = (worldPosition.z  - worldBottonLeft.z) / gridWorldSize.y;
        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        // node�� grid ��ǥ
        int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
        int y = Mathf.RoundToInt((gridSizeY - 1) * percentY);

        return grid[x, y];
    }

    public List<Node> path;
    /*private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, 1, gridWorldSize.y)); // gizmos ����
        if (grid != null)
        {
            for (int x = 0; x < gridSizeX; x++)
            {
                for (int y = 0; y < gridSizeY; y++)
                {
                    Node n = grid[x, y];
                    Gizmos.color = (n.walkable) ? Color.white : Color.red; // ��ֹ��� �ִ� ���� red
                    if (path != null)
                    {
                        if (path.Contains(n))
                        { // startNode���� endNode������ ��
                            Gizmos.color = Color.black;
                        }
                    }
                    Gizmos.DrawCube(n.worldPosition, Vector3.one * (nodeDiameter - .1f));
                }
            }
        }
    }*/

    public void OnDrawGizmos()
    {
        if (grid != null)
        {
            for (int x = 0; x < gridSizeX; x++)
            {
                for (int y = 0; y < gridSizeY; y++)
                {
                    Node n = grid[x, y];
                    if (path != null)
                    {
                        if (path.Contains(n))
                        { // startNode���� endNode������ ��
                            Gizmos.color = Color.black;
                            Gizmos.DrawCube(n.worldPosition, Vector3.one * (nodeDiameter - .1f));
                        }
                        else
                        {

                        }
                    }
                }
            }
        }
    }
}
