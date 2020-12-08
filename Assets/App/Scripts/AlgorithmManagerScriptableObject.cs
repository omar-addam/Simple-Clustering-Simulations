using Clustering.Core;
using Clustering.KMean;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AlgorithmData", menuName = "ScriptableObjects/AlgorithmManagerScriptableObject", order = 1)]
public class AlgorithmManagerScriptableObject : ScriptableObject
{
    
    /// <summary>
    /// References the algorithm currently used.
    /// </summary>
    public AbstractAlgorithm CurrentAlgorithm { private set; get; }

    /// <summary>
    /// KM-Mean algorithm.
    /// </summary>
    [SerializeField]
    private KMeanAlgorithm KMeanAlgorithm;

    /// <summary>
    /// Selects the k-mean algorithm as the current used algorithm.
    /// </summary>
    public void SelectKMeanAlgorithm()
    {
        CurrentAlgorithm = KMeanAlgorithm;
    }

    /// <summary>
    /// Clears all algorithms.
    /// </summary>
    public void Clear()
    {
        KMeanAlgorithm = new KMeanAlgorithm();
        CurrentAlgorithm = null;
    }

}
