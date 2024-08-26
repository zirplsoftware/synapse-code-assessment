Review core README.md in '1 Solution Items' for overall architecture.

This is for framework code that knows nothing about the Synapse business.
It simply helps other code be written more quickly and with fewer bugs.

I chose .Net Standard 2.0 project as it is the most flexible option for sharing between .Net Framework and .Net projects.
It is ideal to keep maintain this strategy, adding additional projects
if .Net Framework/.Net-specific framework functionality is needed.