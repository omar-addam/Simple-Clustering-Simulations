using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Clustering.KMean
{
    [Serializable]
    public class KMeanCluster : Core.Cluster
    {

        #region Constructors

        /// <summary>
        /// Seed constructor
        /// </summary>
        public KMeanCluster(Core.Item seed)
            : this(seed.Id, seed.PositionX, seed.PositionY)
        {
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="id">Unique identifier used to track clusters across multuple iterations.</param>
        /// <param name="x">The x position of the center of the cluster.</param>
        /// <param name="y">The y position of the center of the cluster.</param>
        public KMeanCluster(Guid id, float x, float y)
            : base(id, new List<Core.Item>())
        {
            X = x;
            Y = y;
        }

        #endregion

        #region Fields/Properties

        /// <summary>
        /// The x position of the center of the cluster.
        /// </summary>
        [SerializeField]
        private float X = 0;

        /// <summary>
        /// The x position of the center of the cluster.
        /// </summary>
        public float CenterX { get { return X; } }



        /// <summary>
        /// The y position of the center of the cluster.
        /// </summary>
        [SerializeField]
        private float Y = 0;

        /// <summary>
        /// The y position of the center of the cluster.
        /// </summary>
        public float CenterY { get { return Y; } }

        #endregion

        #region Methods

        /// <summary>
        /// Recomputes the center of the cluster based on its items.
        /// </summary>
        public void RecomputeCenter()
        {
            float x = ClusterItems.Sum(item => item.PositionX) / ClusterItems.Count;
            float y = ClusterItems.Sum(item => item.PositionY) / ClusterItems.Count;

            X = x;
            Y = y;
        }

        #endregion

    }
}
