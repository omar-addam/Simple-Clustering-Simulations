using Clustering.Core;
using Clustering.DBScan;
using Clustering.KMeans;
using Clustering.KMedoids;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TextCore.LowLevel;
using UnityEngine.UI;

public class IntroductionSceneManager : MonoBehaviour
{

	#region Variables

	/// <summary>
	/// References the algorithm scriptable object instance.
	/// </summary>
	public AlgorithmManagerScriptableObject AlgorithmManager;

	/// <summary>
	/// The label displaying the version of the application.
	/// </summary>
	[SerializeField]
	private Text VersionText;

	#endregion

	#region Initialization

	/// <summary>
	/// Executes once on start.
	/// </summary>
	private void Awake()
	{
		AlgorithmManager.Clear();
		VersionText.text = Application.version;
	}

	#endregion

	#region K-Means Methods

	/// <summary>
	/// Selects the k-means algorithm and runs it against an example sample.
	/// </summary>
	public void SelectKMeansSample(int sample)
	{
		Debug.Log("K-Means algorithm has been selected to run against a predefined sample.");

		// Select k-mean algorithm
		AlgorithmManager.Clear();
		AlgorithmManager.SelectKMeansAlgorithm();

		// Create sample
		if (sample == 1)
			GenerateKMeansSample1((KMeansAlgorithm)AlgorithmManager.CurrentAlgorithm);
		else
			GenerateKMeansSample2((KMeansAlgorithm)AlgorithmManager.CurrentAlgorithm);

		// Compute iterations
		AlgorithmManager.CurrentAlgorithm.Compute();

		// Switch to grid scene
		SceneManager.LoadScene("GridScene", LoadSceneMode.Single);
	}

	/// <summary>
	/// Creates an example sample.
	/// </summary>
	private void GenerateKMeansSample1(KMeansAlgorithm algorithm)
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
		algorithm.ClusterSeeds.Add(new Item(-6f, -3f));
	}


	/// <summary>
	/// Creates an example sample.
	/// </summary>
	private void GenerateKMeansSample2(KMeansAlgorithm algorithm)
	{
		// Center
		int centerWidth = 3;
		for (int i = -centerWidth; i <= centerWidth; i++)
		{
			// Add + (horizontal and vertical lines passing through the center)
			algorithm.Items.Add(new Item(i, 0));
			algorithm.Items.Add(new Item(0, i));

			// Add /\ (diagonal lines passing through the center)
			if (i > -centerWidth && i < centerWidth && i != 0)
				algorithm.Items.Add(new Item(i, i));

			// Add the rest
			if (i > -centerWidth && i < centerWidth && i != 0)
				algorithm.Items.Add(new Item(-i, i));
		}

		// Edge
		int edgeStart = 7;
		int edgeWidth = 2;
		for (int i = -edgeStart; i <= edgeStart; i++)
		{
			for (int j = 0; j < edgeWidth; j++)
			{
				algorithm.Items.Add(new Item((edgeStart + j), i));
				algorithm.Items.Add(new Item(-(edgeStart + j), i));
				algorithm.Items.Add(new Item(i, (edgeStart + j)));
				algorithm.Items.Add(new Item(i, -(edgeStart + j)));
			}
		}

		// Create clusters sample
		algorithm.ClusterSeeds.Add(new Item(2f, 3f));
		algorithm.ClusterSeeds.Add(new Item(-6f, -3f));
	}

	#endregion

	#region K-Medoids Methods

	/// <summary>
	/// Selects the k-medoids algorithm and runs it against an example sample.
	/// </summary>
	public void SelectKMedoidsSample(int sample)
	{
		Debug.Log("K-Medoids algorithm has been selected to run against a predefined sample.");

		// Select k-mean algorithm
		AlgorithmManager.Clear();
		AlgorithmManager.SelectKMedoidsAlgorithm();

		// Create sample
		if (sample == 1)
			GenerateKMedoidsSample1((KMedoidsAlgorithm)AlgorithmManager.CurrentAlgorithm);
		else
			GenerateKMedoidsSample2((KMedoidsAlgorithm)AlgorithmManager.CurrentAlgorithm);
		
		// Compute iterations
		AlgorithmManager.CurrentAlgorithm.Compute();

		// Switch to grid scene
		SceneManager.LoadScene("GridScene", LoadSceneMode.Single);
	}

	/// <summary>
	/// Creates an example sample.
	/// </summary>
	private void GenerateKMedoidsSample1(KMedoidsAlgorithm algorithm)
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

	/// <summary>
	/// Creates an example sample.
	/// </summary>
	private void GenerateKMedoidsSample2(KMedoidsAlgorithm algorithm)
	{
		// Center
		int centerWidth = 3;
		for (int i = -centerWidth; i <= centerWidth; i++)
		{
			// Add + (horizontal and vertical lines passing through the center)
			algorithm.Items.Add(new Item(i, 0));
			algorithm.Items.Add(new Item(0, i));

			// Add /\ (diagonal lines passing through the center)
			if (i > -centerWidth && i < centerWidth && i != 0)
				algorithm.Items.Add(new Item(i, i));

			// Add the rest
			if (i > -centerWidth && i < centerWidth && i != 0)
				algorithm.Items.Add(new Item(-i, i));
		}

		// Edge
		int edgeStart = 7;
		int edgeWidth = 2;
		for (int i = -edgeStart; i <= edgeStart; i++)
		{
			for (int j = 0; j < edgeWidth; j++)
			{
				algorithm.Items.Add(new Item((edgeStart + j), i));
				algorithm.Items.Add(new Item(-(edgeStart + j), i));
				algorithm.Items.Add(new Item(i, (edgeStart + j)));
				algorithm.Items.Add(new Item(i, -(edgeStart + j)));
			}
		}

		// Create clusters sample
		algorithm.ClusterSeeds.Add(algorithm.Items[4].Id);
		algorithm.ClusterSeeds.Add(algorithm.Items[7].Id);
	}

	#endregion

	#region DB-Scan Methods

	/// <summary>
	/// Selects the db-scan algorithm and runs it against an example sample.
	/// </summary>
	public void SelecDBScanSample(int sample)
	{
		Debug.Log("DB-Scan algorithm has been selected to run against a predefined sample.");

		// Select k-mean algorithm
		AlgorithmManager.Clear();
		AlgorithmManager.SelectDBScanAlgorithm(2, 2);

		// Create sample
		if (sample == 1)
			GenerateDBScanSample1((DBScanAlgorithm)AlgorithmManager.CurrentAlgorithm);
		else
			GenerateDBScanSample2((DBScanAlgorithm)AlgorithmManager.CurrentAlgorithm);

		// Compute iterations
		AlgorithmManager.CurrentAlgorithm.Compute();

		// Switch to grid scene
		SceneManager.LoadScene("GridScene", LoadSceneMode.Single);
	}

	/// <summary>
	/// Creates an example sample.
	/// </summary>
	private void GenerateDBScanSample1(DBScanAlgorithm algorithm)
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

	/// <summary>
	/// Creates an example sample.
	/// </summary>
	private void GenerateDBScanSample2(DBScanAlgorithm algorithm)
	{
		// Center
		int centerWidth = 3;
		for (int i = -centerWidth; i <= centerWidth; i++)
		{
			// Add + (horizontal and vertical lines passing through the center)
			algorithm.Items.Add(new Item(i, 0));
			algorithm.Items.Add(new Item(0, i));

			// Add /\ (diagonal lines passing through the center)
			if (i > -centerWidth && i < centerWidth && i != 0)
				algorithm.Items.Add(new Item(i, i));

			// Add the rest
			if (i > -centerWidth && i < centerWidth && i != 0)
				algorithm.Items.Add(new Item(-i, i));
		}

		// Edge
		int edgeStart = 7;
		int edgeWidth = 2;
		for (int i = -edgeStart; i <= edgeStart; i++)
		{
			for (int j = 0; j < edgeWidth; j++)
			{
				algorithm.Items.Add(new Item((edgeStart + j), i));
				algorithm.Items.Add(new Item(-(edgeStart + j), i));
				algorithm.Items.Add(new Item(i, (edgeStart + j)));
				algorithm.Items.Add(new Item(i, -(edgeStart + j)));
			}
		}
	}

	#endregion

}
