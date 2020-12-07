using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Clustering.Core
{
    [Serializable]
    public class Item
    {

        #region Constructors

        /// <summary>
        /// Empty constructor.
        /// </summary>
        public Item()
            : this(0f, 0f)
        { 
        }

        /// <summary>
        /// Minimal constructor.
        /// </summary>
        /// <param name="x">The x position of the item.</param>
        /// <param name="y">The y position of the item.</param>
        public Item(float x, float y)
            : this(Guid.NewGuid(), x, y)
        {
        }

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="id">summary>Unique identifier used to track items across multuple iterations.</param>
        /// <param name="x">The x position of the item.</param>
        /// <param name="y">The y position of the item.</param>
        public Item(Guid id, float x, float y)
        {
            Id = id;
            X = x;
            Y = y;
        }

        /// <summary>
        /// Clone constructor
        /// </summary>
        /// <param name="item"></param>
        public Item(Item item)
            : this(item.Id, item.PositionX, item.PositionY)
        {
        }

        #endregion

        #region Fields/Properties

        /// <summary>
        /// Unique identifier used to track items across multuple iterations.
        /// It is also used to link data with the 3d objects representing them.
        /// </summary>
        public Guid Id { private set; get; }



        /// <summary>
        /// The x position of the item.
        /// </summary>
        [SerializeField]
        private float X = 0;

        /// <summary>
        /// The x position of the item.
        /// </summary>
        public float PositionX { get { return X; } }



        /// <summary>
        /// The y position of the item.
        /// </summary>
        [SerializeField]
        private float Y = 0;

        /// <summary>
        /// The y position of the item.
        /// </summary>
        public float PositionY { get { return Y; } }

        #endregion

        #region Methods

        /// <summary>
        /// Uses the id of the item for hash coding.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        /// <summary>
        /// Use the id of the item for comparison.
        /// </summary>
        public override bool Equals(object obj)
        {
            Item item = obj as Item;
            return item?.Id == Id;
        }

        /// <summary>
        /// Displays the (x,y) coordinates of the item.
        /// </summary>
        public override string ToString()
        {
            return string.Format("{0},{1}", X, Y);
        }

        #endregion

    }
}
