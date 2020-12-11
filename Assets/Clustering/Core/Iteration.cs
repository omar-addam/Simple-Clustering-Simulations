using System;
using System.Collections.Generic;
using UnityEngine;

namespace Clustering.Core
{
    [Serializable]
    public class Iteration
    {

        #region Constructors

        /// <summary>
        /// Empty constructor.
        /// </summary>
        public Iteration()
            : this(0, new List<Cluster>())
        {
        }

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="order">The order of the iteration.</param>
        /// <param name="clusters">List of all clusters.</param>
        public Iteration(int order, List<Cluster> clusters)
        {
            _Order = order;
            _Clusters = clusters;
        }

        /// <summary>
        /// Clone constructor.
        /// </summary>
        /// <param name="iteration">Instance to clone.</param>
        public Iteration(Iteration iteration)
            : this(iteration._Order, new List<Cluster>())
        {
            foreach (var cluster in iteration._Clusters)
                _Clusters.Add(new Cluster(cluster));
        }

        #endregion

        #region Fields/Properties

        /// <summary>
        /// The order of the iteration.
        /// </summary>
        [SerializeField]
        [Tooltip("The order of the iteration.")]
        private int _Order;

        /// <summary>
        /// The order of the iteration.
        /// </summary>
        public int Order { get { return _Order; } }




        /// <summary>
        /// List of all clusters.
        /// </summary>
        [SerializeField]
        [Tooltip("List of all clusters.")]
        private List<Cluster> _Clusters;

        /// <summary>
        /// List of all clusters.
        /// </summary>
        public List<Cluster> Clusters { get { return _Clusters; } }

        #endregion

        #region Methods

        /// <summary>
        /// Uses the order of the iteration for hash coding.
        /// </summary>
        public override int GetHashCode()
        {
            return _Order.GetHashCode();
        }

        /// <summary>
        /// Uses the order of the iteration for comparison.
        /// </summary>
        public override bool Equals(object obj)
        {
            Iteration item = obj as Iteration;
            return item?._Order == _Order;
        }

        /// <summary>
        /// Displays the oder of this iteration.
        /// </summary>
        public override string ToString()
        {
            return _Order.ToString("N0");
        }

        #endregion

    }
}
