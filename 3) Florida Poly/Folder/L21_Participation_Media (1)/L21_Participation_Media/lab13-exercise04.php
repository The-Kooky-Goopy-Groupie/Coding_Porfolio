<html lang="en">
<head>

<!-- Latest compiled and minified Bootstrap Core CSS -->
<link rel="stylesheet" href="//netdna.bootstrapcdn.com/bootstrap/3.1.1/css/bootstrap.min.css">
<!-- Bootstrap theme -->
<link href="../../dist/css/bootstrap-theme.min.css" rel="stylesheet">

   <meta http-equiv="Content-Type" content="text/html; charset=UTF-8" />
   <title>Exercise 10-4 | Data Encapsulation</title>
</head>
<body>

<header>
<h1>Weather forecast using classes and encapsulation</h1>
</header>

<div class='container'>
<?php 

ini_set("display_errors",1);
date_default_timezone_set('GMT');
include_once("Forecastv3.class.php");

$today = time();
$oneday = 60*60*24;

//replace forecast = array section
$forecast = array (new Forecast (date("d M, Y", $today),30,20,"sunny"),
   new Forecast (date("d M, Y", $today+$oneday),35, 22, "clear and sunnny"),
   new Forecast (date("d M, Y", $today+2*$oneday),20, 10, "cold and snow"),
   new Forecast (date("d M, Y", $today+3*$oneday),25, 14, "chilly"),
   new Forecast (date("d M, Y", $today+4*$oneday),30, 24, "sunny"),
   new Forecast (date("d M, Y", $today+5*$oneday),40, 26, "warming"),
   new Forecast (date("d M, Y", $today+6*$oneday),50, 34, "hot")
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
