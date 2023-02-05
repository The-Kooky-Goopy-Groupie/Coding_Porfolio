<html lang="en">
<head>
<title>Exercise 15.4 - Error Handlers</title>
<link rel="stylesheet" href="//netdna.bootstrapcdn.com/bootstrap/3.1.1/css/bootstrap.min.css">
</head>
<body>
<?php
//Add error handler here

//Add math expression here

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
  <div class="form-group">
    <legend>Required Information:</legend>
      <label for="name">Name</label>
      <input name="name" id="name" type="text" class="form-control" />
  </div>
  <div class='form-group'>
      <label for="email">Email</label>
      <input name="email" id="email" type="text" class="form-control" />
  </div>
  <div class='form-group'>
      <label for="phone">Phone</label>
      <input name="phone" id="phone" type="text" class="form-control" />
  </div>
  <div class="form-group">
        <legend>Optional Information:</legend>
      <label for="how">How did you hear about us?</label>
      <input name="how" id="how" type="text" size="30" class="form-control" />
  </div>
  <input type="submit" value="Submit Form" class="form-control" />
</form>
</div>
</body>
