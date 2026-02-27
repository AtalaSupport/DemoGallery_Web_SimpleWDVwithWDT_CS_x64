SimpleWDVwithWDT is designed to be a bare-bones, minimal visual Studio soluiont
which demonstrates the absolute basics of using WebDocumentViewer and WebDocumentThumbnailer
in a modern HTML5 web app

It's not meant to do anything fancey. Just quickly use out of the box features to get files viewing.

We stray a bit from absolute minimum in that we add a PdfDecoder to enable reaing of PDF files as 
this is used by a very large number of our customers.

Also, there are a couple really useful WDV-based client side code examples we find useful to have

Finally, we also have enumerated all the various events in the WebDocumentRequestHandler 
(found in the WebDocViewerHandler.asxh.cs file). The intent of this is to give a very quick rundown
of what those functions do.. it's mostly commented out but the code is roughly doing whta the "out of box" 
functionality already does as a default. (in other words, it works fine as is without any of the code but the 
example code in there either shows what the default equates to "behind the curtain" or else gives a small 
idea of a standard practice

There are many more specofic or complete examples. This is however one of the best ways to get a "minimal solution"
working and as such is incredibly useful when attempting to reprouce an issue for support.

For the full API for the Clientside components plese see
https://atalasoft.github.io/web-document-viewer/

for the Full API for the WebDocumentRequestHandler class please see:
https://www.atalasoft.com/docs/dotimage/docs/html/T_Atalasoft_Imaging_WebControls_WebDocumentRequestHandler.htm


