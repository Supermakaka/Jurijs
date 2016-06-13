<%@ Page validateRequest="false" %>
<!-- validateRequest="false" is needed for this page to work in case of "A potentially dangerous Request.Path value was detected from the client" exception -->

<% Response.StatusCode = 500 %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>

    <link href="/Content/bootstrap.css" rel="stylesheet"/>
    <link href="/Content/style.css" rel="stylesheet"/>

</head>
<style>
    .error {
        text-align: center;
        margin-top: 200px;
    }

        .error h1 {
            font-size: 90px;
            font-weight: bold;
        }

        .error .btn {
            margin-top: 30px;
        }
</style>

<body>
    <div class="container">
        <div class="error">
            <h1>Oops, 500!</h1>
            <p class="lead">Looks like something went wrong!</p>
            <p>If the problem persists, feel free to contact us. In the meantime, try refreshing.</p>
            <a href="/" class="btn btn-large btn-primary"><i class="glyphicon glyphicon-home"></i> Take me Home</a>
        </div>
    </div>
</body>
</html>