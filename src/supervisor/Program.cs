using common;
using System;

/// <summary>
/// This is a template for a console / schedule job.   
///   
/// Why ?   
/// *  We do C# well and have an established environment.  No need to clutter 
///    the environment. 
/// *  We can have one solution for all our jobs.  Mix and matching 
///    command means more reuse.
/// *  We have components (third party and home grown) usable from .NET applications;
///    maximizing reused to save time and money.
/// 
/// The application uses a plugin type architecture. The command classes are loaded 
/// using ninject IoC container.
/// 
/// Customize this template .... 
/// a. The namesspaces, project names, assembly name should be changed to represent the 
///    specifics of the environment. i.e.  SomeInsuranceCompany.PersonalInsurance.Homeowners.  
/// b. Create command projects like the hellowWorldcomand project,  a command class, 
///    a ninject module class
/// c. Create a unit test project per command project.  
/// d. A job xml file is required for each run.  It should be of the format {jobname}_job.xml.
///    Look at the Job1_job.xml.
/// </summary>

namespace supervisor
{
    static class Program
    {
        static int Main(string[] args)
        {
            // I would advise to NOT add more agurments; add to the job's configuration file.
            if (args.Length == 0)
            {
                throw new ArgumentNullException("Job name");
            }

            string jobName = args[0];

            return Job.Run(jobName);
        }
    }
}
