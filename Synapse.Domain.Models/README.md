Review core README.md in '1 Solution Items' for overall architecture.

This is the domain model project. It contains the classes that represent the data that is used in the application.
Importantly, it only contains model classes that are within the business's domain. Integration-specific model classes
are in their own integration projects. For small projects, this is irrelevant, but for larger projects, it becomes important.

I chose .Net Standard 2.0 project as it is the most flexible option for sharing between .Net Framework and .Net projects.
It should be rare that the need for a model class exists that has both a .Net Framework and .Net implementation. 
If needed, it's best to discuss the specific case with other developers on the project to determine the best approach.
