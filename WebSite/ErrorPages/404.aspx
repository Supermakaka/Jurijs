<% Response.StatusCode = 404 %>

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
            <h1>Oops, 404!</h1>
            <p class="lead">The page you are looking for could not be found.</p>
            <a href="/" class="btn btn-large btn-primary"><i class="glyphicon glyphicon-home"></i> Take me Home</a>
        </div>
    </div>
</body>
</html>
