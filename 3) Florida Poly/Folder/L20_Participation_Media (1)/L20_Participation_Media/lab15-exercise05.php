<html lang="en">
<head>
<title>Exercise 15.5 - Error Handlers</title>
<link rel="stylesheet" href="//netdna.bootstrapcdn.com/bootstrap/3.1.1/css/bootstrap.min.css">
</head>
<body>
<?php

function validateEmail($email) {
  $pattern = '/^[\-0-9a-zA-Z\.\+_]+@[\-0-9a-zA-Z\.\+_]+\.[a-zA-Z\.]{2,5}$/';
  if ( preg_match($pattern, $email) ) {
    return true;
  }
  return false;
}


//add validate email function here

//add validate name
//add validate  phone

// Add echocss function here

if (isset($_POST['name'])) { //form was posted (or link with query string was clicked).
   echo "form posted";
   echo "<pre>";
   print_r($_POST);
   echo "</pre>";
}

?>
<div class='container'>
<h2>Form to work with</h2>
<form name='mainForm' id='mainForm' method='post'>
	<div class="form-group  ">
    <legend>Required Information:</legend>
      <label for="name">Name</label>
      <input name="name" id="name" type="text" class="form-control" 
	placeholder="Enter full name"
	 />
  </div>
  <div class='form-group  '>
      <label for="email">Email</label>
      <input name="email" id="email" type="text" class="form-control"
	 />
  </div>
  <div class='form-group  '>
      <label for="phone">Phone</label>
      <input name="phone" id="phone" type="text" class="form-control" 
	placeholder="(xxx) xxx-xxxx"
	 />
  </div>
  <div class="form-group">
        <legend>Optional Information:</legend>
      <label for="how">How did you hear about us?</label>
      <input name="how" id="how" type="text" size="30" class="form-control" 
	 />
  </div>
  <input type="submit" value="Submit Form" class="form-control" />
</form>
</div>
</body>
