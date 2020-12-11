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
        public KMedoidsCluster(Guid seedId)
            : this(Guid.NewGuid(), seedId)
        {
        }

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="id">Unique identifier used to track clusters across multuple iterations.</param>
        public KMedoidsCluster(Guid id, Guid seedId)
            : base(id, new List<Item>())
        {
            CenterId = seedId;
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
        public Guid ItemId { get { return CenterId; } }

        /// <summary>
        /// The item representing the center of the cluster.
        /// </summary>
        public Item Centroid
        {
            get
            {
                return Items.FirstOrDefault(x => x.Id == CenterId);
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Recomputes the center of the cluster based on its items.
        /// </summary>
        public void RecomputeCenter()
        {
            float x = Items.Sum(item => item.PositionX) / Items.Count;
            float y = Items.Sum(item => item.PositionY) / Items.Count;

            // Find the item closest to the center
            Item centroid = Centroid;
            float minDistance = (float)
                Math.Sqrt(Math.Pow(centroid.PositionX - x, 2) + Math.Pow(centroid.PositionY - y, 2));

            foreach (var item in Items)
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
