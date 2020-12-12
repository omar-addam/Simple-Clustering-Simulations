using Clustering.Core;
using Clustering.DBScan;
using Clustering.KMeans;
using Clustering.KMedoids;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroductionSceneManager : MonoBehaviour
{

	#region Variables

	/// <summary>
	/// References the algorithm scriptable object instance.
	/// </summary>
	public AlgorithmManagerScriptableObject AlgorithmManager;

	#endregion

	#region Initialization

	/// <summary>
	/// Executes once on start.
	/// </summary>
	private void Awake()
	{
		AlgorithmManager.Clear();
	}

	#endregion

	#region K-Means Methods

	/// <summary>
	/// Selects the k-means algorithm and runs it against an example sample.
	/// </summary>
	public void SelectKMeansSample()
	{
		Debug.Log("K-Means algorithm has been selected to run against a predefined sample.");

		// Select k-mean algorithm
		AlgorithmManager.Clear();
		AlgorithmManager.SelectKMeansAlgorithm();

		// Create sample
		GenerateKMeansSample((KMeansAlgorithm)AlgorithmManager.CurrentAlgorithm);

		// Compute iterations
		AlgorithmManager.CurrentAlgorithm.Compute();

		// Switch to grid scene
		SceneManager.LoadScene("GridScene", LoadSceneMode.Single);
	}

	/// <summary>
	/// Creates an example sample.
	/// </summary>
	private void GenerateKMeansSample(KMeansAlgorithm algorithm)
	{
		// Create items sample
		algorithm.Items.Add(new Item(4, 5));
		algorithm.Items.Add(new Item(3, 7));
		algorithm.Items.Add(new Item(4.5f, 4));
		algorithm.Items.Add(new Item(5, 6));

		algorithm.Items.Add(new Item(-1, -1));
		algorithm.Items.Add(new Item(-4, -3));
		algorithm.Items.Add(new Item(-4, -1.5f));
		algorithm.Items.Add(new Item(-5, -1));
		algorithm.Items.Add(new Item(-5, -2));
		algorithm.Items.Add(new Item(-6, -1));
		algorithm.Items.Add(new Item(-6, -2));

		// Create clusters sample
		algorithm.ClusterSeeds.Add(new Item(2f, 3f));
		algorithm.ClusterSeeds.Add(new Item(-7f, -3f));
	}

	#endregion

	#region K-Medoids Methods

	/// <summary>
	/// Selects the k-medoids algorithm and runs it against an example sample.
	/// </summary>
	public void SelectKMedoidsSample()
	{
		Debug.Log("K-Medoids algorithm has been selected to run against a predefined sample.");

		// Select k-mean algorithm
		AlgorithmManager.Clear();
		AlgorithmManager.SelectKMedoidsAlgorithm();

		// Create sample
		GenerateKMedoidsSample((KMedoidsAlgorithm)AlgorithmManager.CurrentAlgorithm);

		// Compute iterations
		AlgorithmManager.CurrentAlgorithm.Compute();

		// Switch to grid scene
		SceneManager.LoadScene("GridScene", LoadSceneMode.Single);
	}

	/// <summary>
	/// Creates an example sample.
	/// </summary>
	private void GenerateKMedoidsSample(KMedoidsAlgorithm algorithm)
	{
		// Create items sample
		algorithm.Items.Add(new Item(4, 5));
		algorithm.Items.Add(new Item(3, 7));
		algorithm.Items.Add(new Item(4.5f, 4));
		algorithm.Items.Add(new Item(5, 6));

		algorithm.Items.Add(new Item(-1, -1));
		algorithm.Items.Add(new Item(-4, -3));
		algorithm.Items.Add(new Item(-4, -1.5f));
		algorithm.Items.Add(new Item(-5, -1));
		algorithm.Items.Add(new Item(-5, -2));
		algorithm.Items.Add(new Item(-6, -1));
		algorithm.Items.Add(new Item(-6, -2));

		// Create clusters sample
		algorithm.ClusterSeeds.Add(algorithm.Items[4].Id);
		algorithm.ClusterSeeds.Add(algorithm.Items[7].Id);
	}

	#endregion

	#region DB-Scan Methods

	/// <summary>
	/// Selects the db-scan algorithm and runs it against an example sample.
	/// </summary>
	public void SelecDBScanSample()
	{
		Debug.Log("DB-Scan algorithm has been selected to run against a predefined sample.");

		// Select k-mean algorithm
		AlgorithmManager.Clear();
		AlgorithmManager.SelectDBScanAlgorithm(3, 2);

		// Create sample
		GenerateDBScanSample((DBScanAlgorithm)AlgorithmManager.CurrentAlgorithm);

		// Compute iterations
		AlgorithmManager.CurrentAlgorithm.Compute();

		// Switch to grid scene
		SceneManager.LoadScene("GridScene", LoadSceneMode.Single);
	}

	/// <summary>
	/// Creates an example sample.
	/// </summary>
	private void GenerateDBScanSample(DBScanAlgorithm algorithm)
	{
		// Create items sample
		algorithm.Items.Add(new Item(4, 5));
		algorithm.Items.Add(new Item(3, 7));
		algorithm.Items.Add(new Item(4.5f, 4));
		algorithm.Items.Add(new Item(5, 6));

		algorithm.Items.Add(new Item(-1, -1));
		algorithm.Items.Add(new Item(-4, -3));
		algorithm.Items.Add(new Item(-4, -1.5f));
		algorithm.Items.Add(new Item(-5, -1));
		algorithm.Items.Add(new Item(-5, -2));
		algorithm.Items.Add(new Item(-6, -1));
		algorithm.Items.Add(new Item(-6, -2));
	}

	#endregion

}
