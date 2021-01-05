using Clustering.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Clustering.KMedoids
{
    [Serializable]
    public class KMedoidsAlgorithm : AbstractAlgorithm
    {

        #region Constructors

        /// <summary>
        /// Empty constructor.
        /// </summary>
        public KMedoidsAlgorithm()
            : this(new List<Item>(), new List<Guid>())
        {
        }

        /// <summary>
        /// Default constructor.
        /// </summary>
        public KMedoidsAlgorithm(List<Item> items, List<Guid> clusters)
            : base("K-Medoids", items)
        {
            _ClusterSeeds = clusters;
        }

        #endregion

        #region Fields/Properties

        /// <summary>
        /// Id list of items used as cluster seeds.
        /// </summary>
        [SerializeField]
        [Tooltip("Id list of items used as cluster seeds.")]
        private List<Guid> _ClusterSeeds = new List<Guid>();

        /// <summary>
        /// Id list of items used as cluster seeds.
        /// </summary>
        public List<Guid> ClusterSeeds { get { return _ClusterSeeds; } }

        #endregion

        #region Methods

        /// <summary>
        /// Initializes the first set of clusters used.
        /// </summary>
        protected override List<Cluster> InitializeClusters()
        {
            List<Cluster> clusters = new List<Cluster>();
            foreach (var seed in _ClusterSeeds)
            {
                KMedoidsCluster cluster = new KMedoidsCluster(seed);
                clusters.Add(cluster);
                cluster.Items.Add(_Items.First(x => x.Id == seed));
            }
            return clusters;
        }



        /// <summary>
        /// Tries to compute the next iteration. 
        /// If there are no changes to the clusters, the method will return null, identifying the end.
        /// </summary>
        protected override Iteration ComputeNextIteration(Iteration previousIteration)
        {
            // Create a new iteration instance
            Iteration iteration = new Iteration(previousIteration.Order + 1, new List<Cluster>());

            // Create empty clusters
            foreach (KMedoidsCluster cluster in previousIteration.Clusters)
            {
                KMedoidsCluster emptyCluster = new KMedoidsCluster(cluster.Id, cluster.CenterId);
                emptyCluster.Items.Add(cluster.Centroid);
                iteration.Clusters.Add(emptyCluster);
            }

            // Find for each item the cluster it belongs to
            foreach (Item item in _Items)
            {
                Cluster cluster = FindClosestCluster(item, iteration.Clusters);
                cluster.Items.Add(item);
            }

            // Recompute the center for each cluster
            foreach (KMedoidsCluster cluster in iteration.Clusters)
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
        private Cluster FindClosestCluster(Item item, List<Cluster> clusters)
        {
            if (clusters.Count == 0)
                return null;

            // Set the first cluster as the closest cluster by default
            Cluster closestCluster = clusters[0];
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
        private float ComputeDistance(Item item, KMedoidsCluster cluster)
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
        private bool CompareIterations(Iteration previousIteration, Iteration newIteration)
        {
            // Classify clusters by their ids for faster retrieval
            Dictionary<Guid, KMedoidsCluster> previousClusters = new Dictionary<Guid, KMedoidsCluster>();
            Dictionary<Guid, KMedoidsCluster> newClusters = new Dictionary<Guid, KMedoidsCluster>();

            foreach (KMedoidsCluster cluster in previousIteration.Clusters)
                previousClusters.Add(cluster.Id, cluster);
            foreach (KMedoidsCluster cluster in newIteration.Clusters)
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
            if (previousCluster.CenterId != newCluster.CenterId)
                return false;

            // Check if clusters have different number of items
            if (previousCluster.Items.Count != newCluster.Items.Count)
                return false;

            // Check if the clusters contain different items
            List<Guid> previousItemIds = new List<Guid>();
            foreach (Item item in previousCluster.Items)
                previousItemIds.Add(item.Id);
            foreach (Item item in newCluster.Items)
                if (!previousItemIds.Contains(item.Id))
                    return false;

            // They are the same
            return true;
        }

        #endregion

    }
}
