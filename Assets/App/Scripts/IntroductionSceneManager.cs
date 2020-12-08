using Clustering.KMean;
using System.Collections;
using System.Collections.Generic;
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
		// TODO: create sample
	}

	#endregion

}
