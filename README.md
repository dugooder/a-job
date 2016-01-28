#ajob Solution Template by Dugooder

##Usage
Use this application as a template for creating a job type application.  Use a scheduler to execute the application.

##Design 
Commands (plug-ins) are tied together with a job XML configuration file. 

##Components 
The application uses the following components. 

* [.NET 4.5.2](https://www.microsoft.com/net) - the frameworks used.
* [Visual Studio 2015 Community Edition](https://www.visualstudio.com) - a integrated development enviroment.
* [Visual Studio Code](https://code.visualstudio.com/) - a programmer's editor. 
* [Ninject](http://www.ninject.org/) - to load the plugins.
* [Log4Net](https://logging.apache.org/log4net/) - for logging services.
* [Xunit](http://xunit.github.io/) - for unit testing.
* [Nuget](http://nuget.org/) - for package management.
* [Node](https://nodejs.org/) / [NPM](https://www.npmjs.com/) / [Gulp](http://gulpjs.com/) - for building the application.

##Solution

###Projects
* `supervisor` - is the 'runner.  Given the name job configuration XML file (no extension) it executes all the commands in configuration file.

* `common` - common classes used by the `supervisor` and the command type projects

* `commands` - an example of a command.  All commands should have their own project and test project.  A command project must contain class implementing ICommand and a Ninject module loading that class.

* `common.tests` -  unit tests for the common project

###Files
* Job1_job.xml in the `supervisor` project is an example of a job's configuration file.  Job configuration file must end with  '_job.xml'.

* Commands are loaded from ajob.*.dll files only with the current code.

##Build Setup
1. Download and install Nodejs.
2. Download the source from GitHub.
3. In the source directory use the command ```npm install``` to install the build's components.
4. Build the application use the command ```gulp build```.  The build must be run from the Developer Command Prompt for 2015.

I suggest exploring the gulpfile.js for other gulp tasks like test, compile, and package.