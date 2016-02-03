var gulp = require("gulp"),
    msbuild = require("gulp-msbuild"),
    del = require("del"),
    args = require("yargs").argv,
    assemblyInfo = require("gulp-dotnet-assembly-info"),
    shell = require("gulp-shell"),
    fs = require("fs"),
    request = require("request"),
    xunit = require("gulp-xunit-runner"),
    flatten = require("gulp-flatten"),
    runseq = require("gulp-run-sequence");

var projectsWithTests = ["common"];

var pkg = require("./package.json");
var buildFolder = "build/";
var configuration = args.configuration || "Debug";
var srcDir = "src/";
var testsSuffix = ".tests";

gulp.task("nuget-download", function (done) {
    if (!fs.existsSync("nuget.exe")) {
        console.log("Downloading nuget.exe");
        return request("https://www.nuget.org/nuget.exe").pipe(fs.createWriteStream("nuget.exe"));
    } else {
        console.log("Nuget.exe already exists.");
        return done();
    }
});

gulp.task("nuget-restore", ["nuget-download"], shell.task([
    "nuget restore src/" + pkg.name + ".sln "]
    ));

gulp.task("info", function () {
    console.log("Building version:" + pkg.version);
    console.log("Configuration:" + configuration);
});

gulp.task("assemblyInfo", function () {
    return gulp
        .src("**/AssemblyInfo.cs")
        .pipe(assemblyInfo({
            title: pkg.title,
            description: pkg.description,
            configuration: configuration,
            company: pkg.company,
            product: pkg.product,
            trademark: pkg.trademark,
            culture: pkg.culture,
            version: pkg.version,
            fileVersion: pkg.version,
            copyright: function () {
                return pkg.company + " " + new Date().getFullYear();
            },
        }))
        .pipe(gulp.dest("."));
});

gulp.task("clean", function () {
    return del([
        "src/**/bin/**/*",
        "src/**/obj/**/*",
        buildFolder + "*"], { force: true });
});

gulp.task("compile", function () {
    if (!fs.existsSync(buildFolder)) {
        fs.mkdirSync(buildFolder);
    }
    return gulp
        .src("**/" + pkg.name + ".sln")
        .pipe(msbuild({
            toolsVersion: 14.0,
            targets: ["Rebuild"],
            errorOnFail: true,
            stdout: true,
            fileLoggerParameters: "LogFile=" + buildFolder + "Build.log;Verbosity=Normal",
            properties: { Configuration: configuration }
        }));
});

gulp.task("test", ["compile"], function () {
    var testsDllDir = [];

    for (var i = 0; i < projectsWithTests.length; i++) {
        var p = projectsWithTests[i];
        testsDllDir.push(srcDir + p + testsSuffix +
            "/bin/" + configuration + "/*" +
            p + testsSuffix + ".dll");
    }

    console.log(testsDllDir);

    return gulp.src(testsDllDir, { read: false })
        .pipe(xunit({
            executable: "src/packages/xunit.runner.console.2.1.0/tools/xunit.console.exe",
            options: {
                nologo: true,
                html: buildFolder + "test_results.html"
            }
        }));
});

gulp.task("package", function () {
    var baseBinMatch = "src/**/bin/" + configuration + "/";
    return gulp.src([
        baseBinMatch + "*.{exe,dll,config}",
        baseBinMatch + "*_job.xml",
        "!" + baseBinMatch + "*.tests.*",
        "!" + baseBinMatch + "*xunit.*",
    ])
        .pipe(flatten())
        .pipe(gulp.dest(buildFolder));
});

gulp.task("build", function (callback) {
    runseq("info",
        "clean",
        "nuget-restore",
        "assemblyInfo",
        "compile",
        "test",
        "package",
        "info",
        callback);
});
