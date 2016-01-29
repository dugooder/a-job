using common;
using System;

namespace supervisor
{
    static class Program
    {
        static int Main(string[] args)
        {
            // I would advise to NOT add more agurments; add to the job's configuration file.
            if (args.Length == 0)
            {
                Console.WriteLine("Missing argument. Job name is required.");
                return  -1;
            }

            string jobName = args[0];

            return Job.Run(jobName);
        }
    }
}