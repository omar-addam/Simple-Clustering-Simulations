using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{

	#region Variables

	/// <summary>
	/// References the prefab used to generate entities and display them on the grid.
	/// </summary>
	public GameObject EntityTemplate;

	/// <summary>
	/// References the gameobject that will hold all entities.
	/// </summary>
	public GameObject EntitiesParent;

	/// <summary>
	/// References the gameobject that will hold all paths.
	/// </summary>
	public GameObject PathsParent;

	#endregion

	#region Methods

	/// <summary>
	/// Deletes all entities in the grid.
	/// </summary>
	public void ClearEntities()
	{
		foreach (Transform entity in EntitiesParent.transform)
			GameObject.Destroy(entity.gameObject);
	}

	/// <summary>
	/// Displays entities on the grid.
	/// </summary>
	/// <param name="positions">Position of the entities.</param>
	/// <param name="positions">The color applied to the entities. Null = white.</param>
	/// <param name="positions">The z rotation of the entities.</param>
	public void DisplayEntities(List<Vector2> positions, Color? color = null, bool enableEmission = true, float rotation = 0)
	{
		// For each position, create an entity
		foreach (Vector2 position in positions)
		{
			// Normalize position to fit the grid scale
			Vector2 normalizedPosition = position * 0.5f;

			// Create a new entity instance
			GameObject entity = Instantiate(EntityTemplate, new Vector3(normalizedPosition.x, normalizedPosition.y, 0), Quaternion.Euler(new Vector3(0, 0, rotation)), EntitiesParent.transform);

			// Extract the script
			GridEntity entityScript = entity.GetComponent<GridEntity>();

			// Set color
			if (color.HasValue)
				entityScript.SetColor(color.Value, enableEmission);
		}
	}

	#endregion

}
