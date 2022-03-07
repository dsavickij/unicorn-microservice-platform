# Unicorn.Templates

Solutions here consists of project template for creating certain type of Unicorn projects.

As of now, there are templates for:

* Unicorn microservice

## Template installation

Templates can be installed using several different ways, but here only installed from nuget package is provided. Installion from nuget pacakge is superior as it allows distribution of templates by putting the nuget in public or private nuget repository.

To install the templates, do the following steps:

1. **Create template nuget package from nuspec file**. In the folder with nuspec file run the command `nuget pack Unicorn.Templates.nuspec`. This will create template nuget package in the folder
2. **Install the templates**. In the folder with created nuget package run the command `dotnet new -install Unicorn.Templates.1.0.0.nupkg`

After succesful instalation the project templates will become available for new projects in Visual Studio and JetBrains Rider. The templates will also appear in Visual Studio project creation wizard:

![image](https://user-images.githubusercontent.com/22943668/157010388-58bacee5-df8e-410a-8d6a-903f8b0567d5.png)
