import matplotlib.pyplot as plt
import matplotlib.image as mpimg
import cv2
import numpy as np
#%matplotlib inline

# Read in the images
img1 = mpimg.imread('Test1.bmp')
img2 = mpimg.imread('Test2.bmp')
img3 = mpimg.imread('Test3.png')  # BECAUSE YOU JUST HAD TO MAKE EM ALL NOT THE SAME TYPE ARGH... okay i'm good now

# Apply the color conversion to the image
img1 = cv2.cvtColor(img1, cv2.COLOR_BGR2RGB)
img2 = cv2.cvtColor(img2, cv2.COLOR_BGR2RGB)
img3 = cv2.cvtColor(img3, cv2.COLOR_BGR2RGB) # also not technically needed but it's here

# Display Original images
cv2.imshow('Image Original 1', img1)
cv2.imshow('Image Original 2', img2)
cv2.imshow('Image Original 3', img3)


#Apply the box filters to the images
filtered_image_1_3 = cv2.GaussianBlur(img1,(3,3),0)
filtered_image_2_3 = cv2.GaussianBlur(img2,(3,3),0)
filtered_image_3_3 = cv2.GaussianBlur(img3,(3,3),0)

filtered_image_1_5 = cv2.GaussianBlur(img1,(5,5),0)
filtered_image_2_5 = cv2.GaussianBlur(img2,(5,5),0)
filtered_image_3_5 = cv2.GaussianBlur(img3,(5,5),0)

#Output the images

plt.imshow(filtered_image_1_3, cmap='gray')
cv2.imshow('Gau Filtered Image 1-3', filtered_image_1_3)

plt.imshow(filtered_image_1_5, cmap='gray')
cv2.imshow('Gau Filtered Image 1-5', filtered_image_1_5)


plt.imshow(filtered_image_2_3, cmap='gray')
cv2.imshow('Gau Filtered Image 2-3', filtered_image_2_3)

plt.imshow(filtered_image_2_5, cmap='gray')
cv2.imshow('Gau Filtered Image 2-5', filtered_image_2_5)


plt.imshow(filtered_image_3_3, cmap='gray')
cv2.imshow('Gau Filtered Image 3-3', filtered_image_3_3)

plt.imshow(filtered_image_3_5, cmap='gray')
cv2.imshow('Gau Filtered Image 3-5', filtered_image_3_5)

cv2.waitKey(0)
