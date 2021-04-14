using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puzzle : MonoBehaviour {

    public Texture2D image;
    [SerializeField] //esto hace que aunque la variable sea privada se vea en el inspector para modificarlo desde allí
    int blocksPerLine = 4; //bloques por lina (vertical)
    [SerializeField]
    int shuffleLength = 20;
    public float defaultMoveDuration = .2f; //establecer la velocidad de movimiento
    public float shuffleMoveDuration = .1f; //establecer la velocidad de movimiento

    enum PuzzleState { Solved, Shuffling, InPlay }; //enumeración para saber si esta resuelto, se está barajando o el jugador está en medio del juego
    PuzzleState state; //tomará por defecto el primer valor de la enumeración

    Block emptyBlock; // bloques vacios
    Block[,] blocks; //array de bloques
    Queue<Block> inputs; //inputs
    bool blockIsMoving;
    int shuffleMovesRemaining;
    Vector2Int prevShuffleOffset;

    void Start()
    {
        CreatePuzzle();
    }

    void Update()
    {
        //empezar a barajar, revolver los bloques (piezas)
        if (state == PuzzleState.Solved && Input.GetMouseButtonDown(0)) //Dar a clic izquierdo para desordenar piezas
        {
            StartShuffle(); //Desordenar piezas
        }
    }

    void CreatePuzzle() //función o método para crear el puzzle
    {
        blocks = new Block[blocksPerLine, blocksPerLine];
        Texture2D[,] imageSlices = ImageSlicer.GetSlices(image, blocksPerLine); //matriz recortadora de imagenes 
        for (int y = 0; y < blocksPerLine; y++) //iterar sobre la cuadricula
        {
            for (int x = 0; x < blocksPerLine; x++)
            {
                GameObject blockObject = GameObject.CreatePrimitive(PrimitiveType.Quad); //iterar mi objeto del bloque
                blockObject.transform.position = -Vector2.one * (blocksPerLine - 1) * .5f + new Vector2(x, y); //establecer posición para que el objeto del bloque se tranforme
                blockObject.transform.parent = transform;

                Block block = blockObject.AddComponent<Block>();
                block.OnBlockPressed += PlayerMoveBlockInput;
                block.OnFinishedMoving += OnBlockFinishedMoving;
                block.Init(new Vector2Int(x, y), imageSlices[x, y]);
                blocks[x, y] = block;

                if (y == 0 && x == blocksPerLine - 1)
                {
                    emptyBlock = block;
                }
            }
        }

        Camera.main.orthographicSize = blocksPerLine * .55f; //convertir el tamaño de la camara ortografica para que encaje con la altura de la pantalla (.5f) pero para darle un margen ponerlo .55f
        inputs = new Queue<Block>();
    }

    void PlayerMoveBlockInput(Block blockToMove) //intercambiar el bloque que se presiona con el bloque vacio
    {
        if (state == PuzzleState.InPlay) 
        {
            inputs.Enqueue(blockToMove);
            MakeNextPlayerMove();
        }
    }

    void MakeNextPlayerMove()
    {
		while (inputs.Count > 0 && !blockIsMoving)
		{
            MoveBlock(inputs.Dequeue(), defaultMoveDuration);
		}
    }

    void MoveBlock(Block blockToMove, float duration) // mover bloque
    {
		if ((blockToMove.coord - emptyBlock.coord).sqrMagnitude == 1)
		{
            blocks[blockToMove.coord.x, blockToMove.coord.y] = emptyBlock;
            blocks[emptyBlock.coord.x, emptyBlock.coord.y] = blockToMove;

			Vector2Int targetCoord = emptyBlock.coord;
			emptyBlock.coord = blockToMove.coord;
			blockToMove.coord = targetCoord;

			Vector2 targetPosition = emptyBlock.transform.position;
			emptyBlock.transform.position = blockToMove.transform.position;
            blockToMove.MoveToPosition(targetPosition, duration);
            blockIsMoving = true;
		}
    }

    void OnBlockFinishedMoving() //ver si el jugador ha terminado de mover
    {
        blockIsMoving = false;
        CheckIfSolved();

        if (state == PuzzleState.InPlay)
        {
            MakeNextPlayerMove();
        }
        else if (state == PuzzleState.Shuffling)
        {
            if (shuffleMovesRemaining > 0)
            {
                MakeNextShuffleMove();
            }
            else
            {
                state = PuzzleState.InPlay;
            }
        }
    }

    void StartShuffle() // empezar a barajar
    {
        state = PuzzleState.Shuffling;
        shuffleMovesRemaining = shuffleLength;
        emptyBlock.gameObject.SetActive(false);
        MakeNextShuffleMove();
    }

    void MakeNextShuffleMove() //siguiente movimiento aleatorio del bloque vacio con el que presiones
    {
        Vector2Int[] offsets = { new Vector2Int(1, 0), new Vector2Int(-1, 0), new Vector2Int(0, 1), new Vector2Int(0, -1) };
        int randomIndex = Random.Range(0, offsets.Length);

        for (int i = 0; i < offsets.Length; i++)
        {
            Vector2Int offset = offsets[(randomIndex + i) % offsets.Length];
            if (offset != prevShuffleOffset * -1)
            {
                Vector2Int moveBlockCoord = emptyBlock.coord + offset;

                if (moveBlockCoord.x >= 0 && moveBlockCoord.x < blocksPerLine && moveBlockCoord.y >= 0 && moveBlockCoord.y < blocksPerLine)
                {
                    MoveBlock(blocks[moveBlockCoord.x, moveBlockCoord.y], shuffleMoveDuration);
                    shuffleMovesRemaining--;
                    prevShuffleOffset = offset;
                    break;
                }
            }
        }
      
    }

    void CheckIfSolved() //ver si ha resuelto el puzzle
    {
        foreach (Block block in blocks)
        {
            if (!block.IsAtStartingCoord())
            {
                return;
            }
        }

        state = PuzzleState.Solved;
        emptyBlock.gameObject.SetActive(true);
    }
}
