using Clustering.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Clustering.KMedoids
{
    [Serializable]
    public class KMedoidsAlgorithm : Core.AbstractAlgorithm
    {

        /// <summary>
        /// Empty constructor.
        /// </summary>
        public KMedoidsAlgorithm()
            : this(new List<Item>(), new List<Guid>())
        {
        }

        /// <summary>
        /// Minimal constructor.
        /// </summary>
        public KMedoidsAlgorithm(List<Item> items, List<Guid> clusters)
            : base("K-Medoids", items)
        {
            ClusterSeeds = clusters;
        }



        /// <summary>
        /// Id list of items used as cluster seeds.
        /// </summary>
        [SerializeField]
        private List<Guid> ClusterSeeds = new List<Guid>();

        /// <summary>
        /// Id list of items used as cluster seeds.
        /// </summary>
        public List<Guid> Clusters { get { return ClusterSeeds; } }

        /// <summary>
        /// Initializes the first set of clusters used.
        /// </summary>
        protected override List<Cluster> InitializeClusters()
        {
            List<Cluster> clusters = new List<Cluster>();
            foreach (var seed in ClusterSeeds)
            {
                KMedoidsCluster cluster = new KMedoidsCluster(seed);
                clusters.Add(cluster);
                cluster.ClusterItems.Add(_Items.First(x => x.Id == seed));
            }
            return clusters;
        }



        /// <summary>
        /// Tries to compute the next iteration. 
        /// If there are no changes to the clusters, the method will return null, identifying the end.
        /// </summary>
        protected override Core.Iteration ComputeNextIteration(Core.Iteration previousIteration)
        {
            // Create a new iteration instance
            Core.Iteration iteration = new Core.Iteration(previousIteration.IterationOrder + 1, new List<Core.Cluster>());

            // Create empty clusters
            foreach (KMedoidsCluster cluster in previousIteration.IterationClusters)
            {
                KMedoidsCluster emptyCluster = new KMedoidsCluster(cluster.Id, cluster.ItemId);
                emptyCluster.ClusterItems.Add(cluster.Centroid);
                iteration.IterationClusters.Add(emptyCluster);
            }

            // Find for each item the cluster it belongs to
            foreach (Core.Item item in _Items)
            {
                Core.Cluster cluster = FindClosestCluster(item, iteration.IterationClusters);
                cluster.ClusterItems.Add(item);
            }

            // Recompute the center for each cluster
            foreach (KMedoidsCluster cluster in iteration.IterationClusters)
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
        private Core.Cluster FindClosestCluster(Core.Item item, List<Core.Cluster> clusters)
        {
            if (clusters.Count == 0)
                return null;

            // Set the first cluster as the closest cluster by default
            Core.Cluster closestCluster = clusters[0];
            float closestDistance = ComputeDistance(item, (KMedoidsCluster)closestCluster);

            // Go through each cluster and find if it is closer
            foreach (var cluster in clusters)
            {
                float distance = ComputeDistance(item, (KMedoidsCluster)cluster);
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
        /// Computes the distance bhetween an item and a cluster.
        /// </summary>
        private float ComputeDistance(Core.Item item, KMedoidsCluster cluster)
        {
            return (float)
                Math.Sqrt
                (
                    Math.Pow(cluster.Centroid.PositionX - item.PositionX, 2)
                    +
                    Math.Pow(cluster.Centroid.PositionY - item.PositionY, 2)
                );
        }



        /// <summary>
        /// Compares if two iterations are similar.
        /// </summary>
        private bool CompareIterations(Core.Iteration previousIteration, Core.Iteration newIteration)
        {
            // Classify clusters by their ids for faster retrieval
            Dictionary<Guid, KMedoidsCluster> previousClusters = new Dictionary<Guid, KMedoidsCluster>();
            Dictionary<Guid, KMedoidsCluster> newClusters = new Dictionary<Guid, KMedoidsCluster>();

            foreach (KMedoidsCluster cluster in previousIteration.IterationClusters)
                previousClusters.Add(cluster.Id, cluster);
            foreach (KMedoidsCluster cluster in newIteration.IterationClusters)
                newClusters.Add(cluster.Id, cluster);

            // Go through each cluster and see if you can find any difference
            foreach (Guid clusterId in previousClusters.Keys)
            {
                KMedoidsCluster previousCluster = previousClusters[clusterId];
                KMedoidsCluster newCluster = newClusters[clusterId];

                if (!CompareClusters(previousCluster, newCluster))
                    return false;
            }

            // They are the same
            return true;
        }

        /// <summary>
        /// Compares if two clusters are similar.
        /// </summary>
        private bool CompareClusters(KMedoidsCluster previousCluster, KMedoidsCluster newCluster)
        {
            // Check if clusters have different centers
            if (previousCluster.ItemId != newCluster.ItemId)
                return false;

            // Check if clusters have different number of items
            if (previousCluster.ClusterItems.Count != newCluster.ClusterItems.Count)
                return false;

            // Check if the clusters contain different items
            List<Guid> previousItemIds = new List<Guid>();
            foreach (Core.Item item in previousCluster.ClusterItems)
                previousItemIds.Add(item.Id);
            foreach (Core.Item item in newCluster.ClusterItems)
                if (!previousItemIds.Contains(item.Id))
                    return false;

            // They are the same
            return true;
        }

    }
}
