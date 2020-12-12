using Clustering.Core;
using System.Collections.Generic;

namespace Clustering.DBScan
{
    public class DBScanIteration : Iteration
    {

        #region Constructors

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="order">The order of the iteration.</param>
        /// <param name="clusters">List of all clusters.</param>
        public DBScanIteration(int order, List<Cluster> clusters)
            : base(order, clusters)
        {
            Pending = new List<Item>();
            Noise = new List<Item>();
        }

        /// <summary>
        /// Copy constructor.
        /// </summary>
        public DBScanIteration(int order, DBScanIteration iteration)
            : base(iteration)
        {
            _Order = order;

            Pending = new List<Item>();
            Pending.AddRange(iteration.Pending);

            Noise = new List<Item>();
            Noise.AddRange(iteration.Noise);
        }

        #endregion

        #region Fields/Properties

        /// <summary>
        /// List of items pending to be clustered.
        /// </summary>
        public List<Item> Pending { private set; get; }

        /// <summary>
        /// List of items that do not belong to any cluster.
        /// </summary>
        public List<Item> Noise { private set; get; }

        #endregion

    }
}
