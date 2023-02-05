<?php
 // get the form data by grabing the names of all the form fields and then outputing them             
 $fp = fopen('LogData.txt', 'a');

//name
if(isset($_POST['name']))
{
$data=$_POST['name'];
$fp = fopen('LogData.txt', 'a');
fwrite($fp, $data, FILE_APPEND);
}


//descritpion
if(isset($_POST['name']))
{
$data1=$_POST['name'];
$fp1 = fopen('LogData.txt', 'a');
fwrite($fp1, $data1, FILE_APPEND);
}

// year
if(isset($_POST['year']))
{
$data2 = $_POST['year'];
$fp2 = fopen('LogData.txt', 'a');
fwrite($fp2, $data2, FILE_APPEND);
}

//medium
if(isset($_POST['medium']))
{
$data3 =$_POST['name'];
$fp3 = fopen('LogData.txt', 'a');
fwrite($fp3, $data3, FILE_APPEND);
}

//email
if(isset($_POST['email']))
{
$data4=$_POST['email'];
$fp4 = fopen('LogData.txt', 'a');
fwrite($fp4, $data4, FILE_APPEND);
}

// Genre
if(isset($_POST['genre']))
{
$data5=$_POST['genre'];
$fp5 = fopen('LogData.txt', 'a');
fwrite($fp5, $data5, FILE_APPEND);
}
// subject
if(isset($_POST['subject']))
{
$data6 =$_POST['subject'];
$fp6 = fopen('LogData.txt', 'a');
fwrite($fp6, $data6, FILE_APPEND);
}
// type
if(isset($_POST['type']))
{
$data7 =$_POST['type'];
$fp7 = fopen('LogData.txt', 'a');
fwrite($fp7, $data7, FILE_APPEND);
}
// creative commons
if(isset($_POST['cc']))
{
$data8 =$_POST['cc'];
$fp8 = fopen('LogData.txt', 'a');
fwrite($fp8, $data8, FILE_APPEND);
}

fwrite($fp, "Updated on -" . date("Y/m/d") . "<br>"); 
fclose($fp);
?>