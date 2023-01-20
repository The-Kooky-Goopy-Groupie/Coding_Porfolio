num = 100 # so it goes to num

for i in range(1, num+1):
    if (i % 3 == 0)  and (i % 5 == 0):
        if i % 15 == 0:
            print ("Fizzbuzz")
        if i % 3 == 0:
            print ("Fizz")
        if i % 3 == 0:
            print ("Buzz")