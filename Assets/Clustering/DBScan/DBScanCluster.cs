using Clustering.Core;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Clustering.DBScan
{
    [Serializable]
    public class DBScanCluster : Cluster
    {

        #region Constructors

        /// <summary>
        /// Empty constructor.
        /// </summary>
        public DBScanCluster()
            : base(new List<Item>())
        {
            _RecentlyAddedItems = new List<Item>();
        }

        /// <summary>
        /// Clone constructor.
        /// </summary>
        public DBScanCluster(DBScanCluster cluster)
            : base(cluster)
        {
            _RecentlyAddedItems = new List<Item>();
        }

        #endregion

        #region Fields/Properties

        /// <summary>
        /// The list of items added in the last iteration.
        /// </summary>
        [SerializeField]
        [Tooltip("The list of items added in the last iteration.")]
        private List<Item> _RecentlyAddedItems;

        /// <summary>
        /// The list of items added in the last iteration.
        /// </summary>
        public List<Item> RecentlyAddedItems { get { return _RecentlyAddedItems; } }

        #endregion

    }
}
