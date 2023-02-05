<html lang="en">
<head>
   <meta http-equiv="Content-Type" content="text/html; charset=UTF-8" />
   <title>Exercise 13-1 Creating Classes</title>   
   
   <!-- Latest compiled and minified Bootstrap Core CSS -->
   <link href="bootstrap/css/bootstrap.min.css" rel="stylesheet">
</head>
<body>

<header>
<h1>Weather forecast using classes</h1>
</header>

<div class='container'>
<?php 

ini_set("display_errors",1);
date_default_timezone_set('GMT');

include_once("Forecast.class.php");



$forecast[] = $dayOne; 
$today = time();
$oneday = 60*60*24;

$forecast = array (new Forecast (date("d M, Y", $today),30,20,"sunny"),

$dayOne = new Forecast (date("d M, Y", $today),30,20,"sunny");
echo $dayOne;

$daytwo	= new Forecast (date("d M, Y", $today),30,20,"sunny");
echo $daytwo;

$daythree	= new Forecast (date("d M, Y", $today),30,20,"sunny");
echo $daythree;

$dayfour	= new Forecast (date("d M, Y", $today),30,20,"sunny");
echo $dayfour;

$dayfive	= new Forecast (date("d M, Y", $today),30,20,"sunny");
echo $dayfive;

$daysix	= new Forecast (date("d M, Y", $today),30,20,"sunny");
echo $daysix;

$dayseven	= new Forecast (date("d M, Y", $today),30,20,"sunny");
echo $dayseven;

);

foreach($forecast as $oneDay){
   echo $oneDay;
}

?>
</div>

<footer>
  <h3>Record High: <?php echo Forecast::$allTimeHigh; ?></h3>
  <h3>Record Low: <?php echo Forecast::$allTimeLow; ?></h3>
</footer>

</body>
</html>
