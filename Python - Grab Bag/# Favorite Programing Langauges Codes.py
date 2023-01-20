# Favorite Programing Langauges Codes

# The following is the dictionary code - Done
# this sets up the variables with default values
Name = "name"
Age = 0
Years_coding = 0
# this allows you to input the values
Name = input("Name: ")
Age = input("Age: ")
Years_coding = input("Years: ")

# And this makes and outputs the dictionary 
Dict = {1: Name, 2: Age, 3: Years_coding}
print(Dict)


# store as a tuple - Done 
ProgramingLangsFirst = input("3 First Programing Languages: ")
Tup = tuple(ProgramingLangsFirst.split(" "))
print(Tup)
print(type(Tup))

# store as a list - Done
ProgramingLangsFaves = input("3 Faveorite Programing Languages: ")
Lis = list(ProgramingLangsFaves.split(" "))
print(Lis)
print(type(Lis))

#intersection to a set the list and tuple

#add to dictionary the set then print dict
print(Dict)