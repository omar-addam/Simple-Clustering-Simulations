﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Clustering.KMean
{
    [Serializable]
    public class KMeanAlgorithm : Core.AbstractAlgorithm
    {

        /// <summary>
        /// Empty constructor.
        /// </summary>
        public KMeanAlgorithm()
            : this(new List<Core.Item>())
        { 
        }

        /// <summary>
        /// Minimal constructor.
        /// </summary>
        public KMeanAlgorithm(List<Core.Item> items) 
            : base("K-Mean", items)
        {
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
            foreach (KMeanCluster cluster in previousIteration.IterationClusters)
                iteration.IterationClusters.Add(new KMeanCluster(cluster.Id, cluster.CenterX, cluster.CenterY));

            // Find for each item the cluster it belongs to
            foreach (Core.Item item in Items)
            {
                Core.Cluster cluster = FindClosestCluster(item, iteration.IterationClusters);
                cluster.ClusterItems.Add(item);
            }

            // Recompute the center for each cluster
            foreach (KMeanCluster cluster in iteration.IterationClusters)
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
        private Core.Cluster FindClosestCluster(Core.Item item, List<Core.Cluster> clusters)
        {
            if (clusters.Count == 0)
                return null;

            // Set the first cluster as the closest cluster by default
            Core.Cluster closestCluster = clusters[0];
            float closestDistance = ComputeDistance(item, (KMeanCluster)closestCluster);

            // Go through each cluster and find if it is closer
            foreach (var cluster in clusters)
            {
                float distance = ComputeDistance(item, (KMeanCluster)cluster);
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
        private float ComputeDistance(Core.Item item, KMeanCluster cluster)
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
        private bool CompareIterations(Core.Iteration previousIteration, Core.Iteration newIteration)
        {
            // Classify clusters by their ids for faster retrieval
            Dictionary<Guid, KMeanCluster> previousClusters = new Dictionary<Guid, KMeanCluster>();
            Dictionary<Guid, KMeanCluster> newClusters = new Dictionary<Guid, KMeanCluster>();

            foreach (KMeanCluster cluster in previousIteration.IterationClusters)
                previousClusters.Add(cluster.Id, cluster);
            foreach (KMeanCluster cluster in newIteration.IterationClusters)
                newClusters.Add(cluster.Id, cluster);

            // Go through each cluster and see if you can find any difference
            foreach (Guid clusterId in previousClusters.Keys)
            {
                KMeanCluster previousCluster = previousClusters[clusterId];
                KMeanCluster newCluster = newClusters[clusterId];

                if (!CompareClusters(previousCluster, newCluster))
                    return false;
            }

            // They are the same
            return true;
        }

        /// <summary>
        /// Compares if two clusters are similar.
        /// </summary>
        private bool CompareClusters(KMeanCluster previousCluster, KMeanCluster newCluster)
        {
            // Check if clusters have different centers
            if (previousCluster.CenterX != newCluster.CenterX
                || previousCluster.CenterY != newCluster.CenterY)
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
