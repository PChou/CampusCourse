jQuery.extend({
    createUploadIframe: function (id, uri) {
        //create frame
        var frameId = 'jUploadFrame' + id;
        var iframeHtml = '<iframe id="' + frameId + '" name="' + frameId + '" style="position:absolute; top:-9999px; left:-9999px"';
        if (window.ActiveXObject) {
            if (typeof uri == 'boolean') {
                iframeHtml += ' src="' + 'javascript:false' + '"';

            }
            else if (typeof uri == 'string') {
                iframeHtml += ' src="' + uri + '"';

            }
        }
        iframeHtml += ' />';
        jQuery(iframeHtml).appendTo(document.body);

        return jQuery('#' + frameId).get(0);
    },
    createUploadForm: function (id, fileElementId) {
        //create form	
        var formId = 'jUploadForm' + id;
        var fileId = 'jUploadFile' + id;
        var form = jQuery('<form  action="" method="POST" name="' + formId + '" id="' + formId + '" enctype="multipart/form-data"></form>');
        var oldElement = jQuery('#' + fileElementId);
        var newElement = jQuery(oldElement).clone();
        jQuery(oldElement).attr('id', fileId);
        jQuery(oldElement).before(newElement);
        jQuery(oldElement).appendTo(form);
        //set attributes
        jQuery(form).css('position', 'absolute');
        jQuery(form).css('top', '-1200px');
        jQuery(form).css('left', '-1200px');
        jQuery(form).appendTo('body');
        return form;
    },

    handleError: function( s, xhr, status, e )      {  
        // If a local callback was specified, fire it  
        if ( s.error ) {  
            s.error.call( s.context || s, xhr, status, e );  
        }  
  
        // Fire the global callback  
        if ( s.global ) {  
            (s.context ? jQuery(s.context) : jQuery.event).trigger( "ajaxError", [xhr, s, e] );  
        }  
    },

    ajaxFileUpload: function (s) {
        // TODO introduce global settings, allowing the client to modify them for all requests, not only timeout		
        s = jQuery.extend({}, jQuery.ajaxSettings, s);
        var id = new Date().getTime()
        var form = jQuery.createUploadForm(id, s.fileElementId);
        var io = jQuery.createUploadIframe(id, s.secureuri);
        var frameId = 'jUploadFrame' + id;
        var formId = 'jUploadForm' + id;
        // Watch for a new set of requests
        if (s.global && !jQuery.active++) {
            jQuery.event.trigger("ajaxStart");
        }
        var requestDone = false;
        // Create the request object
        var xml = {}
        if (s.global)
            jQuery.event.trigger("ajaxSend", [xml, s]);
        // Wait for a response to come back
        var uploadCallback = function (isTimeout) {
            var io = document.getElementById(frameId);
            try {
                if (io.contentWindow) {
                    xml.responseText = io.contentWindow.document.body ? io.contentWindow.document.body.innerHTML : null;
                    xml.responseXML = io.contentWindow.document.XMLDocument ? io.contentWindow.document.XMLDocument : io.contentWindow.document;

                } else if (io.contentDocument) {
                    xml.responseText = io.contentDocument.document.body ? io.contentDocument.document.body.innerHTML : null;
                    xml.responseXML = io.contentDocument.document.XMLDocument ? io.contentDocument.document.XMLDocument : io.contentDocument.document;
                }
            } catch (e) {
                jQuery.handleError(s, xml, null, e);
            }
            if (xml || isTimeout == "timeout") {
                requestDone = true;
                var status;
                try {
                    status = isTimeout != "timeout" ? "success" : "error";
                    // Make sure that the request was successful or notmodified
                    if (status != "error") {
                        // process the data (runs the xml through httpData regardless of callback)
                        var data = jQuery.uploadHttpData(xml, s.dataType);
                        // If a local callback was specified, fire it and pass it the data
                        if (s.success)
                            s.success(data, status);

                        // Fire the global callback
                        if (s.global)
                            jQuery.event.trigger("ajaxSuccess", [xml, s]);
                    } else
                        jQuery.handleError(s, xml, status);
                } catch (e) {
                    status = "error";
                    jQuery.handleError(s, xml, status, e);
                }

                // The request was completed
                if (s.global)
                    jQuery.event.trigger("ajaxComplete", [xml, s]);

                // Handle the global AJAX counter
                if (s.global && ! --jQuery.active)
                    jQuery.event.trigger("ajaxStop");

                // Process result
                if (s.complete)
                    s.complete(xml, status);

                jQuery(io).unbind()

                setTimeout(function () {
                    try {
                        jQuery(io).remove();
                        jQuery(form).remove();

                    } catch (e) {
                        jQuery.handleError(s, xml, null, e);
                    }

                }, 100)

                xml = null

            }
        }
        // Timeout checker
        if (s.timeout > 0) {
            setTimeout(function () {
                // Check to see if the request is still happening
                if (!requestDone) uploadCallback("timeout");
            }, s.timeout);
        }
        try {

            var form = jQuery('#' + formId);
            jQuery(form).attr('action', s.url);
            jQuery(form).attr('method', 'POST');
            jQuery(form).attr('target', frameId);
            if (form.encoding) {
                jQuery(form).attr('encoding', 'multipart/form-data');
            }
            else {
                jQuery(form).attr('enctype', 'multipart/form-data');
            }
            jQuery(form).submit();

        } catch (e) {
            jQuery.handleError(s, xml, null, e);
        }

        jQuery('#' + frameId).load(uploadCallback);
        return { abort: function () { } };

    },

    uploadHttpData: function (r, type) {
        var data = !type;
        data = type == "xml" || data ? r.responseXML : r.responseText;
        // If the type is "script", eval it in global context
        if (type == "script")
            jQuery.globalEval(data);
        // Get the JavaScript object, if JSON is used.
        if (type == "json") {
            ////////////以下为新增代码///////////////  
            data = r.responseText;
            var start = data.indexOf(">");
            if (start != -1) {
                var end = data.indexOf("<", start + 1);
                if (end != -1) {
                    data = data.substring(start + 1, end);
                }
            }
            ///////////以上为新增代码///////////////
            eval("data = " + data);
        }
        // evaluate scripts within html
        if (type == "html")
            jQuery("<div>").html(data).evalScripts();

        return data;
    }
})


; (function ($) {

    //css
    //attpool-table
    //attpool-downlaod
    //attpool-preview
    //attpool-remove
    $.fn.attpool = function (option) {

        var def = {
            //{id:1,name:'',downloadurl:'',preview:true|false,removeurl:''}
            data:[],
            autorefresh: true,
            startupload: '开始上传',
            continueupload: '继续上传'
            //uniqueId
            //refreshurl:
            //onBeforeLoad:
            //onBeforeRemove:
            //onRemoveCallback(data):删除后返回值的处理程序
            //onPreview:
        };

        var option = $.extend(option, def);

        var div = $(this);
        fetch();
        
        function fetch(){
            if (option.autorefresh && option.refreshurl) {
                var param = {},
                    callback = function (data) {
                        option.data = data;
                    };
                if (option.refreshparam) {
                    param = option.refreshparam;
                }
                if (option.onBeforeLoad && typeof option.onBeforeLoad == 'function') {
                    option.onBeforeLoad(param);
                }
                $.ajax({
                    type: 'POST',
                    url: option.refreshurl,
                    data: param,
                    async: false,
                    success: callback,
                    dataType: 'json'
                });
            }
            render(option.data);
            initevents();
        }


        function render(data) {
            if (data != undefined){
                var length = data.length;
                var formstr = '<div class="file-box"><form action="' + option.uploadurl + '" method="post" enctype="multipart/form-data"><span class="file-literal">' + (length > 0 ? option.continueupload : option.startupload)
                    + '</span><input type="file" name="fileField" class="file" id="' + option.uniqueId + '" size="28"/></form></div>';
                if (option.readonly)
                    var formstr = '';
                var html = [];
                html.push('<table class="attpool-table">');
                var trs = [];
                var count = 0;
                if (length == 0) {
                    trs.push('<tr><td>' + formstr + '</td><td></td></tr>');
                }
                $.each(data, function (index, r) {
                    count++;
                    if (count == 1) {
                        //trs.push('<tr id="' + r.id + '" rolspan="' + length + '">' + '<td>' + formstr + '</td><td>');
                        trs.push('<tr rolspan="' + length + '">' + '<td>' + formstr + '</td><td>');
                    }
                    else {
                        //trs.push('<tr id="' + r.id + '"><td></td><td>');
                        trs.push('<tr><td></td><td>');
                    }
                    trs.push(r.name);
                    trs.push('</td>');
                    if (r.downloadurl) {
                        trs.push('<td class="attpool-downlaod"><a href="#">下载</a></td>');
                    }
                    else {
                        trs.push('<td class="attpool-downlaod"><a href="#" style="display:none"></a></td>');
                    }
                    if (r.preview) {
                        trs.push('<td class="attpool-preview"><a href="#">预览</a></td>');
                    }
                    else {
                        trs.push('<td class="attpool-preview"><a href="#" style="display:none"></a></td>');
                    }
                    if (r.removeurl) {
                        trs.push('<td class="attpool-remove"><a href="#">删除</a></td>');
                    }
                    else {
                        trs.push('<td class="attpool-remove"><a href="#" style="display:none"></a></td>');
                    }
                    trs.push('</tr>');
                });
                html.push(trs.join(''));
                div.html(html.join(''));
            }
        }

        function initevents() {
            div.find(".attpool-downlaod a").each(function (index) {
                $(this).click(function () {
                    window.open(option.data[index].downloadurl);
                });
            });
            if (option.onPreview) {
               div.find(".attpool-preview a").each(function (index) {
                $(this).click(function () {
                    option.onPreview(option.data[index]);
                    });
                }); 
            }
            
            div.find(".attpool-remove a").each(function (index) {

                $(this).click(function () {
                    var param = {};
                    var onRemoveCallback = function () { };
                    if (option.onRemoveCallback && typeof option.onRemoveCallback == 'function') {
                        onRemoveCallback = option.onRemoveCallback;
                    }
                    if (option.onBeforeRemove && typeof option.onBeforeRemove == 'function') {
                        if (option.onBeforeRemove(option.data[index], param) == true) {
                            doRemove(option.data[index], param, onRemoveCallback);
                        }
                    }
                    else {
                        doRemove(option.data[index], param, onRemoveCallback);
                    }

                });

                //$(this).click(function () {
                //    var d = option.onDelete($(this).parents().parents().attr('id'));
                //    if (d == true)
                //    {
                //        if (option.autorefresh && option.refreshurl) {
                //            $.post(option.refreshurl, {}, function (data) {
                //                option.data = data;
                //                div.attpool(option);
                //            });
                //        }
                //    }
                //});
            });
            div.find('#' + option.uniqueId).change(function () {
                $.ajaxFileUpload({
                    url: option.uploadurl,
                    secureuri: false,
                    fileElementId: option.uniqueId,
                    dataType: 'json',
                    beforeSend: function () {
                        //$("#loading").show();
                    },
                    complete: function () {
                        if (option.onCompeleted != undefined && typeof (option.onCompeleted) == "function"){
                            option.onCompeleted();
                        }
                    },
                    success: function (data, status) {
                        if (option.onSuccessed != undefined && typeof (option.onSuccessed) == "function") {
                            option.onSuccessed(data, status);
                        }
                        if (option.autorefresh && option.refreshurl) {
                            //$.post(option.refreshurl, {}, function (data) {
                            //    option.data = data;
                            //    div.attpool(option);
                            //});
                            fetch();
                        }
                    },
                    error: function (data, status, e) {
                        if (option.onError != undefined && typeof (option.onError) == "function") {
                            option.onError(data, status, e);
                        }
                    }
                });
                return false;
            });
        }

        function doRemove(data_row, param, callback){
            $.ajax({
                type: 'POST',
                url: data_row.removeurl,
                data: param,
                async: false,
                success: callback,
                dataType: 'json'
            });
            fetch();
        }

        return this;
    }
})($);