//-----------------------------------------------------------------------
// <copyright file="Job.cs" company="CodeRanger.com">
//     Copyright (c) CodeRanger.com. All rights reserved.
// </copyright>
// <author>Dan Petitt</author>
// <comment />
//-----------------------------------------------------------------------

namespace WebTest.Jobs
{
    public abstract class Job
    {
        /// <summary>
        /// Executes job, only return false if you wan't to be put back on the queue because you temporarily 
        /// dont have something to complete it, returning false will mean your job will always be out back on the queue. 
        /// Otherwise throw and the system will automatically retry a fixed number of times.
        /// </summary>
        /// <returns></returns>
        public abstract bool Execute();

        public abstract override bool Equals( object obj );

        public abstract override int GetHashCode();

        public abstract override string ToString();
    }
}