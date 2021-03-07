// -----------------------------------------------------------------------
// <copyright file="Vector.cs" company="Build and Cyanox">
// Copyright (c) Build and Cyanox. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace Scp035.SerializableClasses
{
#pragma warning disable SA1101

    using UnityEngine;

    /// <summary>
    /// Used to be serialized and read from a config in place of a <see cref="Vector3"/>.
    /// </summary>
    public class Vector
    {
        /// <summary>
        /// Gets or sets the abscissa axis value.
        /// </summary>
        public float X { get; set; }

        /// <summary>
        /// Gets or sets the ordinate axis value.
        /// </summary>
        public float Y { get; set; }

        /// <summary>
        /// Gets or sets the applicate axis value.
        /// </summary>
        public float Z { get; set; }

        /// <summary>
        /// Returns an instance of the <see cref="Vector"/> class as a <see cref="Vector3"/> for alternative uses.
        /// </summary>
        /// <returns>A <see cref="Vector3"/> with identical <see cref="X"/>, <see cref="Y"/>, and <see cref="Z"/> values.</returns>
        public Vector3 ToVector3()
            => new Vector3(X, Y, Z);
    }
}