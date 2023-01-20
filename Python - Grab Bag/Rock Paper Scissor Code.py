# Rock Paper Scissor Code

import random # need this guy to randomly assign computrs actions 
wins = 0 
loses = 0
draws = 0

while True:
    user_action = input("IT'S A BATTLE FOR THE AGES - Enter a choice TO DANCE WITH DESITNY!!! (Select either rock, paper, scissors): ")
    possible_actions = ["rock", "paper", "scissors"] # note i did rock paper or scissors as i didn't see the r p or s bit, but it's as simplle as changing the rocks to r the paper to p
    computer_action = random.choice(possible_actions) # possible actions picks from the list of rock paper scissors and gives the cpu one of them
    print(f"\nYou chose {user_action}, computer chose {computer_action}.\n")
    if user_action not in ("rock", "paper", "scissors"): # this is used to 
        print("Bless you, now what did you say?")
    if user_action == computer_action: # logic gates needed for the rock paper scisors 
        print(f"Both players selected {user_action}. It's a tie!")
        draws = draws + 1
    elif user_action == "rock":
        if computer_action == "scissors":
            print("Rock smashes scissors! You win!")
            wins = wins + 1
        else:
            print("Paper covers rock! You lose.")
            loses  = loses + 1
    elif user_action == "paper":
        if computer_action == "rock":
            print("Paper covers rock! You win!")
            wins = wins + 1
        else:
            print("Scissors cuts paper! You lose.")
            loses  = loses + 1
    elif user_action == "scissors":
        if computer_action == "paper":
            print("Scissors cuts paper! You win!")
            wins = wins + 1
        else:
            print("Rock smashes scissors! You lose.")
            loses  = loses + 1

    play_again = input("Play again? (y/n): ")
    if play_again.lower() != "y":
        break
print(f'wins = ' +  str(wins) + '!') # outputs the total numbers of wins loses and draws
print(f'loses = ' + str(loses) + '!') 
print(f'ties = ' + str(draws) + '!')

# REALLY COME ON HOW DO YOU DO IT NOT ONCE BUT TWICE