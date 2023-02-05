#Multi- Leveled OTSU aka a non-binary OTSU method that can be applied to section images
import matplotlib
import matplotlib.pyplot as plt
import numpy as np
import cv2
from skimage import data
from skimage.filters import threshold_multiotsu

# this is used to set the font to make it reaable
matplotlib.rcParams['font.size'] = 9

# The image we want to apply the OTSU to
image = cv2.imread("OTSU_Multi_2.png", 0)

#blur the image to make it clearer?
image = cv2.GaussianBlur(image, (5, 5), 0)

# For Multi OTSU we can simply use the threshold Multiotsu of Skimage class
thresholds = threshold_multiotsu(image)

# Using the threshold values,this gives us the three regions of the image
regions = np.digitize(image, bins=thresholds)

fig, ax = plt.subplots(nrows=1, ncols=3, figsize=(10, 3.5))

# Plotting the original image onto the chart
ax[0].imshow(image, cmap='gray')
ax[0].set_title('Original')
ax[0].axis('off')

# Plotting the histogram
ax[1].hist(image.ravel(), bins=255)
ax[1].set_title('Histogram')
for thresh in thresholds:
    ax[1].axvline(thresh, color='r')

# Plotting the Multi Otsu result.
ax[2].imshow(regions, cmap='jet')
ax[2].set_title('Multi-Otsu result')
ax[2].axis('off')

plt.subplots_adjust()

plt.show()
cv2.waitKey(0) # Wait key so we can actually see the image