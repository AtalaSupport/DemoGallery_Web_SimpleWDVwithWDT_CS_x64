<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="SimpleWdvwithWDT._Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Simple Web Document Viewer with WebDocument Thumbnailer and Annotations</title>
    <script src="WebDocViewer/jquery-3.5.1.min.js" type="text/javascript"></script>
    <script src="WebDocViewer/jquery-ui-1.14.0.min.js" type="text/javascript"></script>
    <script src="WebDocViewer/raphael-min.js" type="text/javascript"></script>
    <script src="WebDocViewer/clipboard.min.js" type="text/javascript"></script>
    <script src="WebDocViewer/atalaWebDocumentViewer.js" type="text/javascript"></script>

    <link href="WebDocViewer/jquery-ui-1.14.0.min.css" rel="Stylesheet" type="text/css" />
    <link href="WebDocViewer/atalaWebDocumentViewer.css" rel="Stylesheet" type="text/css" />
</head>
<body>
<form id="form1" runat="server">
    <input type="button" id="btn_open1" value="Open 2Page300dpi.tif" onclick="_thumbs.openUrl('/images/2Page300dpi.tif', ''); return false;" />
    <input type="button" id="btn_open2" value="Open DocCleanMultipage.tif" onclick="_thumbs.openUrl('/images/DocCleanMultipage.tif', ''); return false;" />
    <input type="button" id="btn_open3" value="Open DocCleanMultipage.pdf" onclick="_thumbs.openUrl('/images/DCM.pdf', ''); return false;" />
    <input type="button" id="btn_AnnoColor" value="TOGGLE ANNO COLOR" onclick="toggleAnnoColor(); return false;" />
    <input type="button" id="btnTest" value="MOVABLE" onclick="toggleMovable(); return false;" />
    <div style="width: 900px;">
    <div id="_toolbar1"></div>
    <div id="_containerThumbs" style="width: 180px; height: 600px;
    display: inline-block;"></div>
    <div id="_containerViewer" style="width: 710px; height: 600px;
    display: inline-block;"></div>
    </div>
    
    </form>
    <script type="text/javascript" language="javascript">
    var _docUrl = '/Images/DocCleanMultipage.tif';
    var _serverUrl = '/WebDocViewerHandler.ashx';
    
    var _viewer = new Atalasoft.Controls.WebDocumentViewer({
        parent: $('#_containerViewer'), // parent container to put the viewer in
        toolbarparent: $('#_toolbar1'), // parent container to put the viewer toolbar in
        serverurl: _serverUrl, // server handler url to send image requests to
        allowannotations: true, // flag to enable annotations
        savepath: 'Saved/', // relative url to save annotation data to
        showscrollbars: true,
        showselecttools: false,
        direction: Atalasoft.Utils.ScrollDirection.Vertical,
        forcepagefit: true,
        fitting: Atalasoft.Utils.Fitting.Width,
        textannomobilezoom: false,
        singlepage: false
    });
    var _thumbs = new Atalasoft.Controls.WebDocumentThumbnailer({
        parent: $('#_containerThumbs'), // parent container to putthe thumbnails in
        serverurl: _serverUrl, // server handler url tosend image requests to
        documenturl: _docUrl, // document url relative to the server handler url
        //annotationsurl: _annUrl, // annotation file to load upon page loading
        allowannotations: true, // flag to enable annotations 
        viewer: _viewer, // link actions to the _viewer so they open the same doc
        allowdragdrop: false,
        showscrollbars: true,
        showpagenumber: true,
        selectionmode: Atalasoft.Utils.SelectionMode.SingleSelect,
        selecteditemsorder: Atalasoft.Utils.SelectedItemsOrder.ItemIndexOrder,
        direction: Atalasoft.Utils.ScrollDirection.Vertical,
    });



    /* **************************************************************
     * BEGIN SAVE HANDLER EXAMPLE
     * *************************************************************/

    // Very common to bind to documentsaved event
    // https://atalasoft.github.io/web-document-viewer/Atalasoft.Controls.WebDocumentViewer.html#event:documentsaved
    _viewer.bind('documentsaved', saveHandler);


    function saveHandler(e) {
        // e.success will be true on successful save, false on error saving
        // e.fileName wil lbe the file name of successfully saved file (or undefined if e.success == false)
        // e.customData contains custom data set via the handler DocumentSaveResponseSend
        //    see https://www.atalasoft.com/docs/dotimage/docs/html/E_Atalasoft_Imaging_WebControls_WebDocumentRequestHandler_DocumentSaveResponseSend.htm

        var msg = "  e.success: " + e.success;

        if (e.success) {
            msg = msg + "  \n  e.fileName: " + e.fileName;
            if (e.customData != null) {
                msg = msg + "\n  e.customData: " + e.customData;
            }
        }

        alert('saveHandler...\n' + msg);
    }
    /* **************************************************************
     * END SAVE HANDLER EXAMPLE
     * *************************************************************/


    /* **************************************************************
     * BEGIN SETTING UP STAMP ANNOTATIONS
     * *************************************************************/
    // In order for the stamp annotaion type to appear
    // you need to define the stamp values
    // NOTE that a stamp annotation is not image based
    // you'll need to use an image annotation for that
    // Images are done similarly but with setImages
    // see https://atalasoft.github.io/web-document-viewer/Atalasoft.Controls.WebDocumentViewer-AnnotationController.html#setImages
    _viewer.annotations.setStamps(
    [
        {
            'name': 'Approved stuff multi-word',
            'fill': {
                'color': 'white',
                'opacity': 0.50
            },
            'outline': {
                'color': 'green',
                'width': 15
            },
            'text': {
                'value': 'APPROVED',
                'align': 'center',
                'font': {
                    'bold': false,
                    'color': 'green',
                    'family': 'Georgia',
                    'size': 64
                },
                'autoscale': true
            }
        },
        {
            'name': 'Rejected',
            'fill': {
                'color': 'white',
                'opacity': 0.50
            },
            'outline': {
                'color': 'red',
                'width': 15
            },
            'text': {
                'value': 'REJECTED',
                'align': 'center',
                'font': {
                    'bold': false,
                    'color': 'red',
                    'family': 'Georgia',
                    'size': 64
                }
            }
            }
    ]);
   /* **************************************************************
    * END SETTING UP STAMP ANNOTATIONS
    * *************************************************************/


        // The following is entirely optional
        // this is an example to iterate all annotations on a single page and update them 
        // and successfully atler a setting and update the annotation
        // this can be safely removed if you don't use such a feature - it's purely for example
        function updateAllAnnosOnPage(pageIndex) {
            var annos = _viewer.getAnnotationsFromPage(pageIndex);
            if (annos != null) {
                for (var i = 0; i < annos.length; i++) {
                    var anno = annos[i];
                    // YOUR CODE HERE FOR ANY PROPERTIES YOU'RE LOOKING TO UPDATE
                    anno.update();
                }
            }
        }


        // this is code based on updateAllAnnosOnPage which sets the burn flag to true/false as 
        // per the input argulent for burnvalue
        // it is used by teh burnAllAnnotations code as we need to loop all pages
        //then set the property of all annotations on each page
        // Burn all annotations on a single page
        // this can be safely removed only if you also remove burnAllAnnos if you don't use such a feature - it's purely for example
        function burnAllAnnosOnPage(pageIndex, burnvalue) {
            var annos = _viewer.getAnnotationsFromPage(pageIndex);
            if (annos != null) {
                for (var i = 0; i < annos.length; i++) {
                    var anno = annos[i];
                    anno.burn = burnvalue;
                    anno.update();
                }
            }
        }

        // this code shows how to loop through all pages  - in this case it's using that to then call
        // a page specific operation on each page found
        // this code could be adapted for any operation where you need to do someting to all pages
        // this can be safely removed if you don't use such a feature - it's purely for example
        // burn all annos on all pages
        function burnAllAnnos(burnvalue) {
            if (burnvalue == null) {
                burnvalue = true;
            }
            var pageCount = _viewer.getDocumentInfo().count;
            if (pageCount != null && pageCount > 0) {
                for (var i = 0; i < pageCount; i++) {
                    burnAllAnnosOnPage(i, burnvalue);
                }
            }
        }

    //// The following is entirely optional
    //// this is an example to iterate all annotations on a single page and update them 
    //// and successfully atler a setting and update the annotation
    //// this can be safely removed if you don't use such a feature - it's purely for example
    //function updateAllAnnosOnPage(pageIndex) {
    //    var annos = _viewer.getAnnotationsFromPage(pageIndex);
    //    if (annos != null) {
    //        for (var i = 0; i < annos.length; i++) {
    //            var anno = annos[i];
    //            // YOUR CODE HERE FOR ANY PROPERTIES YOU'RE LOOKING TO UPDATE
    //            anno.update();
    //        }
    //    }
    //}



    ////// Here is an example of how to hide buttons on the toolbar
    ////// in this example we're getting the tool button for zoom out and zoom in
    ////// and for the various annotation types
    ////// any valid atala_toolbutton_xxxx  can be hidden this way (calling .show() unhides)
    ////// This runs after load so that the toolbar is there to actually grab hold of and hide
    //$(function () {
    //    //// example for the zoom buttons
    //    $('.atala_tool_button_zoom_out ').hide();
    //    $('.atala_tool_button_zoom_in').hide();
    //    $('.atala_tool_button_fit_none').hide();
    //    $('.atala_tool_button_fit_width').hide();
    //    $('.atala_tool_button_fit_best').hide();
    //    $('.atala_tool_button_rotate_left').hide();
    //    $('.atala_tool_button_rotate_right').hide();

    //    //// example for annotation buttons
    //    $('.atala_tool_button_highlight_anno').hide();
    //    $('.atala_tool_button_line_anno').hide();
    //    $('.atala_tool_button_freehand_anno').hide();
    //    $('.atala_tool_button_rect_anno').hide();
    //    $('.atala_tool_button_text_anno').hide();
    //    $('.atala_tool_button_ellipse_anno').hide();
    //    $('.atala_tool_button_line_anno').hide();
    //    $('.atala_tool_button_lines_anno').hide();
    //    $('.atala_tool_button_polygon_anno').hide();
    //});

    </script>
    
</body>
</html>
