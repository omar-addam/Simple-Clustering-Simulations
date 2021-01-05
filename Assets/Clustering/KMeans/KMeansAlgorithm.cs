using Clustering.Core;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Clustering.KMeans
{
    [Serializable]
    public class KMeansAlgorithm : AbstractAlgorithm
    {

        #region Constructors

        /// <summary>
        /// Empty constructor.
        /// </summary>
        public KMeansAlgorithm()
            : this(new List<Item>(), new List<Item>())
        { 
        }

        /// <summary>
        /// Default constructor.
        /// </summary>
        public KMeansAlgorithm(List<Item> items, List<Item> clusters) 
            : base("K-Means", items)
        {
            _ClusterSeeds = clusters;
        }

        #endregion

        #region Fields/Properties

        /// <summary>
        /// List of positions used as cluster seeds.
        /// </summary>
        [SerializeField]
        [Tooltip("List of positions used as cluster seeds.")]
        private List<Item> _ClusterSeeds = new List<Item>();

        /// <summary>
        /// List of positions used as cluster seeds.
        /// </summary>
        public List<Item> ClusterSeeds { get { return _ClusterSeeds; } }

        #endregion

        #region Methods

        /// <summary>
        /// Initializes the first set of clusters used.
        /// </summary>
        protected override List<Cluster> InitializeClusters()
        {
            List<Cluster> clusters = new List<Cluster>();
            foreach (var seed in _ClusterSeeds)
                clusters.Add(new KMeansCluster(seed));
            return clusters;
        }



        /// <summary>
        /// Tries to compute the next iteration. 
        /// If there are no changes to the clusters, the method will return null, identifying the end.
        /// </summary>
        protected override Iteration ComputeNextIteration(Iteration previousIteration)
        {
            // Create a new iteration instance
            Iteration iteration = new Iteration(previousIteration.Order + 1, new List<Core.Cluster>());

            // Create empty clusters
            foreach (KMeansCluster cluster in previousIteration.Clusters)
                iteration.Clusters.Add(new KMeansCluster(cluster.Id, cluster.CenterX, cluster.CenterY));

            // Find for each item the cluster it belongs to
            foreach (Item item in _Items)
            {
                Cluster cluster = FindClosestCluster(item, iteration.Clusters);
                cluster.Items.Add(item);
            }

            // Recompute the center for each cluster
            foreach (KMeansCluster cluster in iteration.Clusters)
                cluster.RecomputeCenter();

            // Check if the new iteration is different than previous iteration
            if (CompareIterations(previousIteration, iteration))
                return null;

            // Return generated iteration
            return iteration;
        }



        /// <summary>
        /// Finds the best fitting cluster based on the distnace between them.
        /// </summary>
        /// <returns></returns>
        private Cluster FindClosestCluster(Item item, List<Cluster> clusters)
        {
            if (clusters.Count == 0)
                return null;

            // Set the first cluster as the closest cluster by default
            Cluster closestCluster = clusters[0];
            float closestDistance = ComputeDistance(item, (KMeansCluster)closestCluster);

            // Go through each cluster and find if it is closer
            foreach (var cluster in clusters)
            {
                float distance = ComputeDistance(item, (KMeansCluster)cluster);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestCluster = cluster;
                }
            }

            // Return the closest cluster
            return closestCluster;
        }

        /// <summary>
        /// Computes the distance between an item and a cluster.
        /// </summary>
        private float ComputeDistance(Item item, KMeansCluster cluster)
        {
            return (float)
                Math.Sqrt
                (
                    Math.Pow(cluster.CenterX - item.PositionX, 2)
                    +
                    Math.Pow(cluster.CenterY - item.PositionY, 2)
                );
        }



        /// <summary>
        /// Compares if two iterations are similar.
        /// </summary>
        private bool CompareIterations(Iteration previousIteration, Iteration newIteration)
        {
            // Classify clusters by their ids for faster retrieval
            Dictionary<Guid, KMeansCluster> previousClusters = new Dictionary<Guid, KMeansCluster>();
            Dictionary<Guid, KMeansCluster> newClusters = new Dictionary<Guid, KMeansCluster>();

            foreach (KMeansCluster cluster in previousIteration.Clusters)
                previousClusters.Add(cluster.Id, cluster);
            foreach (KMeansCluster cluster in newIteration.Clusters)
                newClusters.Add(cluster.Id, cluster);

            // Go through each cluster and see if you can find any difference
            foreach (Guid clusterId in previousClusters.Keys)
            {
                KMeansCluster previousCluster = previousClusters[clusterId];
                KMeansCluster newCluster = newClusters[clusterId];

                if (!CompareClusters(previousCluster, newCluster))
                    return false;
            }

            // They are the same
            return true;
        }

        /// <summary>
        /// Compares if two clusters are similar.
        /// </summary>
        private bool CompareClusters(KMeansCluster previousCluster, KMeansCluster newCluster)
        {
            // Check if clusters have different centers
            if (previousCluster.CenterX != newCluster.CenterX
                || previousCluster.CenterY != newCluster.CenterY)
                return false;

            // Check if clusters have different number of items
            if (previousCluster.Items.Count != newCluster.Items.Count)
                return false;

            // Check if the clusters contain different items
            List<Guid> previousItemIds = new List<Guid>();
            foreach (Core.Item item in previousCluster.Items)
                previousItemIds.Add(item.Id);
            foreach (Core.Item item in newCluster.Items)
                if (!previousItemIds.Contains(item.Id))
                    return false;

            // They are the same
            return true;
        }

        #endregion

    }
}
