//-----------------------------------------------------------------------
// <copyright file="ListShuffleExtension.cs" company="CodeRanger.com">
//     Copyright (c) CodeRanger.com. All rights reserved.
// </copyright>
// <author>Dan Petitt</author>
// <comment />
//-----------------------------------------------------------------------

namespace WebTest.Queues
{
    using System;
    using System.Collections.Generic;    
    
    static class ListShuffleExtension
    {
        public static void Shuffle<T>( this IList<T> list )
        {
            int n = list.Count;
            while( n > 1 )
            {
                n--;
                int k = rng.Next( n + 1 );
                T value = list[ k ];
                list[ k ] = list[ n ];
                list[ n ] = value;
            }
        }


        private static Random rng = new Random();
    }
}
