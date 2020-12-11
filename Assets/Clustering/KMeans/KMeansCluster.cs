using Clustering.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Clustering.KMeans
{
    [Serializable]
    public class KMeansCluster : Cluster
    {

        #region Constructors

        /// <summary>
        /// Seed constructor.
        /// </summary>
        public KMeansCluster(Core.Item seed)
            : this(seed.Id, seed.PositionX, seed.PositionY)
        {
        }

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="id">Unique identifier used to track clusters across multuple iterations.</param>
        /// <param name="x">The x position of the center of the cluster.</param>
        /// <param name="y">The y position of the center of the cluster.</param>
        public KMeansCluster(Guid id, float x, float y)
            : base(id, new List<Core.Item>())
        {
            _CenterX = x;
            _CenterY = y;
        }

        #endregion

        #region Fields/Properties

        /// <summary>
        /// The x position of the center of the cluster.
        /// </summary>
        [SerializeField]
        [Tooltip("The x position of the center of the cluster.")]
        private float _CenterX = 0;

        /// <summary>
        /// The x position of the center of the cluster.
        /// </summary>
        public float CenterX { get { return _CenterX; } }



        /// <summary>
        /// The y position of the center of the cluster.
        /// </summary>
        [SerializeField]
        [Tooltip("The y position of the center of the cluster.")]
        private float _CenterY = 0;

        /// <summary>
        /// The y position of the center of the cluster.
        /// </summary>
        public float CenterY { get { return _CenterY; } }

        #endregion

        #region Methods

        /// <summary>
        /// Recomputes the center of the cluster based on its items.
        /// </summary>
        public void RecomputeCenter()
        {
            float x = Items.Sum(item => item.PositionX) / Items.Count;
            float y = Items.Sum(item => item.PositionY) / Items.Count;

            _CenterX = x;
            _CenterY = y;
        }

        #endregion

    }
}
