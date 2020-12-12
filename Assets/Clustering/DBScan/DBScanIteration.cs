using Clustering.Core;
using System.Collections.Generic;

namespace Clustering.DBScan
{
    public class DBScanIteration : Iteration
    {

        #region Constructors

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
