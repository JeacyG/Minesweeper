using UnityEngine;

[CreateAssetMenu(fileName = "CellSettings", menuName = "Minesweeper/Cell Settings")]
public class CellSettings : ScriptableObject
{
    public Color closeColor;
    public Color openColor;
    public Color hoverColor;
    public float hoverScaling;
    
    [Header("Sprites")]
    public Sprite emptySprite;
    public Sprite mineSprite;
    public Sprite flagSprite;
    public Sprite oneSprite;
    public Sprite twoSprite;
    public Sprite threeSprite;
    public Sprite fourSprite;
    public Sprite fiveSprite;
    public Sprite sixSprite;
    public Sprite sevenSprite;
    public Sprite eightSprite;
}
