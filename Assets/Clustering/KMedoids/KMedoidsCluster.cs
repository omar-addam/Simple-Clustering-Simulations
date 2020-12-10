using Clustering.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Clustering.KMedoids
{
    [Serializable]
    public class KMedoidsCluster : Core.Cluster
    {

        #region Constructors

        /// <summary>
        /// Seed constructor.
        /// </summary>
        public KMedoidsCluster(Core.Item seed)
            : this(seed.Id, seed)
        {
        }

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="id">Unique identifier used to track clusters across multuple iterations.</param>
        public KMedoidsCluster(Guid id, Core.Item seed)
            : base(id, new List<Core.Item>())
        {
            CenterId = seed.Id;
        }

        #endregion

        #region Fields/Properties

        /// <summary>
        /// The id of the item representing the center of this cluster.
        /// </summary>
        [SerializeField]
        private Guid CenterId;

        /// <summary>
        /// The id of the item representing the center of this cluster.
        /// </summary>
        public Guid ItemId { get { return Id; } }

        /// <summary>
        /// The item representing the center of the cluster.
        /// </summary>
        public Item Centroid
        {
            get
            {
                return ClusterItems.FirstOrDefault(x => x.Id == CenterId);
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Recomputes the center of the cluster based on its items.
        /// </summary>
        public void RecomputeCenter()
        {
            float x = ClusterItems.Sum(item => item.PositionX) / ClusterItems.Count;
            float y = ClusterItems.Sum(item => item.PositionY) / ClusterItems.Count;

            // Find the item closest to the center
            Item centroid = Centroid;
            float minDistance = (float)
                Math.Sqrt(Math.Pow(centroid.PositionX - x, 2) + Math.Pow(centroid.PositionY - y, 2));

            foreach (var item in ClusterItems)
            {
                float distance = (float)
                    Math.Sqrt(Math.Pow(item.PositionX - x, 2) + Math.Pow(item.PositionY - y, 2));
                if (distance < minDistance)
                {
                    minDistance = distance;
                    centroid = item;
                }
            }

            // Set the new centroid
            CenterId = centroid.Id;
        }

        #endregion

    }
}
