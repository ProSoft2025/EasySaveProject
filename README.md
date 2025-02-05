# EasySaveProject
# Introduction 
Easysave is a back up folder software. It's a school project created by a team of 5 students. 

### Context
Our team has just integrated the software publisher ProSoft.   Under the responsibility of the CIO, we are in charge of managing the "EasySave" project which consists in developing a backup software.  As any software of the ProSoft Suite, the software will be integrated into the pricing policy.
- Unit price : 200 €HT
- Annual maintenance contract 5/7 8-17h (updates included): 12% purchase price (Annual contract tacitly renewed with revaluation based on the SYNTEC index)
During this project, we will have to ensure the development, the management of major and minor versions, but also the documentation (user and customer support).  To ensure that our work can be taken over by other teams, we must work within certain constraints such as the tools used.

### Tools used
- [Visual Studio 2022](https://visualstudio.microsoft.com/fr/vs/) (enterprise version)
- [Lucidchart](https://www.lucidchart.com/pages/fr) (for diagram)

### Version 1
The specifications for the first version of the software are as follows:

The software is a console application using . Net Core.

The software must allow to create up to 5 backup jobs

A backup job is defined by :

- A name for the backup
- A source directory
- A target directory
- One type (full backup, differential backup)

The software must be usable by English and French users

The user can request the execution of one of the backup jobs or the sequential execution of all the jobs.

Daily Log File :

The software must write in real time in a daily log file all actions performed during backups (transfer of a file, creation of a directory,...). The minimum information expected is:

- Timestamp
- Backup name
- Full source file address (UNC format)
- Full destination file address (UNC format)
- File size
- File transfer time in ms (negative if error)

File Real-time status: 

The software must record in real time, in a single file, the progress of the backup work and the action in progress. The information to be recorded for each backup job is at least:

- Name of the backup work
- Timestamp of last action
- Status of the Backup job (ex: Active, Not Active...). If the job is active:
- The total number of eligible files
- The size of the files to transfer
- The progression
- Number of files remaining
- Size of remaining files
- Full address of the Source file being saved
- Full destination file address

The locations of both files (daily log and real-time status) will have to be studied to work on our clients servers. Therefore, locations of type “c: temp” should be avoided.

The files (daily log and status) and any configuration files will be in JSON format. To allow quick reading via Notepad, it is necessary to put line breaks between JSON elements. Pagination would be a plus.

# Authors
- BUTEZ Clément
- DUPRE Jérémy
- REVEL Ludovic
- PENTADO Mathéo
- DESROUSSEAUX Tom
