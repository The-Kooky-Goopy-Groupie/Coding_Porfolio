// Create this in index.php
// This is login form, data uses POST method
<!DOCTYPE html>
<html>
<body>

<h1>Some page that has a login form</h1>
<form action="processLogin.php" method="POST">
    Name <input type="text" name="uname"><br>
    Pass <input type="password" name="pass"><br>
    <input type="submit">
</form>
</body>
</html>

// Create this in processLogin.php
// This processes the form
<?php
if ($_SERVER["REQUEST_METHOD"] == "POST") {
    if ( isset($_POST["uname"]) && isset($_POST["pass"]) ) {
        // handle the posted data.
        echo "handling user login now ...";
        echo "... here we could redirect or authenticate ";
        echo " and hide login form or something else. <br>";
        echo "user name is ".$_POST["uname"] . "<br>";
        echo "password is ".$_POST["pass"];
    } else {
        echo "nevermind.";
    }
}
?>

<?php
//null coalescing operator
if ($_SERVER["REQUEST_METHOD"] == "GET") {
    $username = $_GET['uname'] ?? 'nobody';
    echo "user name is ". $username . "<br>";

}
?>

//Checkbox arrays with GET
<!DOCTYPE html>
<html>
<body>

<h1>CHECKBOX FORM</h1>
<form method="get">
    Please select days of the week you are free.<br>
    Monday <input type="checkbox" name="day[]" value="Monday"> <br>
    Tuesday <input type="checkbox" name="day[]" value="Tuesday"> <br>
    Wednesday <input type="checkbox" name="day[]" value="Wednesday"> <br>
    Thursday <input type="checkbox" name="day[]" value="Thursday"> <br>
    Friday <input type="checkbox" name="day[]" value="Friday"> <br>
    <input type="submit" value="Submit">
</form>
</body>
</html>
<?php
echo "You submitted " . count($_GET['day']) . " values<br>";
foreach  ($_GET['day']  as  $d) {
    echo $d . " <br>";
}
?>
