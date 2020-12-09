using Clustering.Core;
using Clustering.KMean;
using System;
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

	/// <summary>
	/// The color associated with each cluster.
	/// </summary>
	private Dictionary<Guid, Color> ClusterColors;

	#endregion

	#region Initialization

	/// <summary>
	/// Executes once on start.
	/// </summary>
	private void Awake()
	{
		// Associate clusters with colors
		InitializeClusterColors();

		// Display seed items and clusters
		DisplaySeeds();
	}

	#endregion

	#region Methods

	/// <summary>
	/// Associates clusters with unique colors.
	/// </summary>
	private void InitializeClusterColors()
	{
		ClusterColors = new Dictionary<Guid, Color>();
		foreach (var cluster in AlgorithmManager.CurrentAlgorithm.AlgorithmIterations.First().IterationClusters)
		{
			Color color = GetNextPseudoRandomColor();
			ClusterColors.Add(cluster.Id, color);
		}
	}

	/// <summary>
	/// Generates random color.
	/// </summary>
	private Color GetNextPseudoRandomColor()
	{
		int keep = new System.Random().Next(0, 2);
		float red = UnityEngine.Random.Range(0f, 1f);
		float green = UnityEngine.Random.Range(0f, 1f);
		float blue = UnityEngine.Random.Range(0f, 1f);
		Color c = new Color(red, green, blue);
		float fixedComp = c[keep] + 0.5f;
		c[keep] = fixedComp - Mathf.Floor(fixedComp);
		return c;
	}

	/// <summary>
	/// Displays the seed entities.
	/// </summary>
	private void DisplaySeeds()
	{
		// Clear all grid entities
		GridManager.ClearEntities();

		// Display seed items
		List<Vector2> seedItems = AlgorithmManager.CurrentAlgorithm.AlgorithmItems.Select(x => new Vector2(x.PositionX, x.PositionY)).ToList();
		GridManager.DisplayEntities(seedItems, Color.white);

		// Check if k-mean
		if (AlgorithmManager.CurrentAlgorithm is KMeanAlgorithm)
		{
			KMeanAlgorithm algorithm = AlgorithmManager.CurrentAlgorithm as KMeanAlgorithm;

			// Display seed clusters
			foreach (Item cluster in algorithm.Clusters)
				GridManager.DisplayEntities(new List<Vector2>() { new Vector2(cluster.PositionX, cluster.PositionY) }, ClusterColors[cluster.Id], 45f);
		}
	}

	#endregion

}
