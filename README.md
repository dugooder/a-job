#ajob Solution Template by Dugooder

##Usage
Use this application as a template for creating a job type application.  Use a scheduler to execute the application.

##Features 
It uses a plugin type approach.  Commands (plug-ins) are tied together with a job XML configuration file. 

The application uses the below components. 

* [Ninject](http://www.ninject.org/) - to load the plugins.
* [Log4Net](https://logging.apache.org/log4net/) - for logging services.
* [Xunit](http://xunit.github.io/) - for unit testing
* [Nuget](http://nuget.org/) - for package management
* [Node](https://nodejs.org/) / [NPM](https://www.npmjs.com/) / [Gulp](http://gulpjs.com/) - for building the application

##Solution

###Projects
* `supervisor` - given the name job configuration XML file (no extension) it is the 'runner'

* `common` - common classes used by the `supervisor` and the command type projects

* `commands` - an example of a command.  Each command should have its own proejct and test project.

* `common.tests` -  unit tests for the common project

###Files
* Job1_job.xml in the `supervisor` project is an example of a job's configuration file.

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