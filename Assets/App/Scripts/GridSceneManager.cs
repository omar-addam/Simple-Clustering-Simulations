using Clustering.Core;
using Clustering.KMeans;
using Clustering.KMedoids;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
		foreach (var cluster in AlgorithmManager.CurrentAlgorithm.Iterations.First().Clusters)
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
		GridManager.Clear();

		// Display seed items
		List<Vector2> seedItems = AlgorithmManager.CurrentAlgorithm.Items.Select(x => new Vector2(x.PositionX, x.PositionY)).ToList();
		if (AlgorithmManager.CurrentAlgorithm is KMedoidsAlgorithm)
		{
			KMedoidsAlgorithm algorithm = AlgorithmManager.CurrentAlgorithm as KMedoidsAlgorithm;
			seedItems = algorithm.Items.Where(x => !algorithm.Clusters.Contains(x.Id)).Select(x => new Vector2(x.PositionX, x.PositionY)).ToList();
		}
		GridManager.DisplayEntities(seedItems, Color.white);

		// Display clusters
		if (AlgorithmManager.CurrentAlgorithm is KMeansAlgorithm)
		{
			KMeansAlgorithm algorithm = AlgorithmManager.CurrentAlgorithm as KMeansAlgorithm;
			foreach (Item cluster in algorithm.ClusterSeeds)
				GridManager.DisplayEntities(new List<Vector2>() { new Vector2(cluster.PositionX, cluster.PositionY) }, ClusterColors[cluster.Id], true, 45f);
		}
		else if (AlgorithmManager.CurrentAlgorithm is KMedoidsAlgorithm)
		{
			KMedoidsAlgorithm algorithm = AlgorithmManager.CurrentAlgorithm as KMedoidsAlgorithm;

			foreach (KMedoidsCluster cluster in algorithm.Iterations.First().Clusters)
			{
				Item item = cluster.Centroid;
				GridManager.DisplayEntities(new List<Vector2>() { new Vector2(item.PositionX, item.PositionY) }, ClusterColors[cluster.Id], true, 45f);
			}
		}
	}

	/// <summary>
	/// Initializes the slider.
	/// </summary>
	private void InitializeIterationsSlider()
	{
		IterationsUIText.text = string.Format("Iteration: {0} / {1}", 0, AlgorithmManager.CurrentAlgorithm.Iterations.Count - 1);
		IterationsSlider.maxValue = AlgorithmManager.CurrentAlgorithm.Iterations.Count - 1;
		IterationsSlider.onValueChanged.AddListener((float value) =>
		{
			IterationsUIText.text = string.Format("Iteration: {0} / {1}", value, AlgorithmManager.CurrentAlgorithm.Iterations.Count - 1);

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
		GridManager.Clear();

		// Find the iteration
		Iteration iteration = AlgorithmManager.CurrentAlgorithm.Iterations.FirstOrDefault(x => x.Order == order);
		if (iteration == null)
			return;

		// Display
		DisplayIterationEntities(iteration);
		DisplayIterationPaths(iteration);
	}

	/// <summary>
	/// Displays the entities of an iteration.
	/// </summary>
	private void DisplayIterationEntities(Iteration iteration)
	{
		// Clear all grid entities
		GridManager.Clear();

		// Go through each cluster
		foreach (Cluster cluster in iteration.Clusters)
		{
			// Display its items
			List<Vector2> seedItems = cluster.Items.Select(x => new Vector2(x.PositionX, x.PositionY)).ToList();
			if (cluster is KMedoidsCluster)
			{
				KMedoidsCluster kmedoidsCluster = cluster as KMedoidsCluster;
				seedItems = kmedoidsCluster.Items.Where(x => x.Id != kmedoidsCluster.ItemId).Select(x => new Vector2(x.PositionX, x.PositionY)).ToList();
			}
			GridManager.DisplayEntities(seedItems, ClusterColors[cluster.Id]);

			// Display cluster
			if (cluster is KMeansCluster)
			{
				KMeansCluster kmeanCluster = cluster as KMeansCluster;
				GridManager.DisplayEntities(new List<Vector2>() { new Vector2(kmeanCluster.CenterX, kmeanCluster.CenterY) }, ClusterColors[cluster.Id], false, 45f);
			}
			else if(cluster is KMedoidsCluster)
			{
				KMedoidsCluster kmedoidsCluster = cluster as KMedoidsCluster;
				GridManager.DisplayEntities(new List<Vector2>() { new Vector2(kmedoidsCluster.Centroid.PositionX, kmedoidsCluster.Centroid.PositionY) }, ClusterColors[cluster.Id], false, 45f);
			}
		}
	}

	/// <summary>
	/// Displays the paths of clusters till this iteration.
	/// </summary>
	private void DisplayIterationPaths(Iteration iteration)
	{
		// Initialize the path list
		Dictionary<Guid, List<Vector2>> clusterPaths = new Dictionary<Guid, List<Vector2>>();

		// Populate with the list of clusters
		foreach (Cluster cluster in iteration.Clusters)
			clusterPaths.Add(cluster.Id, new List<Vector2>());

		// Get the history of each cluster
		foreach (Cluster cluster in iteration.Clusters)
		{
			List<Cluster> history = new List<Cluster>();

			// Go through each iteration and find this cluster
			foreach (var it in AlgorithmManager.CurrentAlgorithm.Iterations)
				if (it.Order <= iteration.Order)
				{
					Cluster historyCluster = it.Clusters.FirstOrDefault(x => x.Id == cluster.Id);
					if (historyCluster != null)
						history.Add(historyCluster);
				}

			// Calculate paths
			if (cluster is KMeansCluster)
				foreach (KMeansCluster historyCluster in history)
					clusterPaths[cluster.Id].Add(new Vector2(historyCluster.CenterX * 0.5f, historyCluster.CenterY * 0.5f));
			else if (cluster is KMedoidsCluster)
				foreach (KMedoidsCluster historyCluster in history)
					clusterPaths[cluster.Id].Add(new Vector2(historyCluster.Centroid.PositionX * 0.5f, historyCluster.Centroid.PositionY * 0.5f));
		}

		// Display paths
		foreach (Guid clusterId in clusterPaths.Keys)
			GridManager.DisplayPaths(clusterPaths[clusterId], ClusterColors[clusterId]);
	}

	#endregion

}
