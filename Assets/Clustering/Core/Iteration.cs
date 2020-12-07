using System;
using System.Collections;
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
            Order = order;
            Clusters = clusters;
        }

        /// <summary>
        /// Clone constructor.
        /// </summary>
        /// <param name="iteration">Instance to clone.</param>
        public Iteration(Iteration iteration)
            : this(iteration.Order, new List<Cluster>())
        {
            foreach (var cluster in iteration.Clusters)
                Clusters.Add(new Cluster(cluster));
        }

        #endregion

        #region Fields/Properties

        /// <summary>
        /// The order of the iteration.
        /// </summary>
        [SerializeField]
        private int Order;

        /// <summary>
        /// The order of the iteration.
        /// </summary>
        public int IterationOrder { private set; get; }




        /// <summary>
        /// List of all clusters.
        /// </summary>
        [SerializeField]
        private List<Cluster> Clusters;

        /// <summary>
        /// List of all clusters.
        /// </summary>
        public List<Cluster> IterationClusters { get { return Clusters; } }

        #endregion

        #region Methods

        /// <summary>
        /// Uses the order of the iteration for hash coding.
        /// </summary>
        public override int GetHashCode()
        {
            return Order.GetHashCode();
        }

        /// <summary>
        /// Uses the order of the iteration for comparison.
        /// </summary>
        public override bool Equals(object obj)
        {
            Iteration item = obj as Iteration;
            return item?.Order == Order;
        }

        /// <summary>
        /// Displays the oder of this iteration.
        /// </summary>
        public override string ToString()
        {
            return Order.ToString("N0");
        }

        #endregion

    }
}
