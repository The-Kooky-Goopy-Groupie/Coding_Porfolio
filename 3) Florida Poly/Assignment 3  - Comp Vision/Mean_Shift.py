#The Following is mean shift applied with the base openCV libarary By Dylan Gartin
import cv2 as cv2

image = cv2.imread("Mean_1.png") # Input Image here, replace with image wanting testing

# filter to reduce noise
# image= cv2.medianBlur(image, 3) #blur i just find is helping it come out clearer based on noise.

image_result = cv2.pyrMeanShiftFiltering(image, 20, 40) #So open CV was just hiding this mother of a duck.
cv2.imshow('Intial_img', image) # baseline Image
cv2.imshow('MeanShift_RESULTS', image_result) #Output the OTSU_2 Method results
cv2.waitKey(0)