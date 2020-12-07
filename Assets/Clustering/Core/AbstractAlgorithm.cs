﻿using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

namespace Clustering.Core
{
    public abstract class AbstractAlgorithm
    {

        #region Constructors

        /// <summary>
        /// Empty constructor.
        /// </summary>
        /// <param name="items">List of all items to be clustered.</param>
        public AbstractAlgorithm(List<Iteration> items)
            : this("N/A", items)
        {
        }

        /// <summary>
        /// Minimal constructor.
        /// </summary>
        protected AbstractAlgorithm(string name, List<Iteration> items)
        {
            Name = name;
            Items = items;
            Iterations = new List<Iteration>();
        }

        #endregion

        #region Fields/Properties

        /// <summary>
        /// The name of the algorithm.
        /// </summary>
        [SerializeField]
        private string Name;



        /// <summary>
        /// List of all iterations generated by this algorithm.
        /// </summary>
        [SerializeField]
        private List<Iteration> Iterations;

        /// <summary>
        /// List of all items to be clustered.
        /// </summary>
        [SerializeField]
        protected readonly List<Iteration> Items;

        #endregion

        #region Methods

        /// <summary>
        /// Computes the clusters.
        /// </summary>
        public void Compute()
        {
            // Iteration 0 represents the beginning
            Iteration currentIteration = new Iteration(0, new List<Cluster>());

            do
            {
                // Add the generated iteration to the list of iterations
                Iterations.Add(currentIteration);

                // Compute next iteration
                currentIteration = ComputeNextIteration(currentIteration);
            }
            while (currentIteration != null);
        }

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
