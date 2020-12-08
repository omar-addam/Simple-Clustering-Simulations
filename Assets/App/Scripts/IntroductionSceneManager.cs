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

}
