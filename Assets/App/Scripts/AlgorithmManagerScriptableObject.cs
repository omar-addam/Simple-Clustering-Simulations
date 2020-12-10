using Clustering.Core;
using Clustering.KMean;
using Clustering.KMedoids;
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
    /// K-Mean algorithm.
    /// </summary>
    [SerializeField]
    private KMeanAlgorithm KMeanAlgorithm;

    /// <summary>
    /// K-Medoids algorithm.
    /// </summary>
    [SerializeField]
    private KMedoidsAlgorithm KMedoidsAlgorithm;

    /// <summary>
    /// Selects the k-mean algorithm as the current used algorithm.
    /// </summary>
    public void SelectKMeanAlgorithm()
    {
        CurrentAlgorithm = KMeanAlgorithm;
    }

    /// <summary>
    /// Selects the k-medoids algorithm as the current used algorithm.
    /// </summary>
    public void SelectKMedoidsAlgorithm()
    {
        CurrentAlgorithm = KMedoidsAlgorithm;
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
