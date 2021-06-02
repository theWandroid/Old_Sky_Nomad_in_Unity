using UnityEngine;
using System.Collections;

public class ST_PuzzleTile : MonoBehaviour 
{
	// la posición (target) de destino para este mosaico (tile).
	public Vector3 TargetPosition;

	// ¿Es esta una ficha activa? normalmente uno por juego está inactivo.
	public bool Active = true;

	// ¿Está este mosaico en la ubicación correcta?
	public bool CorrectLocation = false;

	// almacena la ubicación de esta matriz de mosaicos.
	public Vector2 ArrayLocation = new Vector2();
	public Vector2 GridLocation = new Vector2();

	void Awake()
	{
		// asigna la nueva posición de destino.
		TargetPosition = this.transform.localPosition;

		// inicia la corrutina de movimiento para mover siempre los objetos a la nueva posición de destino.
		StartCoroutine(UpdatePosition());
	}

	public  void LaunchPositionCoroutine(Vector3 newPosition)
	{
		// asigna la nueva posición de destino.
		TargetPosition = newPosition;

		// inicia la corrutina de movimiento para mover siempre los objetos a la nueva posición de destino.
		StartCoroutine(UpdatePosition());
	}

	public IEnumerator UpdatePosition()
	{
		// mientras no estemos en nuestra posición objetivo.
		while (TargetPosition != this.transform.localPosition)
		{
			// lerp hacia nuestro objetivo.
			this.transform.localPosition = Vector3.Lerp(this.transform.localPosition, TargetPosition, 10.0f * Time.deltaTime);
			yield return null;
		}

		// después de cada movimiento, verificamos si ahora estamos en la ubicación correcta.
		if (ArrayLocation == GridLocation){CorrectLocation = true;}else{CorrectLocation = false;}

		// si no somos un mosaico activo entonces ocultamos nuestro renderizador y colisionador.
		if (Active == false)
		{
			this.GetComponent<Renderer>().enabled = false;
			this.GetComponent<Collider>().enabled = false;
		}

		yield return null;
	}

	public void ExecuteAdditionalMove()
	{
		// obtén la pantalla del rompecabezas (puzzle display) y devuelve la nueva ubicación (target) de destino de este mosaico (tile).
		LaunchPositionCoroutine(this.transform.parent.GetComponent<ST_PuzzleDisplay>().GetTargetLocation(this.GetComponent<ST_PuzzleTile>()));
	}

	void OnMouseDown()
	{
		// obtén la pantalla del rompecabezas (puzzle display) y devuelve la nueva ubicación (target) de destino de este mosaico (tile).
		LaunchPositionCoroutine(this.transform.parent.GetComponent<ST_PuzzleDisplay>().GetTargetLocation(this.GetComponent<ST_PuzzleTile>()));
	}
}
