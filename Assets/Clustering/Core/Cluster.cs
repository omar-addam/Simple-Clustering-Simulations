using System;
using System.Collections.Generic;
using UnityEngine;

namespace Clustering.Core
{
    [Serializable]
    public class Cluster
    {

        #region Constructors

        /// <summary>
        /// Empty constructor.
        /// </summary>
        public Cluster()
            : this(new List<Item>())
        {
        }

        /// <summary>
        /// Minimal constructor.
        /// </summary>
        /// <param name="items">List of all items that are included in this cluster.</param>
        public Cluster(List<Item> items)
            : this(Guid.NewGuid(), items)
        {
        }

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="id">Unique identifier used to track clusters across multuple iterations.</param>
        /// <param name="items">List of all items that are included in this cluster.</param>
        public Cluster(Guid id, List<Item> items)
        {
            Id = id;
            _Items = items;
        }

        /// <summary>
        /// Clone constructor.
        /// </summary>
        /// <param name="cluster">Instance to clone.</param>
        public Cluster(Cluster cluster)
            : this(cluster.Id, new List<Item>())
        {
            foreach (var item in cluster._Items)
                _Items.Add(new Item(item));
        }

        #endregion

        #region Fields/Properties

        /// <summary>
        /// Unique identifier used to track clusters across multuple iterations.
        /// </summary>
        public Guid Id { private set; get; }



        /// <summary>
        /// List of all items that are included in this cluster.
        /// </summary>
        [SerializeField]
        [Tooltip("List of all items that are included in this cluster.")]
        private List<Item> _Items;

        /// <summary>
        /// List of all items that are included in this cluster.
        /// </summary>
        public List<Item> Items { get { return _Items; } }

        #endregion

        #region Methods

        /// <summary>
        /// Uses the id of the cluster for hash coding.
        /// </summary>
        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        /// <summary>
        /// Uses the id of the cluster for comparison.
        /// </summary>
        public override bool Equals(object obj)
        {
            Cluster item = obj as Cluster;
            return item?.Id == Id;
        }

        /// <summary>
        /// Displays the number of items in this cluster.
        /// </summary>
        public override string ToString()
        {
            return _Items?.Count.ToString("N0");
        }

        #endregion

    }
}
