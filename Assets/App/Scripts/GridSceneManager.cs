﻿using Clustering.Core;
using Clustering.DBScan;
using Clustering.KMeans;
using Clustering.KMedoids;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
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

	#region Flow

	/// <summary>
	/// Associates clusters with unique colors.
	/// </summary>
	private void InitializeClusterColors()
	{
		ClusterColors = new Dictionary<Guid, Color>();

		List<Guid> clusterIds = new List<Guid>();

		if (AlgorithmManager.CurrentAlgorithm is KMeansAlgorithm
			|| AlgorithmManager.CurrentAlgorithm is KMedoidsAlgorithm)
			clusterIds = GetKMClusterIds();
		else if (AlgorithmManager.CurrentAlgorithm is DBScanAlgorithm)
			clusterIds = GetDBScanClusterIds();

		foreach (var id in clusterIds)
		{
			Color color = UnityEngine.Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
			ClusterColors.Add(id, color);
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
		List<Vector2> seedItems = new List<Vector2>();
		if (AlgorithmManager.CurrentAlgorithm is KMeansAlgorithm)
			seedItems = GetKMeansSeedItems().Select(x => new Vector2(x.PositionX, x.PositionY)).ToList();
		else if (AlgorithmManager.CurrentAlgorithm is KMedoidsAlgorithm)
			seedItems = GetKMedoidsSeedItems().Select(x => new Vector2(x.PositionX, x.PositionY)).ToList();
		else if (AlgorithmManager.CurrentAlgorithm is DBScanAlgorithm)
			seedItems = GetDBScanSeedItems().Select(x => new Vector2(x.PositionX, x.PositionY)).ToList();
		GridManager.DisplayEntities(seedItems, Color.white);

		// Display clusters
		List<Item> seedClusters = new List<Item>();
		if (AlgorithmManager.CurrentAlgorithm is KMeansAlgorithm)
			seedClusters = GetKMeansSeedCluster();
		else if (AlgorithmManager.CurrentAlgorithm is KMedoidsAlgorithm)
			seedClusters = GetKMedoidsSeedClusters();
		foreach (Item cluster in seedClusters)
			GridManager.DisplayEntities(new List<Vector2>() { new Vector2(cluster.PositionX, cluster.PositionY) }, ClusterColors[cluster.Id], true, 45f);
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
		DisplayIterationBoundaries(iteration);
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
				seedItems = kmedoidsCluster.Items.Where(x => x.Id != kmedoidsCluster.CenterId).Select(x => new Vector2(x.PositionX, x.PositionY)).ToList();
			}
			if (cluster is DBScanCluster)
			{
				DBScanCluster dbScanCluster = cluster as DBScanCluster;
				seedItems = dbScanCluster.Items.Except(dbScanCluster.RecentlyAddedItems).Select(x => new Vector2(x.PositionX, x.PositionY)).ToList();
			}
			GridManager.DisplayEntities(seedItems, ClusterColors[cluster.Id]);

			// Display cluster
			if (cluster is KMeansCluster)
			{
				KMeansCluster kmeanCluster = cluster as KMeansCluster;
				GridManager.DisplayEntities(new List<Vector2>() { new Vector2(kmeanCluster.CenterX, kmeanCluster.CenterY) }, ClusterColors[cluster.Id], false, 45f);
			}
			else if (cluster is KMedoidsCluster)
			{
				KMedoidsCluster kmedoidsCluster = cluster as KMedoidsCluster;
				GridManager.DisplayEntities(new List<Vector2>() { new Vector2(kmedoidsCluster.Centroid.PositionX, kmedoidsCluster.Centroid.PositionY) }, ClusterColors[cluster.Id], false, 45f);
			}
			else if (cluster is DBScanCluster)
			{
				DBScanCluster dbScanCluster = cluster as DBScanCluster;
				GridManager.DisplayEntities(dbScanCluster.RecentlyAddedItems.Select(x => new Vector2(x.PositionX, x.PositionY)).ToList(), ClusterColors[cluster.Id], false, 45f);
			}
		}

		// Display noises
		if (iteration is DBScanIteration)
		{
			DBScanIteration dbscanIteration = iteration as DBScanIteration;
			GridManager.DisplayEntities(dbscanIteration.Noise.Select(x => new Vector2(x.PositionX, x.PositionY)).ToList(), Color.black);
		}

		// Display pending
		if (iteration is DBScanIteration)
		{
			DBScanIteration dbscanIteration = iteration as DBScanIteration;
			GridManager.DisplayEntities(dbscanIteration.Pending.Select(x => new Vector2(x.PositionX, x.PositionY)).ToList(), Color.white);
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

	/// <summary>
	/// Displays the boundaries of clusters in this iteration.
	/// </summary>
	private void DisplayIterationBoundaries(Iteration iteration)
	{
		// Go through each cluster
		foreach (Cluster cluster in iteration.Clusters)
		{
			// In db scan, the boundaries are circles around the recently added neighbors
			if (cluster is DBScanCluster)
			{
				DBScanCluster dbScanCluster = cluster as DBScanCluster;
				foreach (var item in dbScanCluster.RecentlyAddedItems)
					GridManager.DisplayCircularBoundary(new Vector2(item.PositionX, item.PositionY), 2, ClusterColors[cluster.Id]);
			}
		}
	}

	#endregion

	#region K-Means and K-Medoids Flow Implementation

	// --- COLORS --- //

	/// <summary>
	/// Gets all cluster ids.
	/// </summary>
	private List<Guid> GetKMClusterIds()
	{
		// In KMeans and KMedoids, the numbher of clusters are defined at first
		return AlgorithmManager.CurrentAlgorithm.Iterations.First().Clusters.Select(x => x.Id).ToList();
	}

	// --- SEEDS --- //

	/// <summary>
	/// Retrieves the list of items at iteration 0.
	/// </summary>
	private List<Item> GetKMeansSeedItems()
	{
		return AlgorithmManager.CurrentAlgorithm.Items;
	}

	/// <summary>
	/// Retrieves the list of items at iteration 0.
	/// </summary>
	private List<Item> GetKMedoidsSeedItems()
	{
		KMedoidsAlgorithm algorithm = AlgorithmManager.CurrentAlgorithm as KMedoidsAlgorithm;
		return algorithm.Items.Where(x => !algorithm.ClusterSeeds.Contains(x.Id)).ToList();
	}

	/// <summary>
	/// Retrieves the list of clusters at iteration 0.
	/// </summary>
	private List<Item> GetKMeansSeedCluster()
	{
		KMeansAlgorithm algorithm = AlgorithmManager.CurrentAlgorithm as KMeansAlgorithm;
		return algorithm.ClusterSeeds;
	}

	/// <summary>
	/// Retrieves the list of clusters at iteration 0.
	/// </summary>
	private List<Item> GetKMedoidsSeedClusters()
	{
		KMedoidsAlgorithm algorithm = AlgorithmManager.CurrentAlgorithm as KMedoidsAlgorithm;
		return algorithm.Iterations.First().Clusters.Select(x => 
			{
				KMedoidsCluster cluster = x as KMedoidsCluster;
				return new Item(cluster.Id, cluster.Centroid.PositionX, cluster.Centroid.PositionY);
			}).ToList();
	}

	#endregion

	#region DB-Scan Flow Implementation

	// --- COLORS --- //

	/// <summary>
	/// Gets all cluster ids.
	/// </summary>
	private List<Guid> GetDBScanClusterIds()
	{
		// In DB-Scan, the final numbher of clusters is deteremined at the end
		return AlgorithmManager.CurrentAlgorithm.Iterations.Last().Clusters.Select(x => x.Id).ToList();
	}

	// --- SEEDS --- //

	/// <summary>
	/// Retrieves the list of items at iteration 0.
	/// </summary>
	private List<Item> GetDBScanSeedItems()
	{
		return AlgorithmManager.CurrentAlgorithm.Items;
	}

	#endregion

}
