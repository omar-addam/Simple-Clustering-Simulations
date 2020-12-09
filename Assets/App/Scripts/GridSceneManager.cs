using Clustering.Core;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GridSceneManager : MonoBehaviour
{

	#region Variables

	/// <summary>
	/// References the grid in the scene.
	/// </summary>
	public Grid GridManager;

	/// <summary>
	/// References the algorithm scriptable object.
	/// </summary>
	public AlgorithmManagerScriptableObject AlgorithmManager;

	#endregion

	#region Initialization

	/// <summary>
	/// Executes once on start.
	/// </summary>
	private void Awake()
	{
		// Display seed items and clusters
		DisplaySeeds();
	}

	#endregion

	#region Methods

	/// <summary>
	/// Displays the seed entities.
	/// </summary>
	private void DisplaySeeds()
	{
		// Clear all grid entities
		GridManager.ClearEntities();

		// Display seed items
		List<Vector2> seedItems = AlgorithmManager.CurrentAlgorithm.AlgorithmItems.Select(x => new Vector2(x.PositionX, x.PositionY)).ToList();
		GridManager.DisplayEntities(seedItems);
	}

	#endregion

}
