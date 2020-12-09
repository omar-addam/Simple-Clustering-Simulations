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
	/// References the gameobject that will hold all gw3e
	/// </summary>
	public GameObject EntitiesParent;

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
	public void DisplayEntities(List<Vector2> positions)
	{
		// For each position, create an entity
		foreach (Vector2 position in positions)
		{
			// Create a new entity instance
			GameObject entity = Instantiate(EntityTemplate, new Vector3(position.x, position.y, 0), Quaternion.Euler(Vector3.zero), EntitiesParent.transform);

			// Extract the script
			GridEntity entityScript = entity.GetComponent<GridEntity>();
		}
	}

	#endregion

}
