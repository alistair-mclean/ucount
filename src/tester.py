from src.preProcessor import PreProcessor
import cv2
import numpy as np
class tester: 
	def testPreprocessor(self):
		pass

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


ima = tester()
ima.testPreprocessor()
	