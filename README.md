#ajob Solution Template

##Usage
Use this application as a template for creating a job type application.  Use a scheduler to execute the application.

##Features 
It uses a plugin type approach.  Commands (plug-ins) are tied together with a Job.XML file. 

The application uses the below components. 

Ninject(http://www.ninject.org/) - to load the plugins.
Log4Net(https://logging.apache.org/log4net/) - for logging services.
Xunit(http://xunit.github.io/) - for unit testing
Nuget(http://nuget.org/) - for package management
Node / NPM / Gulp - for building the application

##Solution

###Projects
* `supervisor` - given the name job configuration XML file (no extension) it runs commands.

* `common` - common classes used by the runnder and command type projects

* `commands` - a demostration type command.  Each command should have its own proejct and test project.

* `common.tests` -  unit tests for the common project

###Files
* Job1.xml in the `supervisor` project is an example of a job's configuration file.

##Build Setup
1. Download and install Nodejs.
2. Download the source.
3. In the source directory use npm to install the build system components. 
```bat 
npm install
```
4. To build the application. 
```bat
gulp build 
```
Explore the gulpfile.js for other gulp tasks.