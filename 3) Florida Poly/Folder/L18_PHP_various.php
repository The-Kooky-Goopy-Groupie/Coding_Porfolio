//PHP Tag Example
<?php
    $user = "Randy";
    ?>
<!DOCTYPE html>
<html>
    <body>
    <h1>Welcome<?php echo $user; ?></h1>
    <p>
    The server time is <?php
    echo "<strong>";
    echo date("H:i:s");
    echo "</strong>";
    ?>
    </p>
    </body>
</html>

//PHP variable types
<?php
$a = 42;
$b = "days left";
echo "There are ". $a . $b;
?>


<!--// PHP constants-->
<?php
// uppercase for constants is a programming convention
define("DATABASE_LOCAL", "localhost");
define("DATABASE_NAME", "ArtStore");
define("DATABASE_USER", "Fred");
define("DATABASE_PASSWD", "F5^7%ad");
// notice that no $ prefaces constant names
echo "<br>" . DATABASE_LOCAL ;
echo "<br>" . DATABASE_NAME ;
?>

//Writing to Output with echo
<?php
echo  "<br>";
echo("hello");
echo  "<br>";
echo "hello";
echo  "<br>";
$username = "Ricardo";
echo "Hello " . $username;
echo  "<br>";
?>
//variable variable
<?php
$pokemon1 = "Bulbasaur";
$pokemon2 = "Charmander";
$pokemon3 = "Squirtle";
$pokemon4 = "Caterpie";
$pokemon5 = "Pidgey";
for ($i = 1; $i <= 5; $i++) {
    echo ${"pokemon". $i};
    echo "<br>";
}
?>

// html with string
<?php
$firstName = "Logan";
$lastName = "Wolverine";
echo "<em> $firstName $lastName </em>";
?>

//echo alternative
<?php
$url = "http://www.funwebdev.com";
$file = "https://assets.pokemon.com/assets/cms2/img/watch-pokemon-tv/seasons/season14/season14_ep07_ss02.jpg";
?>
...
<br>
<a href='<?= $url ?>'>
    <img src='<?= $file ?>' alt='snivey'>
</a>

//if else
<?php
$x = 5;
echo "<br>";
if ($x < 10)
{echo "<br> $x is less than 10";}
else {echo "<br> 5 is not less than 10";
}
?>

//swich statement
<?php
$code = x;
echo "<br>";
switch($code) {
    case "PT":
        echo "Painting";
        break;
    case 1:
        echo "Sculpture";
        break;
    default:
        echo "Other";
}
?>

//switch statement
<?php
$code = x;
echo "<br>";
switch($code) {
    case "PT":
        echo "Painting";
        break;
    case 1:
        echo "Sculpture";
        break;
    default:
        echo "Other";
}
?>

//Alternate syntax for control structure
<?php if ($userStatus == "loggedin") : ?>
    <br>
    <a href="account.php">Account</a>
    <a href="logout.php">Logout</a>
<?php else : ?>
    <br>
    <a href="login.php">Login</a>
    <a href="register.php">Register</a>
<?php endif; ?>

# //create and invoke function
<?php
/**
 * This function returns a nicely formatted string using the current
 * system time.
 */
echo "<br>";
function getNiceTime($showSeconds){
    if ($showSeconds==true)
        return date("H:i:s");
    else
        return date ("H:i");
}
echo getNiceTime(false);
$output = getNiceTime(true);
echo "<br>" . $output . " is the time";
?>

<?php
//  Pass by reference
function changeParameter(&$arg) {
    $arg += 300;
    echo "<br/>arg=". $arg;
}
echo "<br>";
$initial = 15;
echo "<br/>initial=" . $initial;   // output: initial=15
changeParameter($initial);         // output: arg=315
echo "<br/>initial=" . $initial;   // output: initial=315
?>

<?php
// Parameter declaration with default
function getNiceTime2(bool  $showSeconds=true) {
    if ($showSeconds==true)
        return date("H:i:s");
    else
        return date("H:i");
}
echo "<br>";
echo getNiceTime2();
?>

<?php
// test scope
echo "<br>";
$count = 56;

function testScope() {
    // global $count;
    echo "This wont display the var $count";
    //echo "This wont display the var ", $GLOBALS['count'];
    echo "<br>";   // outputs 0 or generates run-time warning
}
testScope();
echo "This will display the var $count";        // outputs 56
?>

<?php
// defining arrays
$days = array("Mon","Tue","Wed","Thu","Fri");
$daysx = ["Mon","Tue","Wed","Thu","Fri"];     // alternate syntax
echo "Value at index 1 is ". $days[1];
echo "<br>";  // index starts at zero
echo "Value at index 1 is ". $daysx[1];

echo "<br>";
$forecast = array("Mon" => 40, "Tue" => 47, "Wed" => 52, "Thu" => 99);
echo $forecast["Wed"]; //associative array
?>


<?php
// multidimensional arrays
$month = array(
    array("Mon","Tue","Wed","Thu","Fri"),
    array("Mon","Tue","Wed","Thu","Fri"),
    array("Mon","Tue","Wed","Thu","Fri"),
    array("Mon","Tue","Wed","Thu","Fri")
);
echo $month[0][3]; // outputs Thu
echo "<br>";

$cart = [];
$cart[] = array("id" => 37, "title" => "Burial at Ornans",
    "quantity" => 1);
$cart[] = array("id" => 345, "title" => "The Death of Marat",
    "quantity" => 1);
$cart[] = array("id" => 63, "title" => "Starry Night", "quantity" => 1);
echo $cart[2]["title"];   // outputs Starry Night
echo "<br>";

$stocks = [
    ["AMZN", "Amazon"],
    ["APPL", "Apple"],
    ["MSFT", "Microsoft"]
];
echo $stocks[2][1];    // outputs Microsoft
echo "<br>";

$aa = [
    "AMZN" => ["Amazon", 234],
    "APPL" => ["Apple", 342],
    "MSFT" => ["Microsoft", 165]
];
echo $aa["APPL"][0];   // outputs Apple
echo "<br>";

$bb = [
    "AMZN" => ["name" =>"Amazon", "price" => 234],
    "APPL" => ["name" => "Apple", "price" => 342],
    "MSFT" => ["name" => "Microsoft", "price" => 165]
];
echo $bb["MSFT"]["price"];  // outputs 165
echo "<br>";
?>

<?php
// iterating through arrays
$days = array("Mon","Tue","Wed","Thu","Fri");
// while loop
$i=0;
while  ($i < count($days)) {
    echo $days[$i] . "<br>";
    $i++;
}
// do while loop
$i=0;
do  {
    echo $days[$i] . "<br>";
    $i++;
}  while  ($i < count($days));

// for loop
for  ($i=0; $i<count($days); $i++) {
    echo $days[$i] . "<br>";
}


echo "<br>";

?>

<?php
// iterating through arrays
$days = array("Mon","Tue","Wed","Thu","Fri");
// while loop
$i=0;
while  ($i < count($days)) {
    echo "while loop :" . $days[$i] . "<br>";
    $i++;
}
// do while loop
$i=0;
do  {
    echo "do while loop :" . $days[$i] . "<br>";
    $i++;
}  while  ($i < count($days));

// for loop
for  ($i=0; $i<count($days); $i++) {
    echo "for loop :" . $days[$i] . "<br>";
}
echo "<br>";
?>

<?php
// iterating associative array foreach
$forecast = array("Mon" => 40, "Tue" => 47, "Wed" => 52, "Thu" => 99, "Fri" => 85);
// foreach: iterating through the values
foreach  ($forecast  as $value) {
    echo $value . "<br>";
}
echo "<br>";
// foreach: iterating through the values AND the keys
foreach  ($forecast  as $key => $value) {
    echo "day[" . $key . "]=" . $value;
    echo "<br>";
}
?>

<?php
// print_r array
$days = array("Mon","Tue","Wed","Thu","Fri");
print_r($days);
echo "<br>";
$forecast = array("Mon" => 40, "Tue" => 47, "Wed" => 52, "Thu" => 99, "Fri" => 85);
print_r($forecast);
?>

<?php
// 2d array select list
$stocks = [ ["AMZ", "Amazon"],
    ["APPL", "Apple"],
    ["MSFT", "Microsoft"] ];

$stocks2 = [
    "AMZN" => ["name" =>"Amazon", "price" => 234],
    "APPL" => ["name" => "Apple", "price" => 342],
    "MSFT" => ["name" => "Microsoft", "price" => 165]
];
?>

<label for "selectbox">Select box</label>
<select name="selectbox">
    <?php  // $array as $value on foreach, increments by +1
    foreach ($stocks as $s) {
        echo "<option value='$s[0]'>$s[1]</option>";
    }
    ?>
</select>


<br>
<label for "selectbox2">Select box2</label>
<select name="selectbo2x">
    <?php  // 2d associative array
    // $array as $value on foreach, increments by +1
    foreach ($stocks2 as $key => $value) {
        echo "<option value='$key'>" . $value["name"] . "</option>";
    }
    ?>
</select>

<?php
// adding and deleting in arrays
$days = array("Mon","Tue","Wed","Thu","Fri");
$days[7] = "Sat"; //results in skipping an index
print_r($days);
echo "<br>";
unset($days[7]); //this removes the incorrect index
echo "<p>This removes the element</p>";
print_r($days);

?>
<?php
// adding to arrays
echo "<p> Now to add index to end of array";
echo "<br>";
$days = array("Mon","Tue","Wed","Thu","Fri");
$days[] = "Sat"; //results in correctly adding index
print_r($days);

echo "<br>";
//test true if element is set
if (isset($days[5])) {
    echo "<p>The 5th element is $days[5]";
}
?>

<?php
//Properties of a class
class Artist {
    public   $firstName;
    public   $lastName;
    public   $birthDate;
    public   $birthCity;
    public   $deathDate;
}
//instantiate two new objects/instances with new keyword
$picasso = new Artist();
$dali = new Artist();

//add properties
$picasso->firstName = "Pablo";
$picasso->lastName = "Picasso";
$picasso->birthCity = "Malaga";
$picasso->birthDate = "October 25 1881";
$picasso->deathDate = "April 8 1973";
echo $picasso->{'firstName'};
?>

<?php
// Class with constructor
class Artist {
    // variables from previous listing still go here
    public   $firstName;
    public   $lastName;
    public   $birthDate;
    public   $birthCity;
    public   $deathDate;

    function  __construct($firstName, $lastName, $city, $birth, $death=null) {
        $this->firstName = $firstName;
        $this->lastName = $lastName;
        $this->birthCity = $city;
        $this->birthDate = $birth;
        $this->deathDate = $death;
    }
}
$picasso = new Artist("Pablo","Picasso","Malaga","Oct 25,1881",
    "Apr 8,1973");
$dali = new Artist("Salvador","Dali","Figures","May 11 1904",
    "Jan 23 1989");
echo $picasso->{'firstName'};

?>

<?php
// Class with methods
class Artist {
    // variables from previous listing still go here
    public   $firstName;
    public   $lastName;
    public   $birthDate;
    public   $birthCity;
    public   $deathDate;

    function  __construct($firstName, $lastName, $city, $birth, $death=null) {
        $this->firstName = $firstName;
        $this->lastName = $lastName;
        $this->birthCity = $city;
        $this->birthDate = $birth;
        $this->deathDate = $death;
    }
    public function outputAsTable()  {
        $table = "<table>";
        $table .= "<tr><th colspan='2'>";
        $table .= $this->firstName . " " . $this->lastName;
        $table .= "</th></tr>";
        $table .= "<tr><td>Birth:</td>";
        $table .= "<td>" . $this->birthDate;
        $table .= "(" . $this->birthCity . ")</td></tr>";
        $table .= "<tr><td>Death:</td>";
        $table .= "<td>" . $this->deathDate . "</td></tr>";
        $table .= "</table>";
        return $table;
    }
}
$picasso = new Artist("Pablo","Picasso","Malaga","Oct 25,1881",
    "Apr 8,1973");
$dali = new Artist("Salvador","Dali","Figures","May 11 1904",
    "Jan 23 1989");
echo $picasso->{'firstName'};
echo $picasso->outputAsTable();

?>

