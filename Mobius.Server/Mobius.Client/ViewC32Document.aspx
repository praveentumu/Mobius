<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ViewC32Document.aspx.cs"
    Inherits="ViewC32Document" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>C32 Document</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table id="tblAttachments" runat="server">
            <tr>
                <td>
                    There are attachments available
                    <asp:LinkButton ID="lnkViewAttachments" runat="server" Text="click here"></asp:LinkButton>
                    to view.
                </td>
            </tr>
        </table>
        <asp:Xml ID="xmlViewC32Document" runat="server"></asp:Xml>
    </div>
    </form>
</body>
</html>

<script language="javascript" type="text/javascript">

    function OpenAttachmentWindow(SharedDocumentId) {        
        retVal = window.showModalDialog("ViewAttachments.aspx?SharedDocumentId=" + SharedDocumentId, "Attachments", "dialogWidth=550px;dialogHeight=200px");

        if (retVal != null) {

        }
    }
</script>

