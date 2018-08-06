# AXLabelTranslator
Tool to translate labels in Dynamics AX (2009/2012) using Bing Translate

-- PREPERATION --

Before you can use the C# program you need to follow this guide to setup the Azure API, and get an access key.

https://docs.microsoft.com/en-us/azure/cognitive-services/translator/translator-text-how-to-signup


Insert the API key in C# in the file ConsoleUI\Program.cs on line 11. (Replace the text ""REPLACE WITH APIKEY)


-- HOW TO USE --

Start with importing the X++ code from :

AXCode\Job_LabelTranslateExport.xpo

AXCode\Job_LabelTranslateImport.xpo


And run the Job LabelTranslateExport to export the list of Danish labels to the file C:\TEMP\SourceLabelFile.csv

Run the C# program, it will read all the labels in the file C:\TEMP\SourceLabelFile.csv, translate them and write the result to the file C:\TEMP\TranslatedLabelFile.csv

If you are running a version controll system for AX, you might need to checkout the label file you are working on, before you run the next step.

Run the Job LabelTranslateImport to import the translated labels from the file C:\TEMP\TranslatedLabelFile.csv

Then again if you are running a version controll system for AX, you need to checkin the label file.
