using Clustering.Core;
using Clustering.KMean;
using Clustering.KMedoids;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroductionSceneManager : MonoBehaviour
{

	#region Variables

	/// <summary>
	/// References the algorithm data instance.
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

	#region K-Mean Methods

	/// <summary>
	/// Selects the k-mean algorithm and runs it against an example sample.
	/// </summary>
	public void SelectKMeanSample()
	{
		Debug.Log("K-Mean algorithm has been selected to run against a predefined sample.");

		// Select k-mean algorithm
		AlgorithmManager.Clear();
		AlgorithmManager.SelectKMeanAlgorithm();

		// Create sample
		GenerateKMeanSample((KMeanAlgorithm)AlgorithmManager.CurrentAlgorithm);

		// Compute iterations
		AlgorithmManager.CurrentAlgorithm.Compute();

		// Switch to grid scene
		SceneManager.LoadScene("GridScene", LoadSceneMode.Single);
	}

	/// <summary>
	/// Creates an example sample.
	/// </summary>
	private void GenerateKMeanSample(KMeanAlgorithm algorithm)
	{
		// Create items sample
		algorithm.AlgorithmItems.Add(new Item(4, 5));
		algorithm.AlgorithmItems.Add(new Item(3, 7));
		algorithm.AlgorithmItems.Add(new Item(4.5f, 4));
		algorithm.AlgorithmItems.Add(new Item(5, 6));

		algorithm.AlgorithmItems.Add(new Item(-1, -1));
		algorithm.AlgorithmItems.Add(new Item(-4, -3));
		algorithm.AlgorithmItems.Add(new Item(-4, -1.5f));
		algorithm.AlgorithmItems.Add(new Item(-5, -1));
		algorithm.AlgorithmItems.Add(new Item(-5, -2));
		algorithm.AlgorithmItems.Add(new Item(-6, -1));
		algorithm.AlgorithmItems.Add(new Item(-6, -2));

		// Create clusters sample
		algorithm.Clusters.Add(new Item(2f, 3f));
		algorithm.Clusters.Add(new Item(-7f, -3f));
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
		algorithm.AlgorithmItems.Add(new Item(4, 5));
		algorithm.AlgorithmItems.Add(new Item(3, 7));
		algorithm.AlgorithmItems.Add(new Item(4.5f, 4));
		algorithm.AlgorithmItems.Add(new Item(5, 6));

		algorithm.AlgorithmItems.Add(new Item(-1, -1));
		algorithm.AlgorithmItems.Add(new Item(-4, -3));
		algorithm.AlgorithmItems.Add(new Item(-4, -1.5f));
		algorithm.AlgorithmItems.Add(new Item(-5, -1));
		algorithm.AlgorithmItems.Add(new Item(-5, -2));
		algorithm.AlgorithmItems.Add(new Item(-6, -1));
		algorithm.AlgorithmItems.Add(new Item(-6, -2));

		// Create clusters sample
		algorithm.Clusters.Add(algorithm.AlgorithmItems[4].Id);
		algorithm.Clusters.Add(algorithm.AlgorithmItems[7].Id);
	}

	#endregion

}
