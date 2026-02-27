using System;
using System.IO;
using System.Web;
using Atalasoft.Imaging.Codec;
using Atalasoft.Imaging.Codec.Pdf;
using Atalasoft.Imaging.WebControls;
using Atalasoft.Annotate;
using System.Drawing;
using Atalasoft.Imaging;

public class WebDocViewerHandler : WebDocumentRequestHandler
{
    static WebDocViewerHandler()
    {
        //// for PDF support you need to reference Atalasoft.dotImage.PdfReader.dll and have a PdfReader license
        RegisteredDecoders.Decoders.Add(new PdfDecoder() { Resolution = 200, RenderSettings = new RenderSettings() { AnnotationSettings = AnnotationRenderSettings.RenderNone } });
        //// This adds the OfficeDecoder .. you need proper licensing and additional OfficeDecoder dependencies (perceptive filter dlls)
        //RegisteredDecoders.Decoders.Add(new OfficeDecoder() { Resolution = 200 });
    }

    public WebDocViewerHandler()
    {
        //// *************************** BASE DOCUMENT VIEWING EVENTS ***************************
        //// these two events are the base events for the loading of pages
        //// the documentInfoRequested fires to ask for the number of pages and size of the first page (minimum requirements)
        //// then, each page needed (and only when it's needed - lazy loading) will fire an ImageRequested event
        //this.DocumentInfoRequested += new DocumentInfoRequestedEventHandler(WebDocViewerHandler_DocumentInfoRequested);
        //this.ImageRequested += new ImageRequestedEventHandler(WebDocViewerHandler_ImageRequested);

        //// *************************** ADDITIONAL DOCUMENT VIEWING EVENTS ***************************
        //// These events are additional/optional events .. 
        //this.AnnotationDataRequested += WebDocViewerHandler_AnnotationDataRequested;
        //this.PdfFormRequested += WebDocViewerHandler_PdfFormRequested;

        //// this is the event that would be used to manually handle page text requests 
        //// (if left unhandled, page text requested wil be handled by the default engine which will provide searchabel text for Searchable PDF and office files)
        //// Manually handling this event is for advanced use cases only
        //this.PageTextRequested += WebDocViewerHandler_PageTextRequested;

        //// *************************** DOCUMENT SAVING EVENTS ***************************
        //this.DocumentSave += WebDocViewerHandler_DocumentSave;
        //this.DocumentStreamWritten += WebDocViewerHandler_DocumentStreamWritten;
        //this.AnnotationStreamWritten += WebDocViewerHandler_AnnotationStreamWritten;


        //// This event is save related but conditional use - see comments in handler
        //this.ResolveDocumentUri += WebDocViewerHandler_ResolveDocumentUri;
        //this.ResolvePageUri += WebDocViewerHandler_ResolvePageUri;

        //// *************************** OTHER EVENTS (usually ignored) ***************************
        //this.ReleaseDocumentStream += WebDocViewerHandler_ReleaseDocumentStream;
        //this.ReleasePageStream += WebDocViewerHandler_ReleasePageStream
    }

    #region BASE DOCUMENT VIEWING EVENTS
    /// <summary>
    /// The whole DocumentOpen process works like this:
    /// On initial opening of the document, DocumentInfoRequested fires once for the document
    ///  The AnnotationDataRequested event may also fire if coditions merit\
    /// then the ImageRequested event will fire for subsequent pages.. 

    /// 
    /// This event fires when the first request to open the document comes in
    /// you can either set e.FilePath to a different value (useful for when the 
    /// paths being opened simply alias to physial files somewhere on the server)
    /// or you can manually handle the request in qhich case you MUST set both e.PageSize and e.PageCount
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    void WebDocViewerHandler_DocumentInfoRequested(object sender, DocumentInfoRequestedEventArgs e)
    {
        ////THESE TWO MUST BE RESOLVED IF YOU ARE MANUALLY HANDLING
        // set e.PageCount to the number of pages this document has
        string filePath = HttpContext.Current.Request.MapPath( e.FilePath );



        //int pageCount = 0;
        //Size pageSize = new Size(0,0);

        //using (FileStream inStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
        //{
        //    pageCount = RegisteredDecoders.GetImageInfo(inStream).FrameCount;
        //    inStream.Seek(0, SeekOrigin.Begin);

        //    using (AtalaImage img = new AtalaImage(inStream,0, null))
        //    {
        //        pageSize = img.Size;
        //    }

        //}
        //e.PageCount = pageCount;
        //e.PageSize = pageSize;

        //// e.PageSize to the System.Drawing.Size you want pages forced into


        // int colorDepth = 1;
        //e.ColorDepth = colorDepth;

        //e.AnnotationFilePath = "";

        //e.IsVector = false;

        //e.Resolution = new Atalasoft.Imaging.Dpi(200, 200, Atalasoft.Imaging.ResolutionUnit.DotsPerInch);
    }

    /// <summary>
    ///  you can use e.FilePath and e.FrameIndex to work out what image to return
    /// set e.Image to an AtalaImage to send back the desired image
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    void WebDocViewerHandler_ImageRequested(object sender, ImageRequestedEventArgs e)
    {
        //// NOTE: If you are using file-based viewing, but the incoming FilePath is an 
        //// alias/ needs parsing but ends up just pointing to a valid file you can just set e.FilePath to the FULL PATH (on the server) or full UNC Path
        //// Example:
        //// Incoming request for "docId-foo" needs to resolve to C:\SomeDirectory\MyDoc.tif
        //e.FilePath = @"C:\SomeDirectory\MyDoc.tif";
        //// or more likely:
        // e.Filepath = SomeMethodToReturnFullPathForIncomingRequest(e.FilePath);

        //// This is an approximation of what the default ImageRequested event would do if you didn't handle it manually
        //e.Image = new AtalaImage(HttpContext.Current.Request.MapPath(e.FilePath), e.FrameIndex, null);

        // When you manually handle it, you need to pass an AtalaImage back to e.Image
        // e.Image = SomeMethodThatReturnsAtalaImage(e.FilePath, e.FrameIndex);
        // but there's no reason that e.FilePath couldn't be a database PKID or similar.. 
        //your method would look up the record and get the data needed to construct and return a valid AtalaImage
    }
    #endregion BASE DOCUMENT VIEWING EVENTS

    #region ADDITIONAL DOCUMENT VIEWING EVENTS
    /// <summary>
    /// When an OpenUrl that includes an AnnotatoinUrl is called, the viewer default action is to go to the 
    /// url specified ... which an XMP file containing all annotation layers (serialized LayerData[]) is read and loaded
    /// Manual handling of this event would be needed if one were to be loading annotations from a Byte[] or from a databasae, etc
    /// NOTE: this event WILL NOT FIRE if there was no annotationsUrl or a blank /null AnnotaitonsUrl was passed in the OpenUrl call
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    void WebDocViewerHandler_AnnotationDataRequested(object sender, AnnotationDataRequestedEventArgs e)
    {

        //e.Layers = new LayerData[]{}; //"entire Layer of all annotations scooped up from File here";
        
        //e.Length = 7;
        //e.Offset = 2;

        // READ-ONLY INPUT VALUE
        //e.FilePath = the passed in FilePath of where to find the annotations (when the OpenUrl is called, this will populate with the AnnotationsUrl value

        // WHAT YOU NEED TO HANDLE THIS EVENT
        // To successfully handle this event, you must populate the e.Layers
        //e.Layers = a LayerData[] containing ALL Annotation Layers for the whole document
    }

    /// <summary>
    /// This event lets you intercept requests for page text to provide your own
    /// manually handling this is not recommended, but the hook is here
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    void WebDocViewerHandler_PageTextRequested(object sender, PageTextRequestedEventArgs e)
    {
        // You can use the e.FilePath  and e.Index to know which document and page are being requested respectively
        // you can then go extract text/ get the text to the control you must ultimately set
        // e.Page = .. an Atalasoft.Imaging.WebControls.Text.Page object containing the page text and position data

        //// Here is an example of manually extracting .. essentially a window into what the default handler will do
        //var serverPath = HttpContext.Current.Server.MapPath(e.FilePath);
        //if (File.Exists(serverPath))
        //{
        //    using (var stream = File.OpenRead(serverPath))
        //    {
        //        try
        //        {
        //            var decoder = RegisteredDecoders.GetDecoder(stream) as ITextFormatDecoder;
        //            if (decoder != null)
        //            {
        //                using (var extractor = new SegmentedTextTranslator(decoder.GetTextDocument(stream)))
        //                {
        //                    // for documents that have comlicated structure, i.e. consist from the isolated pieces of text, or table structure
        //                    // it's possible to configure nearby text blocks are combined into text segments(text containers that provide
        //                    // selection isolated from other document content)
        //                    //extractor.RegionDetection = TextRegionDetectionMode.BlockDetection;
        //                    // each block boundaries inflated to one average character width and two average character height
        //                    // and all intersecting blocks are combined into single segment.
        //                    // Having vertical ratio bigger then horizontal behaves better on column-layout documents.
        //                    //extractor.BlockDetectionDistance = new System.Drawing.SizeF(1, 2);
        //                    e.Page = extractor.ExtractPageText(e.Index);
        //                }
        //            }
        //        }
        //        catch (ImageReadException imagingException)
        //        {
        //            System.Diagnostics.Debug.WriteLine("Text extraction: image type is not recognized. {0}", imagingException);
        //        }
        //    }
        //}
    }

    /// <summary>
    /// This event only fires if the allowforms: true was set and you have a license for the PdfDoc addon
    /// 
    /// It is used to provide the PDF document needed by the form filling components
    /// If left unhandled it will still fire internally and will simply 
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    void WebDocViewerHandler_PdfFormRequested(object sender, PdfFormRequestedEventArgs e)
    {
        //// READ ONLY VALUE
        //e.FilePath contains the original File path in the intial request use this to figure out which file/document/record to fetch

        //// Required if handling you must provide an Atalasoft.PdfDoc.Generating.PdfGeneratedDocument 
        //// containing the PDF with the fillable form if you manually handle this event
        //FileStream fs = new System.IO.FileStream(e.FilePath, FileMode.Open, FileAccess.Read, FileShare.Read);
        //e.PdfDocument = new Atalasoft.PdfDoc.Generating.PdfGeneratedDocument(fs);

        // If you return NULL for e.PdfDocument then the system will treat the PDF as not being a fillable PDF form
    }
    #endregion ADDITIONAL DOCUMENT VIEWING EVENTS

    #region DOCUMENT SAVING EVENTS
    /// <summary>
    /// This event fires initially on DocumentSave
    /// Document saving rundown:
    /// The DocuemtnSave fires first.. it gives you the chance to provide alternate streams for 
    /// e.DocumentStream and e.AnnotationStream
    /// Then when the Document is written the DocumentStreamWritten event will fire.. e.DocumentStream will give oyu access to the 
    /// document stream
    /// Then the AnnotationStreamWritten will fire (if there were annotations to save) and e.AnnotationStream will give you access
    /// to the annotation stream.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    void WebDocViewerHandler_DocumentSave(object sender, DocumentSaveEventArgs e)
    {
        //// If you want to handle Annotations  / Document into a database or similar, 
        //// do this and then handle the writing to db in the AnnotationStreamWritten / DocumentStreamWritten event
        //e.AnnotationStream = new MemoryStream();
        //e.DocumentStream = new MemoryStream();

        //// If you want to provide an alternate location.. you can't modify e.FilePath - its read only
        //// so instead, do this
        //e.AnnotationStream = new FileStream("FullyQualifiedPathForAnnotationsToWriteToHere", FileMode.Create, FileAccess.ReadWrite, FileShare.Read);
        //e.DocumentStream = new FileStream("FullyQualifiedPathForDocumentToWriteToHere", FileMode.Create, FileAccess.ReadWrite, FileShare.Read);
    }

    /// <summary>
    /// e.AnnotationStream  will contain the annotations that were written
    /// The DocumentStreamWritten will ALWAYS fire before the AnnotationStreamWritten
    /// even thought this event has e.DocumentStream.. it will alway be null when 
    /// AnnotationStreamWritten fires.. use the DocumentStreamWritten event to handle the DocumentStream
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    void WebDocViewerHandler_AnnotationStreamWritten(object sender, DocumentSaveEventArgs e)
    {
        //// You should rewind first, then you can store in a DB or similar
        //// EXAMPLE: passed an e.MemoryStream for writing to a DB:
        //MemoryStream annoStream = e.AnnotationStream as MemoryStream
        //if (annoStream != null)
        //{
        //    annoStream.Seek(0,  SeekOrigin.Begin);
        //    SomeMethodToStoreByteArrayToDb(annoStream.ToArray());
        //}

        //// NOTE: if you set e.AnnotationStream to a file stream in the DocumentSave you can skip handling 
        //// this event and the system will take care of closing it. You would only need to handle this event 
        //// if you are doing something else with it other than letting it write to where you specified in that 
        //// FileStream, such as post-processing or similar
    }

    /// <summary>
    /// e.DocumentStream  will contain the entire document being saved
    /// the DocumentStreamWritten will ALWAYS fire before the AnnotationStreamWritten
    /// Even thought this event has e.AnnotationStream.. it will alway be null when 
    /// DocumentStreamWritten fires.. use the AnnotationStreamWritten event to handle the AnnotationStream
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    void WebDocViewerHandler_DocumentStreamWritten(object sender, DocumentSaveEventArgs e)
    {
        //// You should rewind first, then you can store in a DB or similar
        //// EXAMPLE: passed an e.MemoryStream for writing to a DB:
        //MemoryStream docStream = e.DocumentStream as MemoryStream
        //if (docStream != null)
        //{
        //    docStream.Seek(0,  SeekOrigin.Begin);
        //    SomeMethodToStoreByteArrayToDb(docStream.ToArray());
        //}

        //// NOTE: if you set e.DocumentStream to a file stream in the DocumentSave you can skip handling 
        //// this event and the system will take care of closing it you would only need to handle this event 
        //// if you are doing something else with it other than letting it write to where you specified in that 
        //// FileStream such as post-processing or similar
    }

    /// <summary>
    ///  The ResolveDocumentUri event is only needed if you've manually handled DocumentInfoRequested and ImageRequested 
    /// so that the e.FilePath is not pointing to the original document (before any alterations like rotation/deletion/reordering etc..)
    /// The event fires on save
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    void WebDocViewerHandler_ResolveDocumentUri(object sender, ResolveDocumentUriEventArgs e)
    {
        //// EXAMPLE:
        //e.DocumentStream = SomeMethodThatReturnsAValidStreamObjectContainingTheFullOriginal(e.DocumentUri);
    }

    /// <summary>
    /// Fires when a source page stream is requested while performing save operation
    /// During document save it is necessary to get the source document pages to combine them into the destination stream. 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    void WebDocViewerHandler_ResolvePageUri(object sender, ResolvePageUriEventArgs e)
    {
        //// it is often best to leave this unhandled.. the ResolveDocumentUri is usually sufficient for situations where the document tream is neede for save
        //// The time to use this event is if your original opened document is a combination of multiple different source streams/documents 
        //// it is for very advanced use cases only
    }
    #endregion DOCUMENT SAVING EVENTS

    #region OTHER EVENTS
    // The events in this section are almost never used manually by customers.. 
    // They may have some use in extremely difficult/complex use cases, but for the most part should be left 
    // un-handled in your custom  handler .. let the control use its defaults

    /// <summary>
    /// Fires when a document release stream occurs on document save. This event is raised only for streams that were provided in ResolveDocumentUri event. 
    /// After document save operation it is necessary to release the obtained in ResolveDocumentUri event document streams. Fires once for each stream. 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    void WebDocViewerHandler_ReleaseDocumentStream(object sender, ResolveDocumentUriEventArgs e)
    {
        //// Usually just leaving this to the default works fine... 
        //// e.DocumentUri contains the original request uri (path) for the document

        //// The default activity will pretty much be
        //e.DocumentStream.Close();
        //e.DocumentStream.Dispose();
    }

    /// <summary>
    /// Fires when a page release stream occurs on document save. 
    /// This event is raised only for streams that were provided in ResolvePageUri event. 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    void WebDocViewerHandler_ReleasePageStream(object sender, ResolvePageUriEventArgs e)
    {
        //// Consider leaving this event alone as the default built in handling works well
        //// e.DocumentPageIndex - The index of the page in the DocumentStream if it is a multi-page document. Default value is 0.
        //// e.DocumentUri - the original document location / path that was passed in
        //// e.SourcePageIndex - The requested index of the page in the document. 
        //// e.DocumentStream - Stream used to override the default file system save while saving the document. Setting this property will take precedence over using the DocumentUri property. 

        //// Manually handling: if the original resolvePageUri was used and you set e.DoucmentUri to an alias value.. do that here
        //// if you provided a DocumentStream in ResolvePageUri, then you may need to call
        //e.DocumentStream.Close();
        //e.DocumentStream.Dispose();
    }
    #endregion OTHER EVENTS
}