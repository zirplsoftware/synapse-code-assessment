Review core README.md in '1 Solution Items' for overall architecture.

I chose .Net Standard 2.0 project as it is the most flexible option for sharing between .Net Framework and .Net projects.
Some functionality and NuGet packages will not be supported. 
If you need a feature that is .Net Framework specific or .Net specific
please restructure the solution as follows:

- Synapse.Domain.Services.Shared (Shared project type)
    - maintain Synapse.Domain.Services as the default namespace
	- move all code from this project into that one
	- delete this project
	- define interfaces for all classes with separate .Net Framework and .Net implementions
- Synapse.Domain.Services.DotNetFramework (Class Library .Net Framework)
    - maintain Synapse.Domain.Services as the default namespace
	- reference Synapse.Domain.Services.Shared
	- create implemenations for all .Net Framework specific interfaces
- Synapse.Domain.Services.DotNet (Class Library .Net)
    - maintain Synapse.Domain.Services as the default namespace
	- reference Synapse.Domain.Services.Shared
	- create implemenations for .Net specific interfaces
