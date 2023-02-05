<?php
//write to file
$text = file_get_contents( poly_log );
//echo "<h3>$text<h3>";
echo file_put_contents( 'poly-copy.txt', $text, FILE_APPEND );

$nintendo = file_get_contents( 'https://www.nintendo.com' );

if( strpos( $nintendo, 'marioparty' ) ) {
    echo "<p>This website has Mario Party!</p>"; }
    else {
      echo "<p> Mario Party not available</p>";
    }

?>
