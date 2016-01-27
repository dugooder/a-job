#ajob Solution Template by Dugooder

##Usage
Use this application as a template for creating a job type application.  Use a scheduler to execute the application.

##Features 
Commands (plug-ins) are tied together with a  XML configuration file passed as a parameter.

The application uses the below components. 

* [.NET v4.5.2 and Visual Studio 2015 Community](https://www.visualstudio.com)
* [Ninject](http://www.ninject.org/) - to load the plugins.
* [Log4Net](https://logging.apache.org/log4net/) - for logging services.
* [Xunit](http://xunit.github.io/) - for unit testing
* [Nuget](http://nuget.org/) - for package management
* [Node](https://nodejs.org/) / [NPM](https://www.npmjs.com/) / [Gulp](http://gulpjs.com/) - for building the application

##Solution

###Projects
* `supervisor` - is the command line executable.  Given the name job configuration it executes all the commands in confgiuration file.

* `common` - contains the core classes used by the `supervisor` and the command projects.

* `commands` - is an example of a command project.  A command project must contain at least one class implementing ICommand and a Ninject module loading that class.

* `common.tests` -  are unit tests for the common project.

###Unit Testing
Xunit is used for the unit testing. Each command project should have a test project.  The Visual Studio plugin is a Nuget package included in the `commons.tests` project.

###Files
* Job1_job.xml in the `supervisor` project is an example of a job's configuration file.  Job configuration file must end with  '_job.xml'. In the case of the example job the parmeter 'Job1' should be used with the `supervisor' application.

* Commands are loaded from ```ajob.*.dll``` files (easily changed).

###Build
The project can be build and tested from the command line using Gulp.  The ```gulp package``` puts the deployable bits in the ``\build`` folder.  There are some file masks like ```*.tests^``` assumed in the gulp file; please review it.

MSBuild is used during ```gulp complie```.  I suggest exploring the gulpfile.js for other gulp tasks like ```gulp test``` and ```gulp build```.  

##Build Instructions
1. Download and install Nodejs.
2. Download the source from GitHub.
3. In the source directory use the command ```npm install``` to install the build's components.
4. To build the application use the command ```gulp build```.  Use a run Visual Studio Command Prompt (2015 community edition used) to build the application.

###Make it yours
You will probalby want to change Namespaces, project names, DLL/EXE names and a file masks for your domain.  I intended this solution for a domain area / group. One place for all your jobs and to reuse commands. You will also want to add a ```gulp deploy``` task to the `gulpfile.js` to make deployement truely seemless.  

I would like your feedback or just drop me a mail letting me know that you found it useful  Hope this helps.
