
var dragapproved = false
var minrestore = 0
var initialwidth
var initialheight
var ie5 = document.all && document.getElementById
var ns6 = document.getElementById && !document.all
var IsRefreshNeeds = false;

var xxx = 0; var yyy = 0;
var endPos = 0;
var posTop = 0;

function drag_drop(e) {
    if (ie5 && dragapproved && event.button == 1) {
        document.getElementById("divWindow").style.left = tempx + event.clientX - offsetx
        document.getElementById("divWindow").style.top = tempy + event.clientY - offsety
    }
    else if (ns6 && dragapproved) {
        tempx = 300;
        document.getElementById("divWindow").style.left = tempx + e.clientX - offsetx
        document.getElementById("divWindow").style.top = tempy + e.clientY - offsety
    }
}
function initializedrag(e) {
    offsetx = ie5 ? event.clientX : e.clientX
    offsety = ie5 ? event.clientY : e.clientY
    if (ie5)
        tempx = parseInt(document.getElementById("divWindow").style.left)
    tempy = parseInt(document.getElementById("divWindow").style.top)
    dragapproved = true
    document.onmousemove = drag_drop
}


/*
*  This Will close the window Div window
*/
function CloseMSWindow() {
    CloseMSWindow('');
}

function CloseMSWindow(PageName) {
    if (IsRefreshNeeds && PageName != '')
        top.location.href = PageName;
    else {
        top.document.getElementById('divWindow').style.display = 'none';
        LightenMasterPage();
    }
    return false;
}

function CloseDivWindow() {
    document.getElementById('divWindow').style.display = 'none';
    LightenMasterPage();
}

if (ie5 || ns6)
    document.onmouseup = new Function("dragapproved=false;document.onmousemove=null")


var closeWindow = false

function openpopupInFullScreen(Src, Title) {

    if (closeWindow)
        CloseMSWindow();
    DarkenMasterPage();
    IsRefreshNeeds = false

    w = width = parseInt($(document).width()) - 120;
    h = height = parseInt($(document).height());
    height = ($(window).height() * 90) / 100


    ContentHTML = "<div id='POPUP' class='msDivPopUp' ><table border='0' class='msPopupTable' width='" + width + "' height='" + height + "' cellpadding='0'	cellspacing='0' ><tr><td class='msPopupTitle'>" + Title + "</td><td  class='msPopupTitle'><img  src='images/close.gif' class='msCloseButton' alt='Close' onMouseOver=style.cursor='hand' onMouseOver=style.cursor='auto' align='right' onClick=CloseDivWindow()></td></tr><tr><td colspan=2><iframe name='srcFile' src='" + Src + "' width='" + width + "' height='" + height + "' allowtransparency='true' hspace='0' vspace='0' scrolling='auto' marginheight='0' marginwidth='0' frameborder='0'></iframe></td></tr></table></div>"

    //set the dhtml popup in center of visible screen
    dwincontent = document.getElementById('divWindow');
    dwincontent.innerHTML = ContentHTML;
    dwincontent.style.display = 'block';


    w = parseInt(width);
    h = parseInt(height);

    dwincontent.style.left = Math.round(($(window).width() / 2) - (w / 2)) + "px";
    dwincontent.style.top = "10px"



    scroll(0, 0);
    closeWindow = true;

}


function open_popup(Left, Top, Src, Title, Width, Height, IsRefreshNeeded) {

    if (closeWindow)
        CloseMSWindow();
    if (Width == null) {
        var width = 500;
    }
    else {
        var width = Width;
    }
    if (Height == null) {
        var height = 350;
    }
    else {
        var height = Height;
    }

    DarkenMasterPage();

    IsRefreshNeeds = IsRefreshNeeded

    ContentHTML = "<div id='POPUP' class='msDivPopUp' ><table border='0' class='msPopupTable' width='" + width + "' height='" + height + "' cellpadding='0'	cellspacing='0' ><tr><td class='msPopupTitle'>" + Title + "</td><td  class='msPopupTitle'><img  src='images/close.gif' class='msCloseButton' alt='Close' onMouseOver=style.cursor='hand' onMouseOver=style.cursor='auto' align='right' onClick=CloseDivWindow()></td></tr><tr><td height='0' class='yline' colspan='2'></td></tr><tr><td colspan=2><iframe name='srcFile' src='" + Src + "' width='" + width + "' height='" + height + "' allowtransparency='true' hspace='0' vspace='0' scrolling='auto' marginheight='0' marginwidth='0' frameborder='0'></iframe></td></tr></table></div>"

    //set the dhtml popup in center of visible screen
    dwincontent = document.getElementById('divWindow');
    dwincontent.innerHTML = ContentHTML;
    dwincontent.style.display = 'block';

    w = parseInt(Width);
    h = parseInt(Height);


    if (Left.toUpperCase() == "CENTER")
        dwincontent.style.left = Math.round(($(window).width() / 2) - (w / 2)) + "px";
    else
        dwincontent.style.left = Left + "px"; ;
    if (Top.toUpperCase() == "CENTER")
        dwincontent.style.top = Math.round(($(window).height() / 2) - (h / 2) - 125) + "px"
    else
        dwincontent.style.top = Top + "px";

    scroll(0, 0);
    closeWindow = true;
}

function hideLoader() {
    document.getElementById('IFrame').style.display = 'block';
    document.getElementById('Loader').style.display = 'none';
}

// this function removes the dark screen and the page is light again
function LightenMasterPage() {
    var page_screen = top.window.document.getElementById('page_screen');

    if ((document.getElementById('divWindow') == null || document.getElementById('divWindow').style.display == 'none' || document.getElementById('divWindow').style.display == ''))
        page_screen.style.display = 'none';
}
// This will darken the base page 
function DarkenMasterPage() {
    var page_screen = document.getElementById('page_screen');
    if (page_screen) {
        page_screen.style.height = $(window).height() + 'px';
        page_screen.style.display = 'block';
    }
}


function CloseError() {
    document.getElementById('divErrorMessage').style.display = 'none'
    LightenMasterPage();

}


function onError(Title) {

    open_popup('CENTER', 'CENTER', 'Error.aspx', Title, 300, 200, false);
}

//     function moveit() 
//     {        
//        var y = (posTop+ yyy);      
//        if(posTop < y)        
//            posTop = y;
//         else
//             posTop = posTop - y;
//                         
//         document.getElementById('divWindow').style.top= posTop;
//    }
