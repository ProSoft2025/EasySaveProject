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
- [Mermaid Live editor](https://mermaid.live/edit) (for diagram)

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

### Version 2

EasySave version 1 has been distributed to many customers.  

Following a customer survey, the management decided to create a version 2 with the following improvements:  

* Graphic Interface 

Abandoning Console mode. The application must now be developed in Avalonia. 

 * Unlimited number of jobs 

The number of backup jobs is now unlimited.  

* Encryption via CryptoSoft software 

The software will have to be able to encrypt the files using CryptoSoft software (made during prosit 5).  Only the files with extensions defined by the user in the general settings should be encrypted. 

 * Evolution of the Daily Log file 

The daily log file must contain additional information: Time needed to encrypt the file (in ms)   
 
* Business software 

If the presence of business software is detected, the software must prohibit the launch of a backup job. In the case of sequential jobs, the software must complete the current job and stop before launching the next job. The user will be able to define the business software in the general settings of the software. (Note: the calculator application can substitute the business software during demonstrations).
#Getting Started
To get a local copy up and running follow these simple example steps. But before anything else you should be sure to have Visual Studio 2019 in order to run the program.

### Version 3

EasySave version 2 has been distributed to many customers.  

Following a customer survey, the management decided to create a version 3 with the following improvements:

* Backup in parallel

The backup work will be done in parallel (no more sequential mode ).

* Management of priority files

No backup of a non-priority file can be done as long as there are priority extensions pending on at least one job. Extensions are declared by the user in a predefined list (present in general settings).

* Remote console

To allow real-time monitoring of the progress of backups on a remote workstation, we developed an HMI allowing a user to monitor the evolution of backup jobs on a remote workstation.

* Log file

For the logs it is possible to view them with a JSON and XML extension.

Once you will have set up your computer You will have to clone this project.

You can do it in HTTPS with this link: 

```
https://github.com/ProSoft2025/EasySaveProject.git
```

Or by opening it with Github Desktop or downloading the ZIP.

Then refer to the usage section and you will be ready to go !

## Start the application
* Version 1 : [Documentation User] named UserDoc V1.md
* Version 2 : [Documentation User] named UserDoc V2.md
* Version 3 : [Documentation User] named UserDoc V3.md

# Authors
- BUTEZ Clément
- DUPRE Jérémy
- REVEL Ludovic
- PENTADO Mathéo
- DESROUSSEAUX Tom
