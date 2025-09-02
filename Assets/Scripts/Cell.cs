using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class Cell : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private SpriteRenderer visualSprite;
    [SerializeField] private CellSettings cellSettings;
    
    private Vector2 originalScale;
    
    private bool bOpen = false;
    private bool bMine = false;
    private int mineCount = 0;

    public bool IsOpen => bOpen;
    public bool IsMine => bMine;
    public int MineCount => mineCount;

    private bool bHovering = false;
    
    public Vector2Int CellPosition { get; set; }
    public Action<Vector2Int> OnClicked;

    private void Awake()
    {
        originalScale = transform.localScale;
    }

    private void Update()
    {
        if (bOpen)
        {
            return;
        }
        
        if (bHovering)
        {
            transform.localScale = originalScale * cellSettings.hoverScaling;
        }
        else
        {
            transform.localScale = originalScale;
        }
    }
    
    public void Open()
    {
        ChooseSprite();
        bOpen = true;
        transform.localScale = originalScale;
    }

    public void OpenWin()
    {
        visualSprite.sprite = cellSettings.mineSprite;
        bOpen = true;
        transform.localScale = originalScale;
    }

    private void ChooseSprite()
    {
        if (bMine)
        {
            visualSprite.sprite = cellSettings.explodeSprite;
            return;
        }

        switch (mineCount)
        {
            case 0:
                visualSprite.sprite = cellSettings.emptySprite;
                break;
            case 1:
                visualSprite.sprite = cellSettings.oneSprite;
                break;
            case 2:
                visualSprite.sprite = cellSettings.twoSprite;
                break;
            case 3:
                visualSprite.sprite = cellSettings.threeSprite;
                break;
            case 4:
                visualSprite.sprite = cellSettings.fourSprite;
                break;
            case 5:
                visualSprite.sprite = cellSettings.fiveSprite;
                break;
            case 6:
                visualSprite.sprite = cellSettings.sixSprite;
                break;
            case 7:
                visualSprite.sprite = cellSettings.sevenSprite;
                break;
            case 8:
                visualSprite.sprite = cellSettings.eightSprite;
                break;
            default:
                visualSprite.sprite = cellSettings.flagSprite;
                break;
        }
    }

    public void SetMine()
    {
        bMine = true;
    }
    
    public void SetMineCount(int count)
    {
        mineCount = count;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            SetFlag();
        }
        else
        {
            OnClicked.Invoke(CellPosition);
        }
    }

    private void SetFlag()
    {
        visualSprite.sprite = cellSettings.flagSprite;
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
