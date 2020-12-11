using Clustering.Core;
using Clustering.KMeans;
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
    private KMeansAlgorithm KMeansAlgorithm;

    /// <summary>
    /// K-Medoids algorithm.
    /// </summary>
    [SerializeField]
    private KMedoidsAlgorithm KMedoidsAlgorithm;

    /// <summary>
    /// Selects the k-means algorithm as the current used algorithm.
    /// </summary>
    public void SelectKMeansAlgorithm()
    {
        CurrentAlgorithm = KMeansAlgorithm;
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
        KMeansAlgorithm = new KMeansAlgorithm();
        KMedoidsAlgorithm = new KMedoidsAlgorithm();
        CurrentAlgorithm = null;
    }

}
