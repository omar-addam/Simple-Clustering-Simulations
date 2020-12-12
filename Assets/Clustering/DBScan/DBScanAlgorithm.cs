using Clustering.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using UnityEditor.UIElements;
using UnityEngine;

namespace Clustering.DBScan
{
    [Serializable]
    public class DBScanAlgorithm : AbstractAlgorithm
    {

        #region Constructors

        /// <summary>
        /// Empty constructor.
        /// </summary>
        public DBScanAlgorithm()
            : this(new List<Item>(), 1, 1)
        {
            _Noise = new List<Item>();
            Pending = new List<Item>();
        }

        /// <summary>
        /// Minimal constructor.
        /// </summary>
        public DBScanAlgorithm(List<Item> items, float distanceThreshold, int minPoints)
            : base("DB-Scan", items)
        {
            _DistanceThreshold = distanceThreshold;
            _MinPoints = minPoints;
            _Noise = new List<Item>();
            
            Pending = new List<Item>();
            Pending.AddRange(items);
        }

        #endregion

        #region Fields/Properties

        /// <summary>
        /// The distance threshold required to for a point to be a neighbor to a cluster.
        /// </summary>
        [SerializeField]
        [Tooltip("The distance threshold required to for a point to be a neighbor to a cluster.")]
        private float _DistanceThreshold;

        /// <summary>
        /// The distance threshold required to for a point to be a neighbor to a cluster.
        /// </summary>
        public float DistanceThreshold { get { return _DistanceThreshold; } }



        /// <summary>
        /// The minimum number of points requried for a cluster to be formed.
        /// </summary>
        [SerializeField]
        [Tooltip("The minimum number of points requried for a cluster to be formed.")]
        private float _MinPoints;

        /// <summary>
        /// The minimum number of points requried for a cluster to be formed.
        /// </summary>
        public float MinPoints { get { return _MinPoints; } }



        /// <summary>
        /// List of items that do not belong to any cluster.
        /// </summary>
        [SerializeField]
        [Tooltip("List of items that do not belong to any cluster.")]
        private List<Item> _Noise;

        /// <summary>
        /// List of items that do not belong to any cluster.
        /// </summary>
        public List<Item> Noise { get { return _Noise; } }



        /// <summary>
        /// List of items pending to be clustered.
        /// </summary>
        private List<Item> Pending;



        /// <summary>
        /// The id of latest created cluster that we should scan for its neighbors.
        /// </summary>
        private Guid? CurrentClusterId;

        #endregion

        #region Methods

        /// <summary>
        /// Initializes the first set of clusters used.
        /// In DB-Scan, there are no predefined set of clusters.
        /// </summary>
        protected override List<Cluster> InitializeClusters()
        {
            return new List<Cluster>();
        }



        /// <summary>
        /// Tries to compute the next iteration. 
        /// If there are no changes to the clusters, the method will return null, identifying the end.
        /// </summary>
        protected override Iteration ComputeNextIteration(Iteration previousIteration)
        {
            // Check if all items have been processed
            if (Pending.Count == 0)
                return null;

            // Create a new iteration instance
            Iteration iteration = new Iteration(previousIteration.Order + 1, new List<Cluster>());
            foreach (DBScanCluster cluster in previousIteration.Clusters) // In db scan, clusters remain across iterations
                iteration.Clusters.Add(new DBScanCluster(cluster));

            // Check if we are currently still checking neighbors for a cluster
            if (CurrentClusterId != null)
            {
                List<Item> neighborItems = new List<Item>();

                // Find cluster
                DBScanCluster previousCluster = previousIteration.Clusters.FirstOrDefault(x => x.Id == CurrentClusterId.Value) as DBScanCluster;
                DBScanCluster currentCluster = iteration.Clusters.FirstOrDefault(x => x.Id == CurrentClusterId.Value) as DBScanCluster;

                // Go through each recent item and find it if it has neighbors that statisfy the requirements
                foreach (var item in previousCluster.RecentlyAddedItems)
                {
                    // Find items around it
                    List<Item> surroundingItems = FindSurroundingItems(item);

                    // Check if it statisfies the requirements to form a cluster
                    if (surroundingItems.Count + 1 >= _MinPoints) // +1 bcs the core counts too
                        neighborItems.AddRange(surroundingItems);
                }

                // Filter items to only those pending
                neighborItems = neighborItems.Where(x => Pending.Contains(x)).Distinct().ToList();

                // Check if any item is new
                if (neighborItems.Count > 0)
                {
                    // Remove items from pending
                    foreach (var item in neighborItems)
                        Pending.Remove(item);

                    // Add every new item to the rececntly added of current cluster
                    currentCluster.RecentlyAddedItems.AddRange(neighborItems);
                }

                // No more neighbor items to add to this cluster
                else
                {
                    // End this cluster and start looking for a new one
                    CurrentClusterId = null;
                }
            }

            // Start looking for a new cluster
            else
            {
                // Select a random item from the pending list
                var item = Pending.OrderBy(x => Guid.NewGuid()).FirstOrDefault();

                // Remove item from pending list
                Pending.Remove(item);

                // Find items around it
                List<Item> surroundingItems = FindSurroundingItems(item);

                // Check if it statisfies the requirements to form a cluster
                if (surroundingItems.Count + 1 >= _MinPoints) // +1 bcs the core counts too
                {
                    // Create a new cluster
                    DBScanCluster newCluster = new DBScanCluster();

                    // Add all items
                    newCluster.Items.Add(item);
                    newCluster.Items.AddRange(surroundingItems);

                    // Add all neighbors to check next iteration
                    newCluster.RecentlyAddedItems.AddRange(surroundingItems);

                    // Add cluster to iteration
                    iteration.Clusters.Add(newCluster);

                    // Reference the cluster to be used next iteration
                    CurrentClusterId = newCluster.Id;
                }

                // The item is a noise
                else
                {
                    // Add the item to the noise list
                    _Noise.Add(item);
                }
            }

            return iteration;
        }



        /// <summary>
        /// Finds all items around the provide item which fit the requirements.
        /// </summary>
        private List<Item> FindSurroundingItems(Item core)
        {
            List<Item> list = new List<Item>();
            foreach (var item in Items)
                if (item.Id == core.Id)
                    continue;
                else if (ComputeDistance(item, core) <= _DistanceThreshold)
                    list.Add(item);
            return list;
        }

        /// <summary>
        /// Computes the distance between two items.
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
}
