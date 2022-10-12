using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Stage : MonoBehaviour
{
    [Header("Editor Objects")]          // �ν����Ϳ� Editor Objects �ؽ�Ʈ ����� �׷��� ������
    public GameObject tilePrefab;
    public Transform backgroundNode;
    public Transform boardNode;
    public Transform tetrominoNode;

    [Header("Game Settings")]
    [Range(4, 40)]                      // Range(min,max) �ּ�/�ִ� ��
    public int boardWidth = 10;
    [Range(5, 20)]
    public int boardHeight = 20;
    public float fallCyecle = 1.0f;

    private int halfWidth;
    private int halfHeight;

    private void Start()
    {
        halfWidth = Mathf.RoundToInt(boardWidth * 0.5f);    //Round �ݿø� ToInt int������ ��ȯ�Ѵ�.
        halfHeight = Mathf.RoundToInt(boardHeight * 0.5f);

        CreateBackground();
        CreateTetromino();
    }

    

    /// <summary>
    /// Ÿ�� �����
    /// </summary>
    /// <param name="parent">� ����� ����(�ڽ�)���� ������ �ٰ����� ����. �θ�Ȱ</param>
    /// <param name="position">(x,y)��ǥ </param>
    /// <param name="color">Ÿ���� ����</param>
    /// <param name="order">���̾���� ���� ����(Sorting Oder)�� �׸� ����</param>
    /// <returns></returns>
    Tile CreateTile(Transform parent, Vector2 position, Color color, int order=1)
    {
        var go = Instantiate(tilePrefab);
        go.transform.parent = parent;
        go.transform.localPosition = position;

        var tile = go.GetComponent<Tile>();
        tile.color= color;
        tile.sortingOrder = order;

        return tile;
    }
    /// <summary>
    /// ��� Ÿ���� ����
    /// </summary>
    private void CreateBackground()
    {
        Color color = Color.gray;

        // Ÿ�� ����
        color.a = 0.5f;
        for(int x=  -halfWidth; x < halfWidth; ++x)
        {
            for (int y = halfHeight; y > -halfHeight; --y)
            {
                CreateTile(backgroundNode, new Vector2(x, y), color, 0);
            }
        }

        //�¿� �׵θ�
        color.a = 1.0f;
        for(int y = halfHeight; y > -halfHeight; --y)
        {
            CreateTile(backgroundNode, new Vector2(-halfWidth - 1, y), color, 0);
            CreateTile(backgroundNode, new Vector2(halfWidth, y), color, 0);
        }

        // �Ʒ� �׵θ�
        for(int x = -halfWidth -1 ; x <= halfWidth; ++x)
        {
            CreateTile(backgroundNode, new Vector2(x, -halfHeight), color, 0);
        }
    }
    /// <summary>
    /// ��Ʈ�ι̳� ����
    /// </summary>
    private void CreateTetromino()
    {
        
    }

}
