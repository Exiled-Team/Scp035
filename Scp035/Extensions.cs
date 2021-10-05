// -----------------------------------------------------------------------
// <copyright file="Extensions.cs" company="Build and Cyanox">
// Copyright (c) Build and Cyanox. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace Scp035
{
    using System.Collections.Generic;

    /// <summary>
    /// Various extension methods to make my life slightly better.
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Gets a single random object from an <see cref="IList{T}"/>.
        /// </summary>
        /// <param name="list">The list to get the item from.</param>
        /// <typeparam name="T">The type of the objects in the list.</typeparam>
        /// <returns>A random item from the list.</returns>
        public static T GetRandom<T>(this IList<T> list)
        {
            return list[Exiled.Loader.Loader.Random.Next(list.Count)];
        }
    }
}