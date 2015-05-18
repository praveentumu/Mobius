<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ViewFile.aspx.cs" Inherits="ViewConfigFile" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <script type="text/javascript" language="javascript">
        function CloseAndRedirect(redirectionLink) {
            window.opener.location = redirectionLink;
            window.close();
        }
    </script>

    <title></title>
</head>
<body style="background-color: White">
    <form id="form1" runat="server" style="border: none; height: 100%; width: 100%;">
    <div>
        <div id="xmlViewConfig" runat="server" style="border: none; height: 100%; width: 100%;">
            <textarea style="border: none; height: 4500px; width: 100%; overflow: hidden;" id="txtAreaConfig"
                name="txtAreaConfig" runat="server">
             </textarea>
        </div>
    </div>
    </form>
</body>
</html>
