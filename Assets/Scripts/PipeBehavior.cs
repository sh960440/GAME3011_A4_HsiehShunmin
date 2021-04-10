using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PipeType
{
    C_UL,
    C_UR,
    C_LL,
    C_LR,
    S_HORIZONTAL,
    S_VERTICAL,
    COUNT
}
public class PipeBehavior : MonoBehaviour
{
    public PipeType pipeType;
    [SerializeField] private Sprite[] pipeSprites;
    private SpriteRenderer spriteRenderer;
    

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        
        // Randomize
        SetType((PipeType)Random.Range(0 , (int)PipeType.COUNT));
    }

    public void SetType(PipeType type)
    {
        pipeType = type;
        spriteRenderer.sprite = pipeSprites[(int)pipeType];
    }
}