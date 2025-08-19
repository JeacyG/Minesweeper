using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class Cell : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private SpriteRenderer coverSprite;
    [SerializeField] private SpriteRenderer openSprite;
    [SerializeField] private TextMeshPro mineCountText;
    [SerializeField] private CellSettings cellSettings;
    
    private Vector2 originalScale;
    
    private bool bOpen = false;
    private bool bMine = false;
    private int mineCount = 0;

    public bool IsOpen
    {
        get
        {
            return bOpen;
        }
        set
        {
            if (value)
            {
                transform.localScale = originalScale;
                openSprite.color = cellSettings.openColor;
                coverSprite.enabled = false;
            }
            bOpen = value;
        }
    }

    public bool IsMine
    {
        get
        {
            return bMine;
        }
        set
        {
            if (value)
            {
                mineCountText.text = "X";
            }
            bMine = value;
        }
    }

    public int MineCount
    {
        get
        {
            return mineCount;
        }
        set
        {
            mineCount = value;
            mineCountText.text = mineCount.ToString();
        }
    }

    private bool bHovering = false;
    private bool bClicking = false;
    
    public Vector2Int CellPosition { get; set; }
    public Action<Vector2Int> OnClicked;

    private void Awake()
    {
        originalScale = transform.localScale;
        openSprite.color = cellSettings.closeColor;
    }

    private void Update()
    {
        if (bOpen)
        {
            return;
        }
        
        if (bHovering)
        {
            coverSprite.color = cellSettings.hoverColor;
            transform.localScale = originalScale * cellSettings.hoverScaling;
        }
        else
        {
            coverSprite.color = cellSettings.closeColor;
            transform.localScale = originalScale;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log(transform.name + " OnPointerDown");
        bClicking = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        OnClicked(CellPosition);
        bClicking = false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        bHovering = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        bHovering = false;
    }
}
