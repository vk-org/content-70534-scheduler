# Microsoft Azure Exam 70-534 Prep Course
## Azure Scheduler

### What's in this repository
This repository contains the templates and Azure Function code to recreate the demonstration of Azure Scheduler contained in the Azure Exam 70-534 prep course on LinuxAcademy.com.

**Important note:** Your Linux Academy subscription does _not_ include Azure services. If you would like to deploy and run this code, you must do so on your own Azure subscription, which may result in additional charges. If so, Microsoft will bill you directly for those services. Again, your LinuxAcademy.com subscription does _not_ include Azure services.

### The folders
**template**: This folder contains an ARM template (template.json) and a parameters file (parameters.json) to deploy the same kinds of services used in the demo. Note that if you opt to use the parameters.json file, you must first provide valid values in that file.

**launchschedule**: This Azure Function provides a HTTPTrigger function that you can call via Scheduler. It is meant to replicate the kind of maintenance task which Scheduler is often used to perform.