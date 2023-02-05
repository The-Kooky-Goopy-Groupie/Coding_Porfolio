<!DOCTYPE html>
<html>
<!--WARNING - we still have absolutely no idea how PHP works, we also thought we had to the end of sunday to do this, but be prepared for things being very clunky in this file-->
<!-- ANOTHER NOTE: functionality was tested inside of replit and then moved here, if using different software for testing try replit as well P.S. that's still a very annoying thing-->
<head lang="en">
   <meta charset="utf-8">
   <title>Lark Submission Form</title>
   <link rel="stylesheet" href="css/styles.css" />
   <script type="text/javascript" src="js/lark-form.js"></script>
</head>
<body>
   <!--Used to get and run the cookie file on this form-->  
<php $result = file_get_contents(cookies.php); ?>


<form method="post" action="" id="mainForm">
   <fieldset>
      <legend>Unix LARK Submission description</legend>
      <table>
         <tr>
         
      <td colspan="2">
               <p>
               <!--game title textbox-->  
               <label>Title</label><br/>
               <input type="text" name="title" id="title" class="required hilightable" />
               </p>
               <p>
              
              <!--descritpion textbox-->     
                  <label>Description</label><br/>
               <input type="text" name="description" id="description" class="required hilightable">
               </p>
               
               
            </td>
         </tr>
         <tr>
            <td> 
               <p> 
            <!--genre dropdown-->             
      <label>Genre</label><br/>
               <select name="genre" class="hilightable">
                  <option>Choose genre</option> 
                  <option>Puzzle</option>
                  <option>Dungeon Crawler</option>
                  <option>RPG</option>
                  <option>Fighting</option>
               </select>
               </p>
               <p> 
    <!--subject section-->           
                  <label>Subject</label><br/>
             
               <select name="subject" class="hilightable">
                  <option>Choose game subject</option>
                  <option>Survival</option>
                  <option>Best Score</option>
                  <option>Explore</option>
               </select>
               </p>
               
<!--new one  uses the validations in order to run iteself-->
  
<form action="Validations.php" name='mainForm' id='mainForm' method='post'> 
<legend>Required Information:</legend>

<p>
<div class='form-group <?php echoCssError('medium'); ?>'>
      <label for="medium"> Medium </label><br/>
      <input name="medium" id="medium" type="text" class="form-control" class="hilightable required"
             value="<?php echo $_POST['medium']; ?>" />
  </div>
</p>


<!-- old one  
                 <p>	
               <label>Medium</label><br/>               
               <input type="text" name="medium" id="medium" class="hilightable"/>
               </p>
-->


<!-- new one -->
              

<p>
<div class='form-group <?php echoCssError('year'); ?>'>
      <label for="year"> Year Published</label><br/>
      <input name="year" id="year" type="text" class="form-control" class="hilightable required"
             value="<?php echo $_POST['year']; ?>" />
  </div>
</p>

  <!-- 
                    Old one
  <p>	
               <label>Year Published</label><br/>
               <input type="text" name="year" id="year" class="hilightable required"/>
               </p>  
	-->

               <!-- new one  -->

 <p>
<div class='form-group <?php echoCssError('email'); ?>'>
      <label for="email"> Student Email</label>
      <input name="email" id="email" type="text" class="form-control" class="hilightable required"
             value="<?php echo $_POST['email']; ?>" />
  </div>
               </p> 
				<!-- Old student email in case this breaks it
              <label>Student Email</label><br/>
               <input type="text" name="student" id="student" class="hilightable required"/>
               </p> 
              -->
           
     <!-- get and upload the file uses the upload system for how it work -->    
<form action="upload.php" method="post" enctype="multipart/form-data">
  Upload:
  <input type="file" name="fileToUpload" id="fileToUpload">
  <input type="submit" value="Upload" name="submit">
</form>


            </td>
            <td>

            
               <div class="box">
                  <label>Type </label><br/>
                  <input type="radio" name="type" value="1" checked>Linux Bash<br/>
                  <input type="radio" name="type" value="2">WebGL<br/>
               </div>


             
               <div class="box">
                  <label>Creative Commons Specification </label><br/>
                  <input type="checkbox" name="cc" >Attribution <br/>
                  <input type="checkbox" name="cc" >Noncommercial <br/>    
                  <input type="checkbox" name="cc" >No Derivative Works <br/>  
                  <input type="checkbox" name="cc" >Share Alike<br/>
                  <input type="checkbox" name="cc" >Keep Private
               </div> 
               
               
            </td>
         </tr>
 
         <tr>
            <td colspan="2">
               <div class="rectangle centered"> 
                  <input type="submit" class="btn"> <input type="reset" value="Clear Form" class="btn">   
                     
               </div>
            </td>
         </tr>
      </table>
   </fieldset>
   <php $result = file_get_contents(textwrite.php); ?>
</form>
</body>
</html>
