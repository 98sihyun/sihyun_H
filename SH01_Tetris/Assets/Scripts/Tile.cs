using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    //������Ƽ________________________________________
    /// <summary>
    /// ��������Ʈ�� ���� ����
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
    /// �ش� ���̾���� ������ ���������� ����
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
