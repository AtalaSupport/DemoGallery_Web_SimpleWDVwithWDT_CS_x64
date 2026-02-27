This text file is here solely to keep our source control from deleting/skipping the "empty directory"
This directory is where the print process will dump files for printing

It is up to the developer using our SDK to come up with their own means of cleaning the temp files

One possible approach is to write a "cleanup temp files" method that would saerch /Saved
for any files older than x minutes/ hours / days and attempt to remove them 
you could add calls to this to various server side request so that its run fairly regularly and will result
in cleaning up older files

This exercise is left to the developer 