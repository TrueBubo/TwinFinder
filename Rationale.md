# Rationale behind this project
This project is able to find similar files or other types of media based on their content. This will be of much use to
everyone who has ever forgotten where they copied the files, if they created something already or just wanting to know if
two people plagiarized each other.

# Program organization
The program is divided into multiple namespaces, which are similar in parts they handle. This helps the developers taking
part in this project to quickly navigate which part they want to edit in order to achieve their goals.
The current namespaces in this project include:
- `Configuration` for everything regarding options. Their parsing, reading and values
- `ContentAnalysis` for statistical analysis of contents, including finding lexicographically similar contents
- `ContentFinding` for converting patterns to specific file list
- `ContentIO` for reading and writing to std and another sources and destinations

There are two independent files `Project.cs` and `Program.cs`. `Project.cs` includes the constants for the project as a 
whole, and `Program.cs` is responsible for connecting various components of the program to accomplish the aim of this program.

The classes serve as little for making sure the developer knows what to expect just by looking at the name. They rely on
    external handles to be more modular and not require maintenance over multiple files. The method rely on interfaces to make
the program modular for future changes or expansions.

# Expansions
This program has many possible ways it could be expended. Due to its modularity, we could easily interact with other
sources for contents such as databases. The expansion to support more languages could be done by adding another synonym 
files. Lastly there are several modes which could be implemented too, such as Levenshtein distance.

# Conclusion
During this project I have encountered many difficulties. I should have used dataclasses earlier as they made it much 
easier to modify their structure without modifying the structure of the code using such data. Another good idea I have
came out with was separate classes for constants as before I have used them I have got trouble with referencing them as they
were defined all over the program, and I had to find them. Now when I want to change them, I change them in one place, and
as all other uses reference this place, they are updated automatically. If I were to do this again, I would compress the
synonym files as the long keys repeating makes the file larger, which makes the program slower as it has to read it.

The things I am the most grateful that I have done are binary file handling and multithreading. Binary files were the
biggest culprit in the slow running of this program as they are huge, and do not contain useful text. Therefore, I have
decided, and ignore them as they took way too long to load. Multithreading was essential as it now can read multiple files
in parallel, which significantly boost the program speed, as now small files do not need to wait for big files to be completely
loaded. I am happy with my decision to introduce configuration so early on, as it provided single point of entry for all
the settings, and they were managed to contain valid options, which made me less reliant on checking the options, which I
believe made my program less brittle.