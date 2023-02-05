<?php

class Vehicle {

      protected $make;
      protected $model;
      protected $fuel;
      protected $topSpeed;

      function __construct($mk, $md, $f, $spd) {
         $this->make = $mk;
         $this->model = $md;
         $this->fuel = $f;
         $this->topSpeed = $spd;
      }
      class LandVehicle extends Vehicle {
        private $wheels;
  
        function __construct($mk, $md, $f, $spd, $whlCount) {
           parent::__construct($mk, $md, $f, $spd);
           $this->wheels = $whlCount;
        }
  }
  
  function __toString(){
    return 
       '<div class="panel panel-default col-lg-3 col-md-4 col-sm-6">
           <div class="panel-heading">
              <h3 class="panel-title">'.$this->make.'</h3>
           </div>
           <div class="panel-body">
               <table class="table table-hover">
                 <tr><td>Model:</td><td>'.$this->model.'</td></tr>
                   <tr><td>Fuel:</td><td>'.$this->fuel.'</td></tr>
                 <tr><td>Top Speed:</td><td>'. $this->topSpeed .
                           ' Mph</td></tr>
                 <tr><td>Wheels:</td><td>'.$this->wheels.'</td></tr>
               </table>
          </div>
       </div>';
 }
 


      function __toString(){
         return 
           '<div class="panel panel-default col-lg-3 col-md-4 col-sm-6">
               <div class="panel-heading">
                  <h3 class="panel-title">'.$this->make.'</h3>
              </div>
              <div class="panel-body">
                <table class="table table-hover">
                  <tr><td>Model:</td><td>'.$this->model.'</td></tr>
	            <tr><td>Fuel:</td><td>'.$this->fuel.'</td></tr>
                 <tr><td>Top Speed:</td><td>'.$this->topSpeed.' Mph</td></tr>
                 <tr><td>Wheels:</td><td>'.$this->wheels.'</td></tr>
               </table>
             </div>
          </div>';
      }

}

class WaterVehicle extends Vehicle{
  private $capacity;
  private $lifeBoatCapacity;

  function __construct($mk, $md, $f, $spd,$cap, $lifeboat) {
    parent::__construct($mk, $md, $f, $spd);
      $this->capacity = $cap;
    $this->lifeBoatCapacity = $lifeboat;
  }

  function __toString(){
    return 
     '<div class="panel panel-default col-lg-3 col-md-4 col-sm-6">
          <div class="panel-heading">
            <h3 class="panel-title">'.$this->make.'</h3>
          </div>
          <div class="panel-body">
            <table class="table table-hover">
             <tr><td>Model:</td><td>'.$this->model.'</td></tr>
               <tr><td>Fuel:</td><td>'.$this->fuel.'</td></tr>
             <tr><td>Top Speed:</td><td>' . $this->topSpeed . 
                       ' Knots</td></tr>
               <tr><td>Capacity:</td><td>'.$this->capacity.'</td></tr>
               <tr><td>Life Boat Capacity:</td><td>' . 
                     $this->lifeBoatCapacity . '</td></tr>
            </table>
          </div>
    </div>';
    }
}

            
?>