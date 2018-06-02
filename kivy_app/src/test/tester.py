#from src.analyzer.imageProcessor import PreProcessor
#from src.analyzer.imageProcessor import PostProcessor
import cv2
import matplotlib.pyplot as plt
import numpy as np

# This class is designed for unit testing various aspects 
# of uCounter
class tester: 
	testImg = cv2.imread('samples/Mix_Well1_2Steel_new.tif')
	def testPreprocessor(self):
		pass

	def histogramValues(self, image):
		gray = cv2.cvtColor(image, cv2.COLOR_BGR2GRAY)
		plt.hist(gray.ravel(), bins=256, range=(0, 255), fc='k', ec='k')
		plt.show()
		cv2.imshow('gray', gray)
		cv2.waitKey(0)
	def testColorThresholding(self, image, color1, color2):
		mask = cv2.inRange(image, color1, color2)
		result = cv2.bitwise_and(image, image, mask=mask)
		print('Color1:')
		print(color1)
		print('Color2:')
		print(color2)
		cv2.imshow('mask', mask)
		cv2.imshow('result', result)
		cv2.waitKey(0)


