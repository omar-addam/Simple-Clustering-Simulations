using Clustering.Core;
using Clustering.KMean;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro.EditorUtilities;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GridSceneManager : MonoBehaviour
{

	#region Variables

	/// <summary>
	/// References the grid in the scene.
	/// </summary>
	public Grid GridManager;

	/// <summary>
	/// References the title UI text element.
	/// </summary>
	public Text TitleUIText;

	/// <summary>
	/// References the iterations title UI text element.
	/// </summary>
	public Text IterationsUIText;

	/// <summary>
	/// References thje iterations slider UI element.
	/// </summary>
	public Slider IterationsSlider;

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
		// Display title
		TitleUIText.text = AlgorithmManager.CurrentAlgorithm.ToString();

		// Display iterations
		InitializeIterationsSlider();

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
			Color color = UnityEngine.Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
			ClusterColors.Add(cluster.Id, color);
		}
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

	/// <summary>
	/// Initializes the slider.
	/// </summary>
	private void InitializeIterationsSlider()
	{
		IterationsUIText.text = string.Format("Iteration: {0} / {1}", 0, AlgorithmManager.CurrentAlgorithm.AlgorithmIterations.Count);
		IterationsSlider.maxValue = AlgorithmManager.CurrentAlgorithm.AlgorithmIterations.Count;
		IterationsSlider.onValueChanged.AddListener((float value) =>
		{
			IterationsUIText.text = string.Format("Iteration: {0} / {1}", value, AlgorithmManager.CurrentAlgorithm.AlgorithmIterations.Count);

			// Display iteration entities
			DisplayIteration((int)value);
		});
	}

	/// <summary>
	/// Loads the introduction scene.
	/// </summary>
	public void LoadIntroductionScene()
	{
		SceneManager.LoadScene("IntroductionScene", LoadSceneMode.Single);
	}

	/// <summary>
	/// Displays the entities of an iteration.
	/// </summary>
	private void DisplayIteration(int order)
	{
		// Check if we should display seeds
		if (order == 0)
		{
			DisplaySeeds();
			return;
		}

		// Clear all displayed entities
		GridManager.ClearEntities();

		// Find the iteration
		Iteration iteration = AlgorithmManager.CurrentAlgorithm.AlgorithmIterations.FirstOrDefault(x => x.IterationOrder == order);
	}

	#endregion

}
