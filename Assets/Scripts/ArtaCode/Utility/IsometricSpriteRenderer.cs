using UnityEngine;

[ExecuteInEditMode]
public class IsometricSpriteRenderer : MonoBehaviour 
{
	public int offsetSorting;

	void Update ()
	{
		GetComponent<Renderer>().sortingOrder = (int)(transform.position.y * -10 + offsetSorting);
	}
}
