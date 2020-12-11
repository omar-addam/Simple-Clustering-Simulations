﻿using System.Collections.Generic;
using UnityEngine;

namespace Clustering.Core
{
    public abstract class AbstractAlgorithm
    {

        #region Constructors

        /// <summary>
        /// Minimal constructor.
        /// </summary>
        /// <param name="items">List of all items to be clustered.</param>
        public AbstractAlgorithm(List<Item> items)
            : this("N/A", items)
        {
        }

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="name">The name of the algorithgm.</param>
        /// <param name="items">List of all items to be clustered.</param>
        protected AbstractAlgorithm(string name, List<Item> items)
        {
            Name = name;
            _Items = items;
            _Iterations = new List<Iteration>();
        }

        #endregion

        #region Fields/Properties

        /// <summary>
        /// The name of the algorithm.
        /// </summary>
        [SerializeField]
        [Tooltip("The name of the algorithm.")]
        private string Name;



        /// <summary>
        /// List of all iterations generated by this algorithm.
        /// </summary>
        [SerializeField]
        [Tooltip("List of all iterations generated by this algorithm.")]
        private List<Iteration> _Iterations;

        /// <summary>
        /// List of all iterations generated by this algorithm.
        /// </summary>
        public List<Iteration> Iterations { get { return _Iterations; } }



        /// <summary>
        /// List of all items to be clustered.
        /// </summary>
        [SerializeField]
        [Tooltip("List of all items to be clustered.")]
        protected List<Item> _Items;

        /// <summary>
        /// List of all items to be clustered.
        /// </summary>
        public List<Item> Items { get { return _Items; } }

        #endregion

        #region Methods

        /// <summary>
        /// Computes the clusters.
        /// </summary>
        public void Compute()
        {
            // Iteration 0 represents the beginning
            Iteration currentIteration = new Iteration(0, InitializeClusters());

            do
            {
                // Add the generated iteration to the list of iterations
                _Iterations.Add(currentIteration);

                // Compute next iteration
                currentIteration = ComputeNextIteration(currentIteration);
            }
            while (currentIteration != null);
        }

        /// <summary>
        /// Initializes the first set of clusters used.
        /// </summary>
        protected abstract List<Cluster> InitializeClusters();

        /// <summary>
        /// Tries to compute the next iteration. 
        /// If there are no changes to the clusters, the method will return null, identifying the end.
        /// </summary>
        protected abstract Iteration ComputeNextIteration(Iteration previousIteration);

        /// <summary>
        /// Displays the name of this algorithm.
        /// </summary>
        public override string ToString()
        {
            return Name;
        }

        #endregion

    }
}
