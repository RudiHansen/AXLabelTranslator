# AXLabelTranslator
Tool to translate labels in Dynamics AX (2009/2012) using Bing Translate

## PREPERATION
Before you can use the C# program you need to follow this guide to setup the Azure API, and get an access key.

https://docs.microsoft.com/en-us/azure/cognitive-services/translator/translator-text-how-to-signup

Insert the API key in C# in the file ConsoleUI\Settings.cs on line 13. (Replace the text ""REPLACE WITH APIKEY)

## HOW TO USE 

### Dynamics AX
Start with importing the X++ code from :

AXCode\Class_LabelTranslate.xpo

This will import the class LabelTranslate, for now you will have to do some changes in the class to use it.
In the New method, you have to change the variables to make the class do what you want.

### C# 
Edit ConsoleUI\Settings.cs, in this file the API key and Laguage for translation is setup.
Run the C# program, it will read all the labels in the file C:\TEMP\SourceLabelFile.csv, translate them and write the result to the file C:\TEMP\TranslatedLabelFile.csv

### Dynamics AX
If you are running a version controll system for AX, you might need to checkout the label file you are working on, before you run the next step.

Run the Job LabelTranslateImport to import the translated labels from the file C:\TEMP\TranslatedLabelFile.csv

Then again if you are running a version controll system for AX, you need to checkin the label file.

## Changelog
### TODO
002.	AX - Add dialog to class, all settings must be set in Dialog
003.    C# - API Key must be saved locally
004.    C# - Move variable FilePath, SourceLabelFile and TranslatedLabelFile to Settings.cs
004.    C# - Change UI to Windows Forms
005.    C# - All Values in Settings.cs should be changable in Form.

### DONE
001.	Use variables for settings C# Settings.cs, AX LabelTranslate.new