using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Stage : MonoBehaviour
{
    [Header("Editor Objects")]          // 인스펙터에 Editor Objects 텍스트 출력해 그룹을 구분함
    public GameObject tilePrefab;
    public Transform backgroundNode;
    public Transform boardNode;
    public Transform tetrominoNode;

    [Header("Game Settings")]
    [Range(4, 40)]                      // Range(min,max) 최소/최댓 값
    public int boardWidth = 10;
    [Range(5, 20)]
    public int boardHeight = 20;
    public float fallCyecle = 1.0f;

    private int halfWidth;
    private int halfHeight;

    private void Start()
    {
        halfWidth = Mathf.RoundToInt(boardWidth * 0.5f);    //Round 반올림 ToInt int형으로 반환한다.
        halfHeight = Mathf.RoundToInt(boardHeight * 0.5f);

        CreateBackground();
        CreateTetromino();
    }

    

    /// <summary>
    /// 타일 만들기
    /// </summary>
    /// <param name="parent">어떤 노드의 하위(자식)으로 연겨해 줄것인지 지정. 부모역활</param>
    /// <param name="position">(x,y)좌표 </param>
    /// <param name="color">타일의 색상</param>
    /// <param name="order">레이어에서의 정렬 순서(Sorting Oder)로 그릴 순서</param>
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
    /// 배경 타일을 생성
    /// </summary>
    private void CreateBackground()
    {
        Color color = Color.gray;

        // 타일 보드
        color.a = 0.5f;
        for(int x=  -halfWidth; x < halfWidth; ++x)
        {
            for (int y = halfHeight; y > -halfHeight; --y)
            {
                CreateTile(backgroundNode, new Vector2(x, y), color, 0);
            }
        }

        //좌우 테두리
        color.a = 1.0f;
        for(int y = halfHeight; y > -halfHeight; --y)
        {
            CreateTile(backgroundNode, new Vector2(-halfWidth - 1, y), color, 0);
            CreateTile(backgroundNode, new Vector2(halfWidth, y), color, 0);
        }

        // 아래 테두리
        for(int x = -halfWidth -1 ; x <= halfWidth; ++x)
        {
            CreateTile(backgroundNode, new Vector2(x, -halfHeight), color, 0);
        }
    }
    /// <summary>
    /// 테트로미노 생성
    /// </summary>
    private void CreateTetromino()
    {
        
    }

}
