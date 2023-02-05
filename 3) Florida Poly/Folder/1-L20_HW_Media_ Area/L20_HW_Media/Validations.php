<?php
// put these here like in the participation assignement
function validateEmail($email) {
  $pattern = '/^[\-0-9a-zA-Z\.\+_]+@[\-0-9a-zA-Z\.\+_]+\.[a-zA-Z\.]{2,5}$/';
  if ( preg_match($pattern, $email) ) {
    return true;
  }
  return false;
}

// medium - Aplpha charcaters are only ones allowed
function validateMedium($medium) {
  $pattern = '/[^a-zA-Z0-9]+/'; // should now only take the lettere characters and 0-9 nums ( no symbols, alphas only )
  if ( preg_match($pattern, $medium) ) {
    return true;
  }
  return false;
}


// year - only 4 number characters 
function validateYear($year) {
  $pattern = '/d{4}$/'; // should now only take the 0-9  and only up to 4 of them
 // $pattern =  "/^\(?\s*\d{3}\s*[\)-\.]?\s*[2-9]\d{2}\s*[-\.]\s*\d{4}$/";
  if ( preg_match($pattern, $year) ) {
    return true;
  }
  return false;
}


// used to check all the other fields are valid 
function echoCssError($name) {
    if ($_SERVER['REQUEST_METHOD']=="GET")
    return;  

    if ($name=='email' &&	!validateEmail($_POST['email'])) {
       echo "has-error email";
    }

    if ($name=='medium' &&	!validateMedium($_POST['medium'])) {
        echo "has-error medium";
     } 
     
     
     if ($name=='year' &&	!validateYear($_POST['year'])) {
        echo "has-error year";
     }
 }
 



 if (isset($_POST['name'])) { //form was posted (or link with query string was clicked).
    echo "form posted";
    echo "<pre>";
    print_r($_POST);
    echo "</pre>";
 }





?>


 <?php
//write to file
$text = file_get_contents($text);


echo "<h3>$text<h3>";

echo file_put_contents( 'poly-copy.txt', $text, FILE_APPEND );
?>