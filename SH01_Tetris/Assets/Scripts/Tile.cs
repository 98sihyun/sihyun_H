using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    //프로퍼티________________________________________
    /// <summary>
    /// 스프라이트의 색상 지정
    /// </summary>
    public Color color
    {
        set
        {
            spriteRenderer.color = value;
        }
        get
        {
            return spriteRenderer.color;
        }
    }
    /// <summary>
    /// 해당 레이어에서의 순서를 정수값으로 지정
    /// </summary>
    public int sortingOrder
    {
        set
        {
            spriteRenderer.sortingOrder = value;
        }

        get
        {
            return spriteRenderer.sortingOrder;
        }
    }

    //_______________________________________________

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        if(spriteRenderer == null)
        {

        }
    }
}
