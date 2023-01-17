# The Highest Number Game  -  Dylan Gartin  - This allows you to play a game against Is-it-int where you have to guess the right number

import random # The random function which is needed to select from the lower_Bound_Bound_Bound and upper_bound bounds
import math # Math makes it where i can use the log function in my game so i can use it to choose a fair ammount of attempts


def invoke_input(Int_based_value, Output_Msg): #to call this function you provide an variable to store the int and an output message
        
    while True: # this forces you to pass this check to continue 
        try: # try the int input
            Int_based_value = int(input(Output_Msg))  # Choose a Number to be the lower_Bound bounding Box of the game.
            break # exit on success
        except ValueError: # get any value errors 
            print("This is no int, it's a con, Now come on put in an interger, (A whole number, can be negative to positives)") # error msg for failure
    return Int_based_value

class HighestNum: # Makes a Default Base Class Case
     
    def __init__(self) -> None: # not sure if this does anything but it also doesnt hurt anyone
         pass
    # with open("Log_File_Num.txt", "w") as file: file.write("This Logs the Highest Number Game files\n") # Used to setup this games log file

    def main_highest_num(file_path):
        print("Hey Kiddo Welcome To Is-it-int, Number Gameo! Pick some Interger Numbers to establish the range your playing under!\n") # Flavor Text Print Statement giving game instructions 
        print("-" * 100) # formatting output
        
        lower_bound = 0 # initliazie the lower bound variable 
        lower_bound = invoke_input(lower_bound,"Give me your Lowest possible Number:  " ) # run the invoking of inputs in order to use the function
        print("Low Value:", lower_bound) # lower Value output just to check it comes out right

        while True: # forces you to pass this check to continue 
            try: # try the int input
                upper_bound = int(input("Give me your Highest possible Number:  ")) # Choose a number to be the highest boundry block
                print("High Value:", upper_bound)
                if upper_bound < lower_bound: # make sure is not a bigger then lower bound one
                    raise Exception # if so go to the exception
                break
            except ValueError: # get any value errors 
                print("This is no int, it's a con, Now come on put in an interger, (A whole number, can be negative to positives)")
            except Exception:
                print ("This number must be bigger, it's called an upper bound for a reason.")


        # Game Setup Variables 
        TheChoosen = random.randint(lower_bound, upper_bound) # Selects a random int in the range 
        attempts = round(math.log(upper_bound - lower_bound + 1,2)) # this makes the numbers of attempts a nice fair value
        count = 0 # starts the number of guesses at 0 and this will increment each guess
        

        print("\nYou've only ", attempts, " chances to guess that bad int, good luck!\n") # does the log of upper_bound - lower_Bound + 1 , e = 2.7 ish, could make it harder by allowing a challenege rating here
        print("-" * 100) # formatting output

        # Number of Guesses allowed by the system is less then the number of counts
        while count < attempts :
            count += 1 # add one to the count as to 

            guess = 0
            guess = invoke_input(guess,"Guess that number or make a blunder!: " ) # allow the user to guess the int
            print("Guess Value:", guess)
            print("-" * 100) 

            # The following if statemnet below will output the feedback needed for the numbers
            if TheChoosen  == guess: # If the numbers 
                print("-" * 100) # formatting output
                print("Darn it you got it, and in only", count, " try or tries, Congrats, I guess.") # print your victory statement 
                with open("Log_File_Num.txt", "at") as file: file.write(f"------------------------------------------------------------------------------------------------------"  "\n ") 
                with open("Log_File_Num.txt", "at") as file: file.write(f"- Num - Game Recorded as a WIN - Num - \n Hidden Number Value {TheChoosen}  \n Total Number of Guesses -  {attempts} \n Guesses Used -  {count}\n") 
                break # Once correct break the loop
            elif TheChoosen  > guess: #  if the choosen number is Higher then your guess
                print("Nope your going for the floor kid, go a little lower!") # Should Guess higher! this guy is a liar and try to throw you off the number by saying advice that makes you think the other way
            elif TheChoosen  < guess: #  if the choosen number is lower_Bound then your guess
                print("Nope your going for the clouds kid, go a little higher!") # Should guess lower_Bound! HE A CON MAN A CONNNN

        #  Once your over the max number of tries
        if count >= attempts: # when the count excededs allowed attempts
            print("-" * 100) # formatting output
            print("\nThe number is,", TheChoosen , "Ooo, sorry Better Luck Next time Kid!") # output the failure text 
            with open("Log_File_Num.txt", "at") as file: file.write(f"------------------------------------------------------------------------------------------------------"  "\n ") 
            with open("Log_File_Num.txt", "at") as file: file.write(f"- Num - Game Recorded as a LOSS - Num - \n Hidden Number Value {TheChoosen}  \n Total Number of Guesses -  {attempts} \n Guesses Used -  {count}\n") 
             
        

