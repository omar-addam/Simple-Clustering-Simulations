using System;
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
            _PositionX = x;
            _PositionY = y;
        }

        /// <summary>
        /// Clone constructor.
        /// </summary>
        /// <param name="item">Instance to clone.</param>
        public Item(Item item)
            : this(item.Id, item.PositionX, item.PositionY)
        {
        }

        #endregion

        #region Fields/Properties

        /// <summary>
        /// Unique identifier used to track items across multuple iterations.
        /// </summary>
        public Guid Id { private set; get; }



        /// <summary>
        /// The x position of the item.
        /// </summary>
        [SerializeField]
        [Tooltip("The x position of the item.")]
        private float _PositionX = 0;

        /// <summary>
        /// The x position of the item.
        /// </summary>
        public float PositionX { get { return _PositionX; } }



        /// <summary>
        /// The y position of the item.
        /// </summary>
        [SerializeField]
        [Tooltip("The y position of the item.")]
        private float _PositionY = 0;

        /// <summary>
        /// The y position of the item.
        /// </summary>
        public float PositionY { get { return _PositionY; } }

        #endregion

        #region Methods

        /// <summary>
        /// Uses the id of the item for hash coding.
        /// </summary>
        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        /// <summary>
        /// Uses the id of the item for comparison.
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
            return string.Format("{0},{1}", _PositionX, _PositionY);
        }

        #endregion

    }
}
