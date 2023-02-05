// JavaScript doc that is going to be used for the the formating

//This defines email format, making sure that it follows the typical character- @ -character. characters formats seen in emails addresses
var emailFormsetup =/^([^@]+([^@])*)@([^@]+(\.[^@]+)*).([^@]+[^@]+(\.[^@]+)*)$/;
//This checkes that a section has only numbers and dashes
var phoneFormsetup = /^[0123456789-]*$/;
//This checks for characters
var hasLetter = /[a-zA-Z]/;
//This checks for numbers
var hasNumber = /[0-9]/;

//The function below Bassically starts all the listenters in the document 
function defineListeners()
{
	var mainForm = document.getElementById("FullForm");

	window.addEventListener("change", capitalize);

    mainForm.addEventListener("submit", checkEmpty);
	mainForm.addEventListener("submit", checkEmail);
	mainForm.addEventListener("submit", checkPass);
	mainForm.addEventListener("submit", checkPhone);
	mainForm.addEventListener("submit", checkRadio);
	mainForm.addEventListener("submit", finalCheck);
}

//This adds one more another listener to make sure everything is loaded in.
window.addEventListener("DOMContentLoaded", defineListeners);

// This is a function for our typing out boxes to change the color of it on click like we did in the participation assignments
function setBackground(prompt)
{
	//If selected, make this a nice shade of yellow
	if(prompt.type == "selected")
		{
			prompt.target.style.backgroundColor = "#D7CB17";
		}
	//If not, go back to normal
	else if(prompt.type == "notselected")
		{
			prompt.target.style.backgroundColor = "white";
		}
}


//Another event listener that, after the webpage is loaded checks the input fields being selected and then sets them so they can be colores
window.addEventListener("load", function (i)
{
	//set the selctions to be all of the input fields
	var selections = "input[type=text], input[type=password]";
	var fields = document.querySelectorAll(selections);
	//Changes the input field color if they are selected / not selected
	for(i = 0; i < fields.length; i++)
	{
		fields[i].addEventListener("selected", setBackground);
		fields[i].addEventListener( "notselected", setBackground);
	}
});


//This function is used to check that fields are not empty for the names
function checkEmpty()
{
	//This is used to display the Error message under the footer.
	var ErrorArea = document.getElementById("errors");
	
	// and theses are the type of errors functions we have
	var firstnameError = document.getElementById("firstnameError");
	var lastnameError = document.getElementById("lastnameError");

	//the below is used to hid the errors so they only display where theere is an erro
    ErrorArea.className = "hidden";
	firstnameError.className = "hidden";
	lastnameError.className = "hidden";

	//The these check the input fields for the names and emails 
	var textinput = "input[id=fname],input[id=lname],input[id=email]";
	var fields = document.querySelectorAll(textinput);

	//cycle through the input fields
	for(var i = 0; i < fields.length;i++)
	{
		//This checks if the current value is empty/null.
		if(fields[i].value == null || fields[i].value == "")
		{
			//set the background to red
			fields[i].style.backgroundColor = "#D72017";
			

			//The new error message is set.
			var msg = "Your missing some form of data! Go and check the above areas!";
		
			//This then displays the error message here for first and last name if they have them
			if(fields[i].id == "fname")
			{
				firstnameError.className = "visible";
			}

			if(fields[i].id == "lname")
			{
				lastnameError.className = "visible";
			}
			//This is then displayed the error using the following 
			ErrorArea.innerHTML = "<p>" + msg + "</p>";
			ErrorArea.className = "visible";
		}
	}
}

//This function checks and captalizes the names automatically for the first and last name.
function capitalize()
{
	//As such, we only select these two fields.
	var names = "input[id=fname], input[id=lname]";
	var fields = document.querySelectorAll(names);
	//so long as the input for names are not empty
	if(fields != null)
	{
		//go through the input
		for(var i = 0; i <fields.length; i++)
		{
			//Then we make another variable that is the string vales of this.
			var string = fields[i].value;
			// make sure string isn't null
			if(string != "") 
			{
				// lastly set the first input to uppercase and then rest to lower
				string = string[0].toUpperCase() + string.slice(1).toLowerCase(); 
				fields[i].value = string;
			}
			
		}
	}
}



//Next is the checkEmail function, in general these formats are mostly the same just using the same sort of input check
function checkEmail()
{
	//This is used to display the Error message under the footer.
	var ErrorArea = document.getElementById("errors");
	var msg = "Your missing some form of data! Go and check the above areas!";

	
	// hid the email error message
	var error = document.getElementById('emailError');
	error.className = "hidden";

	//We get the input from the email
	var email = document.getElementById('email');
	//Check it matches email form setup if not then output the error msg 
	if(!emailFormsetup.test(email.value))
	{
		email.style.backgroundColor = "#D72017";
		error.className = "visible";
		
		ErrorArea.innerHTML = "<p>" + msg + "</p>";
	}
}


//the phone has litterally the same setup as email just the variable changes for them.
function checkPhone()
{
	//This is used to display the Error message under the footer.
	var ErrorArea = document.getElementById("errors");
	var msg = "Your missing some form of data! Go and check the above areas!";
	
	//hid the phone error
	var error = document.getElementById('phoneError');
	error.className = "hidden";

	//get the phone input field
	var phone = document.getElementById('phone');
	// lastly check it matches the input varriable setup
	if(!phoneFormsetup.test(phone.value))
	{
		phone.style.backgroundColor = "#D72017";
		error.className = "visible";
		ErrorArea.innerHTML = msg;
	}
}


//This function checks the password is valid
function checkPass()
{
	//once again the same starting setup is used here 
	var ErrorArea = document.getElementById("errors");
	var msg = "Your missing some form of data! Go and check the above areas!";

	var error = document.getElementById('passError');
	error.className = "hidden";

	var password = document.getElementById('password');
	
	//Now, we check to make sure all criteria are met for the password feild. then if it doesnt work.  
	if(!(password.value.length <= 9 && password.value.length >=6)  || !hasNumber.test(password.value) || !hasLetter.test(password.value))
	{
		password.style.backgroundColor = "#D72017";
		error.className = "visible";
		ErrorArea.innerHTML = "<p>" + msg + "</p>";
	}
}



//This function checks to ensure that a radio button is selected out of the form. 
function checkRadio()
{
	//This is used to display the Error message under the footer.
	var ErrorArea = document.getElementById("errors");
	var msg = "Your missing some form of data! Go and check the above areas!";
	
	//hide the radio error
	var error = document.getElementById('radioError');
	error.className = "hidden";
	
	//This checks to ensure that, prior to displaying an error message, it is ensured that all radio buttons are unpressed.
	if(!document.getElementById("base").checked && !document.getElementById("pro").checked && !document.getElementById("omega").checked)
	{
		error.className = "visible";
		ErrorArea.innerHTML = "<p>" + msg + "</p>";
	}
}


///////////////////////////////////////////////////////////////////////////////////////////


//Checks to ensure all error fields are inactive, and thus, no errors occurred.
function finalCheck(doc)
{
	var emptyError = document.getElementById('errors');

	var firstnameError = document.getElementById('firstnameError');
	
	var lastnameError = document.getElementById('lastnameError');

	var phoneError = document.getElementById('phoneError');
	
	var emailError = document.getElementById('emailError');

	var passError = document.getElementById('passError');

	var radioError = document.getElementById('radioError');

	
	//If all error fields are hidden, that means that there are no errors.
	if(!(emptyError.className == "hidden" && firstnameError.className == "hidden" && lastnameError.className == "hidden" && phoneError.className == "hidden"  && emailError.className == "hidden" && radioError.className == "hidden" && passError.className == "hidden" ))
	{
		//if there is an error then it it goes and displays the error however if there is no error the then it turns it back on 
		doc.preventDefault();
	}
	else
	{
		
		var final = document.getElementById('Final');
		if(final.className == "hidden")
		{
			final.classList.toggle('hidden');
		}
		doc.preventDefault();
	}
}