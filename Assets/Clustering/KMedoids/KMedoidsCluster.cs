using Clustering.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Clustering.KMedoids
{
    [Serializable]
    public class KMedoidsCluster : Cluster
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
            _CenterId = seedId;
        }

        #endregion

        #region Fields/Properties

        /// <summary>
        /// The id of the item representing the center of this cluster.
        /// </summary>
        [Tooltip("The id of the item representing the center of this cluster.")]
        [SerializeField]
        private Guid _CenterId;

        /// <summary>
        /// The id of the item representing the center of this cluster.
        /// </summary>
        public Guid CenterId { get { return _CenterId; } }

        /// <summary>
        /// The item representing the center of the cluster.
        /// </summary>
        public Item Centroid
        {
            get
            {
                return Items.FirstOrDefault(x => x.Id == _CenterId);
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
            _CenterId = centroid.Id;
        }

        #endregion

    }
}
