<?php 
// used to display the message after two days 
if(isset($_COOKIE['visit']) && $_COOKIE['visit'] == "true"){
    echo 'cookie set, welcome back';
}else{
    setcookie("visit", "true", time()+(3600*24*2));
    echo 'cookie not set, welcome new user';
}
?>
