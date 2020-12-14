using Clustering.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class AbstractGridVisualizer
{

    #region Constructors

    /// <summary>
    /// Default constructor.
    /// </summary>
    public AbstractGridVisualizer(
        AlgorithmManagerScriptableObject algorithmManager,
        Grid gridManager)
    {
        // Reference variables
        AlgorithmManager = algorithmManager;
        GridManager = gridManager;

        // Create unique color for each cluster
        InitializeClusterColors();

        // Display seed entities
        DisplaySeeds();
    }

    #endregion

    #region Fields/Properties

    /// <summary>
    /// References the algorithm scriptable object.
    /// </summary>
    protected AlgorithmManagerScriptableObject AlgorithmManager { private set; get; }

    /// <summary>
    /// References the grid in the scene.
    /// </summary>
    protected Grid GridManager;

    /// <summary>
    /// The color associated with each cluster.
    /// </summary>
    protected Dictionary<Guid, Color> ClusterColors { private set; get; }

    #endregion

    #region Color Methods

    /// <summary>
    /// Associates clusters with unique colors.
    /// </summary>
    private void InitializeClusterColors()
    {
        ClusterColors = new Dictionary<Guid, Color>();
        foreach (var id in GetClusterIds())
            ClusterColors.Add(id, UnityEngine.Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f));
    }

    /// <summary>
    /// Gets all cluster ids.
    /// </summary>
    protected abstract List<Guid> GetClusterIds();

    #endregion

    #region Seed Methods

    /// <summary>
    /// Displays the seed entities. (Iteration = 0)
    /// </summary>
    private void DisplaySeeds()
    {
        // Clear all grid entities
        GridManager.Clear();

        // Display items
        List<Vector2> items = GetSeedItems()
            .Select(x => new Vector2(x.PositionX, x.PositionY)).ToList();
        GridManager.DisplayEntities(items, Color.white);

        // Display clusters
        List<Item> clusters = GetSeedClusters();
        foreach (Item cluster in clusters)
            GridManager.DisplayEntities
            (
                positions: new List<Vector2>() { new Vector2(cluster.PositionX, cluster.PositionY) },
                color: ClusterColors[cluster.Id], 
                enableEmission: true,
                rotation: 45f
            );
    }

    /// <summary>
    /// Retrieves the list of items at iteration 0.
    /// </summary>
    protected virtual List<Item> GetSeedItems()
    {
        return AlgorithmManager.CurrentAlgorithm.Items;
    }

    /// <summary>
    /// Retrieves the list of clusters at iteration 0.
    /// </summary>
    protected abstract List<Item> GetSeedClusters();

    #endregion

    #region Iteration Methods

    /// <summary>
    /// Gets the number of steps that we will display.
    /// </summary>
    public virtual int GetIterationsCount()
    {
        return AlgorithmManager.CurrentAlgorithm.Iterations.Count;
    }

    /// <summary>
    /// Displays the entities of an iteration.
    /// </summary>
    public void DisplayIteration(int iterationNumber)
    {
        // Check if we should display seeds
        if (iterationNumber == 0)
        {
            DisplaySeeds();
            return;
        }

        // Clear all displayed entities
        GridManager.Clear();

        // Get order
        int order = GetIterationOrder(iterationNumber);

        // Find the iterations
        Iteration previousIteration = AlgorithmManager.CurrentAlgorithm.Iterations.FirstOrDefault(x => x.Order == order - 1);
        Iteration currentIteration = AlgorithmManager.CurrentAlgorithm.Iterations.FirstOrDefault(x => x.Order == order);

        // Display iterations
        DisplayIteration(previousIteration, currentIteration);
    }

    /// <summary>
    /// Retrieves the order of the iteration.
    /// </summary>
    protected virtual int GetIterationOrder(int iterationNumber)
    {
        return iterationNumber;
    }

    /// <summary>
    /// Displays the entities of an iteration.
    /// </summary>
    protected abstract void DisplayIteration(Iteration previousIteration, Iteration currentIteration);

    #endregion

}
