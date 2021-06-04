using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class ST_PuzzleDisplay : MonoBehaviour 
{
	// esta textura de rompecabezas.
	public Texture PuzzleImage;

	// el ancho y alto del rompecabezas en mosaicos (tiles).
	public int Height = 3;
	public int Width  = 3;
	public int Difficulty = 1; //es el número de veces que se desordenan las piezas

	// valor de escala adicional.
	public Vector3 PuzzleScale = new Vector3(1.0f, 1.0f, 1.0f);

	// desplazamiento de posicionamiento adicional (offset).
	public Vector3 PuzzlePosition = new Vector3(0.0f, 0.0f, 0.0f);

	// valor de separación entre fichas de rompecabezas.
	public float SeperationBetweenTiles = 0.5f;

	// el objeto de visualización de mosaico (tile).
	public GameObject Tile;

	// el sombreador (shader) utilizado para renderizar el rompecabezas.
	public Shader PuzzleShader;

	// matriz (array) de los mosaicos generados.
	private GameObject[,] TileDisplayArray;
	private List<Vector3>  DisplayPositions = new List<Vector3>();

	// valores de posición y escala.
	private Vector3 Scale;
	private Vector3 Position;

	// ¿Se ha completado el rompecabezas?
	public bool Complete = false;

	// Use esto para la inicialización 
	void Start () 
	{
		// crea los mosaicos del rompecabezas del juego a partir de la imagen proporcionada.
		CreatePuzzleTiles();

		// mezcla el rompecabezas.
		StartCoroutine(JugglePuzzle());

	}

	// La actualización (Update) se llama una vez por fotograma
	void Update () 
	{
		// mueve el rompecabezas a la posición establecida en el inspector.
		this.transform.localPosition = PuzzlePosition;

		// establece la escala de todo el objeto del rompecabezas como se establece en el inspector.
		this.transform.localScale = PuzzleScale;
	}

	public Vector3 GetTargetLocation(ST_PuzzleTile thisTile)
	{
		// verificamos si podemos mover esta ficha y obtener la posición a la que podemos movernos.
		ST_PuzzleTile MoveTo = CheckIfWeCanMove((int)thisTile.GridLocation.x, (int)thisTile.GridLocation.y, thisTile);

		if(MoveTo != thisTile)
		{
			// obtén la posición de destino (target) para este nuevo mosaico.
			Vector3 TargetPos = MoveTo.TargetPosition;
			Vector2 GridLocation = thisTile.GridLocation;
			thisTile.GridLocation = MoveTo.GridLocation;

			// mueve la ficha (tile) vacía a la posición actual de esta ficha.
			MoveTo.LaunchPositionCoroutine(thisTile.TargetPosition);
			MoveTo.GridLocation = GridLocation;

			// devuelve la nueva posición (target) de destino.
			return TargetPos;
		}

		// de lo contrario, devuelve la posición real de las fichas (sin movimiento). 
		return thisTile.TargetPosition;
	}

	private ST_PuzzleTile CheckMoveLeft(int Xpos, int Ypos, ST_PuzzleTile thisTile)
	{
		// mover hacia la izquierda
		if ((Xpos - 1)  >= 0)
		{
			// podemos movernos a la izquierda, ¿se está utilizando el espacio actualmente?
			return GetTileAtThisGridLocation(Xpos - 1, Ypos, thisTile);
		}
		
		return thisTile;
	}
	
	private ST_PuzzleTile CheckMoveRight(int Xpos, int Ypos, ST_PuzzleTile thisTile)
	{
		// moverse a la derecha 
		if ((Xpos + 1)  < Width)
		{
			// podemos movernos a la derecha, ¿se está utilizando el espacio actualmente?
			return GetTileAtThisGridLocation(Xpos + 1, Ypos , thisTile);
		}
		
		return thisTile;
	}
	
	private ST_PuzzleTile CheckMoveDown(int Xpos, int Ypos, ST_PuzzleTile thisTile)
	{
		// mover hacia abajo
		if ((Ypos - 1)  >= 0)
		{
			// podemos movernos hacia abajo, ¿se está utilizando el espacio actualmente?
			return GetTileAtThisGridLocation(Xpos, Ypos  - 1, thisTile);
		}
		
		return thisTile;
	}
	
	private ST_PuzzleTile CheckMoveUp(int Xpos, int Ypos, ST_PuzzleTile thisTile)
	{
		// mover hacia arriba 
		if ((Ypos + 1)  < Height)
		{
			// podemos movernos hacia arriba, ¿se está utilizando el espacio actualmente?
			return GetTileAtThisGridLocation(Xpos, Ypos  + 1, thisTile);
		}
		
		return thisTile;
	}
	
	private ST_PuzzleTile CheckIfWeCanMove(int Xpos, int Ypos, ST_PuzzleTile thisTile)
	{
		// comprobar cada dirección de movimiento
		if (CheckMoveLeft(Xpos, Ypos, thisTile) != thisTile)
		{
			return CheckMoveLeft(Xpos, Ypos, thisTile);
		}
		
		if(CheckMoveRight(Xpos, Ypos, thisTile) != thisTile)
		{
			return CheckMoveRight(Xpos, Ypos, thisTile);
		}
		
		if(CheckMoveDown(Xpos, Ypos, thisTile) != thisTile)
		{
			return CheckMoveDown(Xpos, Ypos, thisTile);
		}
		
		if(CheckMoveUp(Xpos, Ypos, thisTile) != thisTile)
		{
			return CheckMoveUp(Xpos, Ypos, thisTile);
		}

		return thisTile;
	}

	private ST_PuzzleTile GetTileAtThisGridLocation(int x, int y, ST_PuzzleTile thisTile)
	{
		for(int j = Height - 1; j >= 0; j--)
		{
			for(int i = 0; i < Width; i++)
			{
				// verifica si este mosaico tiene la ubicación de visualización de cuadrícula (grid) correcta.
				if ((TileDisplayArray[i,j].GetComponent<ST_PuzzleTile>().GridLocation.x == x)&&
				   (TileDisplayArray[i,j].GetComponent<ST_PuzzleTile>().GridLocation.y == y))
				{
					if(TileDisplayArray[i,j].GetComponent<ST_PuzzleTile>().Active == false)
					{
						// devuelve esta propiedad activa de mosaico. 
						return TileDisplayArray[i,j].GetComponent<ST_PuzzleTile>();
					}
				}
			}
		}

		return thisTile;
	}

	private IEnumerator JugglePuzzle()
	{
		yield return new WaitForSeconds(1.0f);

		// esconde una ficha de rompecabezas (siempre falta una para permitir el movimiento del rompecabezas).
		TileDisplayArray[0,0].GetComponent<ST_PuzzleTile>().Active = false;

		yield return new WaitForSeconds(1.0f);

		for(int k = 0; k < Difficulty; k++)
		{
			// usar random para colocar cada sección del rompecabezas en la matriz (array) elimine el número una vez que se llene el espacio.
			for (int j = 0; j < Height; j++)
			{
				for(int i = 0; i < Width; i++)
				{
					// intenta ejecutar un movimiento para esta ficha.
					TileDisplayArray[i,j].GetComponent<ST_PuzzleTile>().ExecuteAdditionalMove();

					yield return new WaitForSeconds(0.02f);
				}
			}
		}

		// verifica continuamente la respuesta correcta.
		StartCoroutine(CheckForComplete());

		yield return null;
	}

	public IEnumerator CheckForComplete()
	{
		while(Complete == false)
		{
			// itera sobre todos los mosaicos y comprueba si están en la posición correcta.
			Complete = true;
			for(int j = Height - 1; j >= 0; j--)
			{
				for(int i = 0; i < Width; i++)
				{
					// verifica si este mosaico tiene la ubicación de visualización de cuadrícula (grid) correcta.
					if (TileDisplayArray[i,j].GetComponent<ST_PuzzleTile>().CorrectLocation == false)  
					{
						Complete = false;
					}
				}
			}

			yield return null;
		}

		// si todavía estamos completos, entonces todos los mosaicos son correctos.
		if (Complete)
		{
			// puzzle completado correctamente
			Debug.Log("Puzzle Completado!");
			SceneManager.LoadScene("town");
		}

		yield return null;
	}

	private Vector2 ConvertIndexToGrid(int index)
	{
		int WidthIndex = index;
		int HeightIndex = 0;

		// toma el valor del índice y devuelve la ubicación de la matriz de cuadrícula X, Y.
		for (int i = 0; i < Height; i++)
		{
			if(WidthIndex < Width)
			{
				return new Vector2(WidthIndex, HeightIndex);
			}
			else
			{
				WidthIndex -= Width;
				HeightIndex++;
			}
		}

		return new Vector2(WidthIndex, HeightIndex);
	}

	private void CreatePuzzleTiles()
	{
		// usando las variables de ancho y alto crea una matriz (array).
		TileDisplayArray = new GameObject[Width,Height];

		// establece los valores de escala y posición para este rompecabezas.
		Scale = new Vector3(1.0f/Width, 1.0f, 1.0f/Height);
		Tile.transform.localScale = Scale;

		// se usa para contar el número de mosaicos y asignar a cada mosaico un valor correcto.
		int TileValue = 0;

		// genera los mosaicos en una matriz (array).
		for (int j = Height - 1; j >= 0; j--)
		{
			for(int i = 0; i < Width; i++)
			{
				// calcula la posición de este mosaico centrado alrededor de Vector3 (0.0f, 0.0f, 0.0f). 
				Position = new Vector3(((Scale.x * (i + 0.5f))-(Scale.x * (Width/2.0f))) * (10.0f + SeperationBetweenTiles), 
				                       0.0f, 
				                      ((Scale.z * (j + 0.5f))-(Scale.z * (Height/2.0f))) * (10.0f + SeperationBetweenTiles));

				// establece esta ubicación en la cuadrícula de visualización (display grid).
				DisplayPositions.Add(Position);

				// genera el objeto en juego.
				TileDisplayArray[i,j] = Instantiate(Tile, new Vector3(0.0f, 0.0f, 0.0f) , Quaternion.Euler(90.0f, -180.0f, 0.0f)) as GameObject;
				TileDisplayArray[i,j].gameObject.transform.parent = this.transform;

				// establece e incrementa el contador de números de pantalla.
				ST_PuzzleTile thisTile = TileDisplayArray[i,j].GetComponent<ST_PuzzleTile>();
				thisTile.ArrayLocation = new Vector2(i,j);
				thisTile.GridLocation = new Vector2(i,j);
				thisTile.LaunchPositionCoroutine(Position);
				TileValue++;

				// crea un nuevo material usando el sombreador (shader) definido.
				Material thisTileMaterial = new Material(PuzzleShader);

				// aplica la imagen del rompecabezas.
				thisTileMaterial.mainTexture = PuzzleImage;

				// establece los valores de desplazamiento y mosaico para este material.
				thisTileMaterial.mainTextureOffset = new Vector2(1.0f/Width * i, 1.0f/Height * j);
				thisTileMaterial.mainTextureScale  = new Vector2(1.0f/Width, 1.0f/Height);

				// asigna el nuevo material a este mosaico para su visualización.
				TileDisplayArray[i,j].GetComponent<Renderer>().material = thisTileMaterial;
			}
		}

		/*
		// ¡Habilita un rompecabezas imposible para divertirte!
		// cambia las texturas de ubicación de la segunda y tercera cuadrícula. 
		Material thisTileMaterial2 = TileDisplayArray[1,3].GetComponent<Renderer>().material;
		Material thisTileMaterial3 = TileDisplayArray[2,3].GetComponent<Renderer>().material;
		TileDisplayArray[1,3].GetComponent<Renderer>().material = thisTileMaterial3;
		TileDisplayArray[2,3].GetComponent<Renderer>().material = thisTileMaterial2;
		*/
	}

	public void Return()
    {
		SceneManager.LoadScene("town");
    }
}
