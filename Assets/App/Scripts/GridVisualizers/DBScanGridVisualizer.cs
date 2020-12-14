using Clustering.Core;
using Clustering.DBScan;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DBScanGridVisualizer : AbstractGridVisualizer
{

    #region Constructors

    /// <summary>
    /// Default constructor.
    /// </summary>
    public DBScanGridVisualizer(
        AlgorithmManagerScriptableObject algorithmManager,
        Grid gridManager)
        : base(algorithmManager, gridManager)
    {
    }

    #endregion

    #region Fields/Properties

    /// <summary>
    /// References the algorithm scriptable object.
    /// </summary>
    private DBScanAlgorithm Algorithm 
    {
        get
        {
            return (DBScanAlgorithm)AlgorithmManager.CurrentAlgorithm;
        }
    }

    #endregion

    #region Color Methods

    /// <summary>
    /// Gets all cluster ids.
    /// </summary>
    protected override List<Guid> GetClusterIds()
    {
        return Algorithm.Iterations.Last().Clusters.Select(x => x.Id).ToList();
    }

    #endregion

    #region Seed Methods

    /// <summary>
    /// Retrieves the list of items at iteration 0.
    /// </summary>
    protected override List<Item> GetSeedItems()
    {
        return Algorithm.Items;
    }

    /// <summary>
    /// Retrieves the list of clusters at iteration 0.
    /// </summary>
    protected override List<Item> GetSeedClusters()
    {
        return new List<Item>(); // There are no predefined clusters in DB-Scan
    }

    #endregion

    #region Iteration Methods

    /// <summary>
    /// Displays the entities of an iteration.
    /// </summary>
    protected override void DisplayIteration(Iteration previousIteration, Iteration currentIteration)
    {
        // The end
        if (currentIteration == null)
            return;

        // Display entities
        DisplayEntities(currentIteration);

        // Display boundaries
        DisplayBoundaries(previousIteration, currentIteration);
    }

    /// <summary>
    /// Displays the entities of an iteration.
    /// </summary>
    private void DisplayEntities(Iteration iteration)
    {
        DBScanIteration dbscanIteration = iteration as DBScanIteration;

        // Go through each cluster
        foreach (DBScanCluster cluster in iteration.Clusters)
        {
            // Display its items
            List<Vector2> seedItems = cluster.Items.Except(cluster.RecentlyAddedItems).Select(x => new Vector2(x.PositionX, x.PositionY)).ToList();
            GridManager.DisplayEntities(seedItems, ClusterColors[cluster.Id]);

            // Display cluster
            GridManager.DisplayEntities(cluster.RecentlyAddedItems.Select(x => new Vector2(x.PositionX, x.PositionY)).ToList(), ClusterColors[cluster.Id], false, 45f);
        }

        // Display noises
        GridManager.DisplayEntities(dbscanIteration.Noise.Select(x => new Vector2(x.PositionX, x.PositionY)).ToList(), Color.black);

        // Display pending
        GridManager.DisplayEntities(dbscanIteration.Pending.Select(x => new Vector2(x.PositionX, x.PositionY)).ToList(), Color.white);
    }

    /// <summary>
    /// Displays the boundaries of clusters in previous iteration.
    /// </summary>
    private void DisplayBoundaries(Iteration previousIteration, Iteration currentIteration)
    {
        // Classify clusters by their ids
        Dictionary<Guid, DBScanCluster> previousClusters = previousIteration.Clusters.ToDictionary(x => x.Id, x => (DBScanCluster)x);
        Dictionary<Guid, DBScanCluster> currentClusters = currentIteration?.Clusters.ToDictionary(x => x.Id, x => (DBScanCluster)x);

        // Go through each cluster
        foreach (DBScanCluster currentCluster in currentClusters.Values)
        {
            // Find previous cluster
            DBScanCluster previousCluster = null;
            if (previousClusters.ContainsKey(currentCluster.Id))
                previousCluster = previousClusters[currentCluster.Id];

            // Add items that we are scanning around them
            List<Item> items = previousCluster?.RecentlyAddedItems ?? currentCluster.Items.Except(currentCluster.RecentlyAddedItems).ToList();

            // In db scan, the boundaries are circles around the recently added neighbors
            foreach (var item in items)
                GridManager.DisplayCircularBoundary(new Vector2(item.PositionX, item.PositionY), 2, ClusterColors[currentCluster.Id]);

            // Display line to display which ones we got added in current iteration and which item added them
            foreach (var newItem in currentCluster.RecentlyAddedItems)
            {
                // Find closest point from items scanning
                Item scanningItem = FindClosestItem(newItem, items);

                // Draw a line between them
                GridManager.DisplayPaths(new List<Vector2>() { new Vector2(scanningItem.PositionX, scanningItem.PositionY), new Vector2(newItem.PositionX, newItem.PositionY) });
            }
        }
    }

    /// <summary>
	/// Finds the closes item from a list to a provided item.
    /// </summary>
	private Item FindClosestItem(Item item, List<Item> items)
    {
        Item closestItem = items.First();
        float closestDistance = ComputeDistance(item, closestItem);

        foreach (var closeItem in items)
        {
            float distance = ComputeDistance(item, closeItem);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestItem = closeItem;
            }
        }

        return closestItem;
    }

    /// <summary>
    /// Computes distance between two items.
    /// </summary>
    private float ComputeDistance(Item item1, Item item2)
    {
        return (float)
                Math.Sqrt
                (
                    Math.Pow(item1.PositionX - item2.PositionX, 2)
                    +
                    Math.Pow(item1.PositionY - item2.PositionY, 2)
                );
    }

    #endregion

}
