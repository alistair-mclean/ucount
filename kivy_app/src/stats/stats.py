import cv2
import numpy as np
import matplotlib.pyplot as plt



def histogramImage(image):
	gray = cv2.cvtColor(image, cv2.COLOR_BGR2GRAY)
	blue, green, red = cv2.split(image)

	equ = cv2.equalizeHist(gray)
	res = np.hstack((gray,equ))
	histr1 = cv2.calcHist([gray],[0], None, [256],[0,256])
	histr2 = cv2.calcHist([red],[0], None, [256],[0,256])
	histr3 = cv2.calcHist([green],[0], None, [256],[0,256])
	histr4 = cv2.calcHist([blue],[0], None, [256],[0,256])

	color = ('b', 'g', 'r')
	b = image.copy()
	# set green and red channels to 0
	b[:, :, 1] = 0
	b[:, :, 2] = 0


	g = image.copy()
	# set blue and red channels to 0
	g[:, :, 0] = 0
	g[:, :, 2] = 0

	gGray = cv2.cvtColor(g, cv2.COLOR_BGR2GRAY)
	clahe = cv2.createCLAHE(clipLimit=2.0, tileGridSize=(8,8))
	gGrayEqu = cv2.equalizeHist(gGray)
	gcl = clahe.apply(gGray)
	gBlur = cv2.blur(g, (5,5))
	r = image.copy()
	# set blue and green channels to 0
	r[:, :, 0] = 0
	r[:, :, 1] = 0
	rGray = cv2.cvtColor(r, cv2.COLOR_BGR2GRAY)
	rGrayEqu = cv2.equalizeHist(rGray)
	rcl = clahe.apply(rGray)
	rBlur = cv2.blur(r, (5,5))
	# RGB - Blue
#	cv2.imshow('B-RGB', b)

	# RGB - Green
	cv2.imshow('G-RGB', g)
#	cv2.imshow('G-GRAY', gGray)
#	cv2.imshow('G-GRAY-EQUI', gGrayEqu)
	cv2.imshow('G-CLAHE', gcl)
	cv2.imshow('G-CLAHE-BLUR', gBlur)

	cv2.imwrite('../../results/G-RGB.png', g)
	cv2.imwrite('../../results/G-GRAY.png', gGray)
	cv2.imwrite('../../results/G-GRAY-EQUI.png', gGrayEqu)
	#cv2.imwrite('../../results/G-COMPARE.png', gRes)


	# RGB - Red
	cv2.imshow('R-RGB', r)
#	cv2.imshow('R-GRAY', rGray)
#	cv2.imshow('R-GRAY-EQUI', rGrayEqu)
	cv2.imshow('R-CLAHE', rcl)
	cv2.imshow('R-CLAHE-BLUR', rBlur)

	cv2.imwrite('../../results/R-RGB.png', r)
	cv2.imwrite('../../results/R-GRAY.png', rGray)
	cv2.imwrite('../../results/R-GRAY-EQUI.png', rGrayEqu)
	#cv2.imwrite('../../results/R-COMPARE.png', rRes)

	print('Green SNR: ', calculateImageSNR(gGray))
	print('Red SNR: ', calculateImageSNR(rGray))
	cv2.waitKey(0)
	cv2.destroyAllWindows()

	for i,col in enumerate(color):
		histr = cv2.calcHist([image], [i], None, [256], [2, 256])
		plt.subplot(121)
		plt.title('Pixel Histogram')
		plt.plot(histr, color = col)
		plt.xlim([2, 256])
		plt.xlabel('pixel intensity')
		plt.ylabel('pixel count')
		probDist = probabilityDist(histr)
		plt.subplot(122)
		plt.title('Cumulative Distribution Histogram')
		plt.xlabel('pixel intensity')
		plt.ylabel('normalized pixel frequency')
		plt.plot(probDist, color = col)
	plt.show()


def probabilityDist(array):
	n = len(array)
	pDist = []
	totalPixels = 0
	# Calculate the total number of pixels 
	for i in range(n):
		totalPixels += array[i]
	
	# Create the probability distribution 
	for i in range(n):
		pDist.append(array[i]/totalPixels)

	return pDist


def calculateImageSNR(image):
	# Check if image is already grayscale, if not
	# then apply this 
#	gray = cv2.cvtColor(image, cv2.COLOR_BGR2GRAY)
	mean = np.mean(image, dtype=np.float64)
	stdDev = np.std(image, dtype=np.float64)
	print('Mean = ',mean)
	print('STD = ',stdDev)
	SNR = mean / stdDev
	return SNR

def calcMeanOfHist(array):
	n = len(array)
	SUM = 0
	for i in range(n):
		SUM += array[i] * i
	return SUM / n	

testImg = cv2.imread('../../samples/Mix_Well1_2Steel_new.tif')
testImg2 = cv2.imread('../../samples/Mix_Well1_3Teflon_new.tif')
testImg3 = cv2.imread('../../samples/Mix_Well1_2Steel_new.tif')
histogramImage(testImg)
#histogramImage(testImg2)
