#The Following is a Binary Version of OTSU - Known  as OTSU 2 Made  by Dylan Gartin

import cv2
import numpy as np # we need this for the array again
import matplotlib.pyplot as plt #Not really needed for this one but keeping it in for future projects
import matplotlib.image as mpimg

# Read the image in a grayscale mode - # NOTE CHANGE THE IMAGE NAME TO CHANGE WHICH IMAGE USED
image = cv2.imread("OTSU_1.png", 0)

# Apply GaussianBlur to reduce image noise to have simpler data to work with
image = cv2.GaussianBlur(image, (5, 5), 0)

# Set total number of bits that are in the histogram, since we greyscale the image, this is 256
bins_num = 256

# Get the histogram of the image, this is used to determine where to place the threshold value
hist, bin_edges = np.histogram(image, bins=bins_num)

# Calculate centers of bits, aka what would be the middle of values.
bin_mids = (bin_edges[:-1] + bin_edges[1:]) / 2.

# Iterate over all thresholds (indices) and get the probabilities.
weight1 = np.cumsum(hist)
weight2 = np.cumsum(hist[::-1])[::-1]

#get the means of these using the cumulative sum
mean1 = np.cumsum(hist * bin_mids) / weight1
mean2 = (np.cumsum((hist * bin_mids)[::-1]) / weight2[::-1])[::-1]

# then combine them together
inter_class_variance = weight1[:-1] * weight2[1:] * (mean1[:-1] - mean2[1:]) ** 2

# Maximize the threshold by using this inter_class_variance function val
index_of_max_val = np.argmax(inter_class_variance)
threshold = bin_mids[:-1][index_of_max_val]
print("Otsu's algorithm implementation thresholding result: ", threshold)

# Applying Otsu's method by using the above data point that we got for the threshold and having this passed in for that value.
otsu_threshold, image_result = cv2.threshold(
    image, 0, 255, cv2.THRESH_BINARY + cv2.THRESH_OTSU,
)
#print this threshold as well to check it alings with our data
print("Obtained threshold: ", otsu_threshold)
cv2.imshow('Image', image) #Output the blured and greyscale img
cv2.imshow('OTSU_2_RESULTS', image_result) #Output the OTSU_2 Method results
cv2.waitKey(0) # Wait key so we can actually see the image