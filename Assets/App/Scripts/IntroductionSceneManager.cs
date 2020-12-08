using Clustering.Core;
using Clustering.KMean;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;

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

		// TODO: Create sample
		GenerateKMeanSample((KMeanAlgorithm)AlgorithmManager.CurrentAlgorithm);

		// Compute iterations
		AlgorithmManager.CurrentAlgorithm.Compute();
	}

	/// <summary>
	/// Creates an example sample.
	/// </summary>
	private void GenerateKMeanSample(KMeanAlgorithm algorithm)
	{
		// Create items sample
		algorithm.AlgorithmItems.Add(new Item(1, 1));
		algorithm.AlgorithmItems.Add(new Item(1, 2));
		algorithm.AlgorithmItems.Add(new Item(2, 1));
		algorithm.AlgorithmItems.Add(new Item(2, 2));

		algorithm.AlgorithmItems.Add(new Item(5, 1));
		algorithm.AlgorithmItems.Add(new Item(5, 2));
		algorithm.AlgorithmItems.Add(new Item(6, 1));
		algorithm.AlgorithmItems.Add(new Item(6, 2));

		// Create clusters sample
		algorithm.Clusters.Add(new Item(1.5f, 1.5f));
		algorithm.Clusters.Add(new Item(5.5f, 1.5f));
	}

	#endregion

}
