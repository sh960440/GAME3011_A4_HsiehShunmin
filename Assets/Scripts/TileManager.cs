using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    [SerializeField] private GameObject tilePrefab;
    private int size;
    private GameObject[,] pipes;
    private GameObject pipeToSwap;
    private int currentX = 0;
    private int currentY = 0;
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (currentX + 1 < size)
                SelectTile(currentX + 1, currentY);
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (currentX - 1 >= 0)
                SelectTile(currentX - 1, currentY);
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (currentY - 1 >= 0)
                SelectTile(currentX, currentY - 1);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (currentY + 1 < size)
                SelectTile(currentX, currentY + 1);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Swap(pipes[currentX, currentY]); 
        }
    }

    public void GenerateTiles(int levelSize)
    {
        if (levelSize > 6 || levelSize < 3) return;

        pipeToSwap = Instantiate(tilePrefab, new Vector3(0.0f, 4.0f, 0.0f), Quaternion.identity, this.transform);

        size = levelSize;
        pipes = new GameObject[size, size];
        float startPositionX = -2.5f + 0.5f * (6 - size);
        float startPositionY = 2.5f - 0.5f * (6 - size);

        // Generate pipes
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                pipes[i,j] = Instantiate(tilePrefab, new Vector3(startPositionX + i, startPositionY - j, 0.0f), Quaternion.identity, this.transform);
            }
        }

        // Select first pipe
        currentX = 0;
        currentY = 0;
        pipes[currentX, currentY].GetComponent<SpriteRenderer>().color = Color.yellow;
    }

    private void SelectTile(int nextX, int nextY)
    {
        // Reset background
        pipes[currentX, currentY].GetComponent<SpriteRenderer>().color = Color.white;
        // Update pipe index
        currentX = nextX;
        currentY = nextY;
        pipes[currentX, currentY].GetComponent<SpriteRenderer>().color = Color.yellow;
    }

    private void Swap(GameObject swappingPipe)
    {
        PipeType temp = swappingPipe.GetComponent<PipeBehavior>().pipeType;
        swappingPipe.GetComponent<PipeBehavior>().SetType(pipeToSwap.GetComponent<PipeBehavior>().pipeType);
        pipeToSwap.GetComponent<PipeBehavior>().SetType(temp);
        
        // Start checking path
        if (pipes[0, size - 1].GetComponent<PipeBehavior>().pipeType == PipeType.C_UL)
            CheckNextPipe(0, size - 1, 8);
        else if (pipes[0, size - 1].GetComponent<PipeBehavior>().pipeType == PipeType.S_HORIZONTAL)
            CheckNextPipe(0, size - 1, 6);
    }

    private void CheckPath()
    {
        if (pipes[0, size - 1].GetComponent<PipeBehavior>().pipeType != PipeType.C_UL && pipes[0, size - 1].GetComponent<PipeBehavior>().pipeType != PipeType.S_HORIZONTAL) return;

        if (pipes[0, size - 1].GetComponent<PipeBehavior>().pipeType == PipeType.C_UL)
            CheckNextPipe(0, size - 1, 8);
        else if (pipes[0, size - 1].GetComponent<PipeBehavior>().pipeType == PipeType.S_HORIZONTAL)
            CheckNextPipe(0, size - 1, 6);
    }

    private void CheckNextPipe(int pipeX, int pipeY, int direction)
    {
        switch (direction)
        {
            case 2:
                if (pipeY + 1 >= size) break;
                switch (pipes[pipeX, pipeY + 1].GetComponent<PipeBehavior>().pipeType)
                {
                    case PipeType.C_UL:
                        CheckNextPipe(pipeX, pipeY + 1, 4);
                        break;
                    case PipeType.C_UR:
                        CheckNextPipe(pipeX, pipeY + 1, 6);
                        break;
                    case PipeType.S_VERTICAL:
                        CheckNextPipe(pipeX, pipeY + 1, 2);
                        break;
                }
                break;
            case 4:
                if (pipeX - 1 < 0) break;
                switch (pipes[pipeX - 1, pipeY].GetComponent<PipeBehavior>().pipeType)
                {
                    case PipeType.C_UR:
                        CheckNextPipe(pipeX - 1, pipeY, 8);
                        break;
                    case PipeType.C_LR:
                        CheckNextPipe(pipeX - 1, pipeY, 2);
                        break;
                    case PipeType.S_HORIZONTAL:
                        CheckNextPipe(pipeX - 1, pipeY, 4);
                        break;
                }
                break;
            case 6:
                if (pipeX == size - 1 && pipeY == 0)
                {
                    FindObjectOfType<GameManager>().Gameover(true);
                    break;
                }
                if (pipeX + 1 >= size) break;
                switch (pipes[pipeX + 1, pipeY].GetComponent<PipeBehavior>().pipeType)
                {
                    case PipeType.C_UL:
                        CheckNextPipe(pipeX + 1, pipeY, 8);
                        break;
                    case PipeType.C_LL:
                        CheckNextPipe(pipeX + 1, pipeY, 2);
                        break;
                    case PipeType.S_HORIZONTAL:
                        CheckNextPipe(pipeX + 1, pipeY, 6);
                        break;
                }
                break;
            case 8:
                if (pipeY - 1 < 0) break;
                switch (pipes[pipeX, pipeY - 1].GetComponent<PipeBehavior>().pipeType)
                {
                    case PipeType.C_LL:
                        CheckNextPipe(pipeX, pipeY - 1, 4);
                        break;
                    case PipeType.C_LR:
                        CheckNextPipe(pipeX, pipeY - 1, 6);
                        break;
                    case PipeType.S_VERTICAL:
                        CheckNextPipe(pipeX, pipeY - 1, 8);
                        break;
                }
                break;
        }
    }
}