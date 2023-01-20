import datetime

with open("employees.csv", "w") as file:
  file.write("first_name,last_name,age,email\n")

def add_new_employees(file_path):
  while True:
    try:
      num = int(input("How many new employees will you add? "))
    except ValueError:
      print("Please enter an integer.")
    else:
      break

  for i in range(num):
    first = input("First name: ")
    last = input("Last name: ")

    while True:
      try:
        age = int(input("Age: "))
      except ValueError:
        print("Please enter an integer.")
      else:
        break

    email = f"{first}.{last}{str(datetime.date.today().year - age)[-2:]}@company.com"

    with open(file_path, "at") as file:
      file.write(f"{first},{last},{age},{email}\n")

    print(f"{first} {last} added to records with the email '{email}'")

add_new_employees("employees.csv")

with open("employees.csv") as file:
  print(file.read())